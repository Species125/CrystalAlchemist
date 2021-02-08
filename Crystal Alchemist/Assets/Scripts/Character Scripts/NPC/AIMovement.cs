using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum MovementPriority
    {
        target,
        partner
    }

    public class AIMovement : AIComponent
    {
        public enum PatrolType
        {
            path,
            area
        }

        #region Parameter fürs Verfolgen
        [BoxGroup("Character Movement")]
        [SerializeField]
        private MovementPriority movementPriority = MovementPriority.target;

        [BoxGroup("Character Movement")]
        [SerializeField]
        private float targetRadius = 0.1f;

        [BoxGroup("Character Movement")]
        [SerializeField]
        private float partnerRadius = 0.1f;

        [BoxGroup("Character Movement")]
        [SerializeField]
        private float delay = 0;

        [BoxGroup("Character Movement")]
        [SerializeField]
        private bool teleportWithPartner = false;

        [ShowIf("teleportWithPartner")]
        [BoxGroup("Character Movement")]
        [SerializeField]
        private float maxTeleportDistance = 4f;





        [BoxGroup("Movement Attributes")]
        [SerializeField]
        private bool backToStart = true;

        [ShowIf("backToStart")]
        [BoxGroup("Movement Attributes")]
        [SerializeField]
        private float returnDelay = 3f;



        [BoxGroup("Patrol")]
        [SerializeField]
        [OnValueChanged("SetReturn")]
        private bool isPatrol = false;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        [SerializeField]
        private PatrolType patrolType = PatrolType.path;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        [SerializeField]
        private float patrolDelay = 3f;

        [ShowIf("isPatrol")]
        [HideIf("patrolType", PatrolType.area)]
        [BoxGroup("Patrol")]
        [SerializeField]
        private List<Transform> patrolPath = new List<Transform>();

        [ShowIf("isPatrol")]
        [HideIf("patrolType", PatrolType.path)]
        [BoxGroup("Patrol")]
        [SerializeField]
        private Collider2D patrolArea;

        [ShowIf("isPatrol")]
        [HideIf("patrolType", PatrolType.path)]
        [BoxGroup("Patrol")]
        [SerializeField]
        private bool hasMaxTime = false;

        [ShowIf("isPatrol")]
        [HideIf("patrolType", PatrolType.path)]
        [ShowIf("hasMaxTime", true)]
        [BoxGroup("Patrol")]
        [SerializeField]
        private float maxPatrolTime = 5f;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        [SerializeField]
        private float followPathPrecision = 0.01f;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        [HideIf("patrolType", PatrolType.area)]
        [SerializeField]
        private bool followPathInCircle = true;

        [BoxGroup("Pathfinding")]
        [SerializeField]
        private bool usePathfinding = true;

        [BoxGroup("Pathfinding")]
        [ShowIf("usePathfinding")]
        [SerializeField]
        private float accuracy = 0.25f;

        [BoxGroup("Pathfinding")]
        [ShowIf("usePathfinding")]
        [SerializeField]
        private float updateDelay = 0.5f;

        private PathSeeker seeker = null;
        private List<Vector2> path;
        private int index;

        private bool wait = false;
        private bool enableCoroutine = true;

        private int factor = 1;
        private int currentPoint = 0;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private Vector3 targetPosition;
        private Vector3 randomColliderPoint;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private float areaCountdown = 0f;

        #endregion

        public override void Initialize()
        {
            base.Initialize();

            if (!NetworkUtil.IsMaster()) return;

            AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", false);
            if (Pathfinding.Instance != null) this.seeker = this.GetComponent<PathSeeker>();

            if (this.usePathfinding && this.seeker != null) InvokeRepeating("UpdatePath", 0, this.updateDelay);

            this.targetPosition = MasterManager.globalValues.nullVector;

            if (this.isPatrol && this.patrolType == PatrolType.area) SetRandomPoint();
        }

        #region Update und Movement Funktionen
        private void FixedUpdate()
        {
            if (!NetworkUtil.IsMaster()) return;

            if (this.npc.values.currentState != CharacterState.knockedback && !this.npc.values.isOnIce)
            {
                if (this.npc.myRigidbody.bodyType != RigidbodyType2D.Static) this.npc.myRigidbody.velocity = Vector2.zero;
            }

            if (!this.wait)
            {
                if (this.hasMaxTime && this.areaCountdown > 0) this.areaCountdown -= Time.fixedDeltaTime;

                UpdateTargetPosition();

                if (this.usePathfinding && this.seeker != null) MoveTroughPaths(); //Pathfinding
                else MoveToPosition(this.targetPosition); //No Pathfinding
            }

            if (MasterManager.debugSettings.showTargetPosition
                && this.targetPosition != MasterManager.globalValues.nullVector)
                Debug.DrawLine(this.npc.GetGroundPosition(), this.targetPosition, Color.blue);
        }

        private void SetReturn()
        {
            if (this.isPatrol) this.backToStart = false;
        }

        private void UpdateTargetPosition()
        {
            if (this.npc.values.currentState != CharacterState.dead
                && this.npc.values.currentState != CharacterState.knockedback
                && this.npc.values.currentState != CharacterState.manually
                && this.npc.values.currentState != CharacterState.respawning)
            {
                SetNextPoint();
                this.npc.values.currentState = CharacterState.idle;
            }
        }

        private void SetNextPoint()
        {
            Vector3 patrolVector = MasterManager.globalValues.nullVector;
            Vector3 spawnVector = MasterManager.globalValues.nullVector;
            Vector3 chaseVector = MasterManager.globalValues.nullVector;

            Character target = this.npc.GetTarget();

            if (this.isPatrol)
            {
                if (this.patrolType == PatrolType.path && this.patrolPath.Count > 0)
                    patrolVector = GetNextPoint(this.patrolPath[currentPoint].position, this.followPathPrecision, SetNextWayPoint);
                else if (this.patrolType == PatrolType.area)
                {
                    SetNewRandomPointAfterTime();
                    patrolVector = GetNextPoint(this.randomColliderPoint, this.followPathPrecision, SetRandomPoint);
                }
            }
            if (this.backToStart) spawnVector = GetNextPoint(this.npc.GetSpawnPosition(), 0.25f);

            if (this.movementPriority == MovementPriority.partner)
            {
                chaseVector = CheckTargets(this.npc.partner, this.partnerRadius, target, this.targetRadius);
                if (this.teleportWithPartner && Vector2.Distance(this.npc.GetGroundPosition(), this.npc.partner.GetGroundPosition()) > this.maxTeleportDistance)
                    this.npc.transform.position = this.npc.partner.GetGroundPosition();
            }
            else
                chaseVector = CheckTargets(target, this.targetRadius, this.npc.partner, this.partnerRadius);

            SetVector(patrolVector, spawnVector, chaseVector);
        }

        private void SetNewRandomPointAfterTime()
        {
            if (this.hasMaxTime && this.areaCountdown <= 0) SetRandomPoint();
        }

        private void SetVector(Vector3 patrolVector, Vector3 spawnVector, Vector3 chaseVector)
        {
            Vector3 newVector = MasterManager.globalValues.nullVector;
            float delay = 0f;

            SetVector(patrolVector, this.patrolDelay, ref newVector, ref delay);
            SetVector(spawnVector, this.returnDelay, ref newVector, ref delay);
            SetVector(chaseVector, this.delay, ref newVector, ref delay);

            if (newVector != this.targetPosition)
            {
                this.targetPosition = newVector;
                if (this.enableCoroutine) StartCoroutine(delayMovementCo(delay));
            }
        }

        private void SetVector(Vector3 vector, float delay, ref Vector3 newVector, ref float newDelay)
        {
            if (vector != MasterManager.globalValues.nullVector)
            {
                newVector = vector;
                newDelay = delay;
            }
        }

        #region Check Distances

        private Vector3 CheckTargets(Character first, float firstRadius, Character second, float secondRadius)
        {
            if (first != null) return GetNextPoint(first.GetGroundPosition(), firstRadius);
            else if (second != null) return GetNextPoint(second.GetGroundPosition(), secondRadius);
            else
            {
                this.enableCoroutine = true;
                return MasterManager.globalValues.nullVector;
            }
        }

        private Vector3 GetNextPoint(Vector2 nextPosition, float maxDistance)
        {
            return GetNextPoint(nextPosition, maxDistance, null);
        }

        private Vector3 GetNextPoint(Vector2 nextPosition, float maxDistance, Action action)
        {
            float distance = Vector2.Distance(this.npc.GetGroundPosition(), nextPosition);

            if (distance > maxDistance) return nextPosition;
            else
            {
                if (action != null) action.Invoke();
                return MasterManager.globalValues.nullVector;
            }
        }

        #endregion

        private IEnumerator delayMovementCo(float delay)
        {
            this.enableCoroutine = false;
            this.wait = true;
            yield return new WaitForSeconds(delay);
            this.wait = false;
        }

        private void UpdatePath()
        {
            Vector2 currentPos = this.npc.GetGroundPosition();
            this.path = this.seeker.FindPath(currentPos, this.targetPosition);
        }

        private void MoveTroughPaths()
        {
            if (this.path != null && this.index >= this.path.Count) this.path = null;

            if (path != null)
            {
                Vector2 pos = this.path[this.index];
                Vector2 currentPos = this.npc.GetGroundPosition();

                if (Vector2.Distance(currentPos, pos) > this.accuracy) MoveToPosition(pos);
                else this.index++;
            }
        }

        private void MoveToPosition(Vector3 position)
        {
            if (this.npc.values.CanMove()
                && position != MasterManager.globalValues.nullVector)
            {
                Vector2 direction = ((Vector2)position - this.npc.GetGroundPosition()).normalized;

                Vector2 movement = new Vector2(direction.x, direction.y + (this.npc.values.steps * direction.x));
                if (!this.npc.values.isOnIce) this.npc.myRigidbody.velocity = (movement * this.npc.values.speed * this.npc.values.timeDistortion);

                updateAnimation(direction);
            }
        }

        private void updateAnimation(Vector2 direction)
        {
            if (!this.npc.values.isCharacterStunned()) this.npc.ChangeDirection(direction.normalized);

            this.npc.values.currentState = CharacterState.walk;
            AnimatorUtil.SetAnimatorParameter(this.npc.animator, "isWalking", true);
        }

        private void SetNextWayPoint()
        {
            if (currentPoint == patrolPath.Count - 1) //letzter Knoten
            {
                if (this.followPathInCircle)
                {
                    this.currentPoint = 0;
                }
                else
                {
                    this.factor = -1; //rückwärts laufen
                    currentPoint += this.factor;
                }
            }
            else if (currentPoint == 0)//erster Knoten 
            {
                if (!this.followPathInCircle)
                {
                    this.factor = 1; //reset to normal
                }
                currentPoint += this.factor;
            }
            else
            {
                currentPoint += this.factor;
            }
        }

        private void SetRandomPoint()
        {
            if (this.patrolArea == null)
            {
                this.randomColliderPoint = MasterManager.globalValues.nullVector;
                return;
            }

            Bounds bounds = this.patrolArea.bounds;
            Vector2 center = bounds.center;
            bool found = false;
            int attempts = 0;

            do
            {
                float x = UnityEngine.Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
                float y = UnityEngine.Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
                Vector2 result = new Vector2(x, y);
                attempts++;

                if (this.patrolArea.OverlapPoint(result))
                {
                    this.randomColliderPoint = result;
                    this.areaCountdown = this.maxPatrolTime;
                    found = true;
                }
            }
            while (!found && attempts < 50);
        }

        #endregion

    }
}