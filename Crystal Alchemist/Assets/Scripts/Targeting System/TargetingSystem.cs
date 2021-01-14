using System.Collections;
using System.Collections.Generic;
using System.Linq;




using UnityEngine;

namespace CrystalAlchemist
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D circleCollider;

        [SerializeField]
        private PolygonCollider2D viewCollider;

        private Character sender;
        public List<Character> selectedTargets = new List<Character>();
        public List<Character> allTargetsInRange = new List<Character>();

        private int index;
        private bool selectAll = false;
        private bool inputPossible = true;
        private FloatValue timeLeftValue;
        private TargetingProperty properties;
        private Ability ability;
        private float timeLeft = 60f;
        private Vector2 change;

        #region Unity Functions

        public void Initialize(Character sender)
        {
            this.sender = sender;
        }

        public void SetTimeValue(FloatValue timeValue)
        {
            this.timeLeftValue = timeValue;
        }

        public void SetTargetChange(Vector2 change)
        {
            this.change = change;
        }


        private void OnEnable()
        {
            ability.SetLockOnState();
            SetColliders();

            if (this.properties.hasMaxDuration) this.timeLeft = this.properties.maxDuration;
            if (this.timeLeftValue != null) this.timeLeftValue.SetValue(1f);

            StartCoroutine(delayCo());
        }

        private void Update()
        {
            this.ability.SetLockOnState();
            this.allTargetsInRange.RemoveAll(item => item == null);
            this.allTargetsInRange.RemoveAll(item => item.gameObject.activeInHierarchy == false);
            this.allTargetsInRange.RemoveAll(item => item.values.isInvincible);  //WHY?

            RotationUtil.rotateCollider(this.sender, this.viewCollider.gameObject);

            if (this.properties.targetingMode == TargetingMode.auto) selectAllNearestTargets();
            else if (this.properties.targetingMode == TargetingMode.manual) selectTargetManually();

            updateIndicator();
            updateTimer();
        }

        private void OnDisable() => this.ability.HideIndicator();

        #endregion


        #region get set

        private void SetColliders()
        {
            this.circleCollider.gameObject.SetActive(false);
            this.viewCollider.gameObject.SetActive(false);

            if (this.properties.rangeType == RangeType.circle) this.circleCollider.gameObject.SetActive(true);
            else if (this.properties.rangeType == RangeType.view) this.viewCollider.gameObject.SetActive(true);

            this.circleCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);
            this.viewCollider.transform.localScale = new Vector3(this.properties.range, this.properties.range, 1);
        }

        public void setParameters(Ability ability)
        {
            this.properties = Instantiate(ability.targetingProperty);
            this.ability = ability;
        }

        public float GetTimeLeft()
        {
            return this.timeLeft;
        }

        public List<Character> getTargets()
        {
            List<Character> targets = new List<Character>();

            foreach (Character target in this.selectedTargets)
            {
                if (target != null && target.gameObject.activeInHierarchy) targets.Add(target);
            }
            return targets;
        }

        public float getDelay()
        {
            if (this.properties != null) return this.properties.multiHitDelay;
            return 0;
        }

        #endregion


        #region Target Functions

        private List<Character> sortTargets()
        {
            List<Character> result = this.allTargetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();
            result.RemoveAll(item => item.gameObject.activeInHierarchy == false);
            result.RemoveAll(item => item.values.isInvincible);  //WHY?
            return result;
        }

        private void selectNextTarget(int value)
        {
            this.selectedTargets.Clear();
            List<Character> sorted = sortTargets();

            if (this.allTargetsInRange.Count > 0)
            {
                this.index += value;
                if (this.index >= sorted.Count) this.index = 0;
                else if (index < 0) this.index = sorted.Count - 1;

                addTarget(sorted[this.index]);
            }
        }

        private void selectTargetManually()
        {
            if (this.selectAll) selectAllNearestTargets();
            else selectNextTarget(0);

            if (this.inputPossible && this.change != Vector2.zero)
            {
                if (this.change.y != 0) //switch modes
                {
                    StartCoroutine(delayCo());

                    if (this.selectAll) this.selectAll = false;
                    else this.selectAll = true;
                }

                if (this.change.x != 0) //switch targets
                {
                    StartCoroutine(delayCo());
                    selectNextTarget((int)this.change.x);
                }
            }
        }

        private void selectAllNearestTargets()
        {
            this.selectedTargets.Clear();
            List<Character> sorted = sortTargets();

            for (int i = 0; i < this.properties.maxAmountOfTargets && i < sorted.Count; i++)
            {
                addTarget(sorted[i]);
            }
        }

        public void removeTarget(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null && CollisionUtil.checkCollision(collision, this.ability.skill, this.sender))
            {
                if (this.allTargetsInRange.Contains(character)) this.allTargetsInRange.Remove(character);
                if (this.selectedTargets.Contains(character)) this.selectedTargets.Remove(character);
            }
        }

        public void addTarget(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (character != null && !character.values.isInvincible && CollisionUtil.checkCollision(collision, this.ability.skill, this.sender))  //WHY?
            {
                if (!this.allTargetsInRange.Contains(character)) this.allTargetsInRange.Add(character);
            }

            this.allTargetsInRange.RemoveAll(item => item == null);
            this.allTargetsInRange = this.allTargetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();
        }

        private void addTarget(Character character)
        {
            if (!this.selectedTargets.Contains(character)) this.selectedTargets.Add(character);
        }

        #endregion


        #region Indicator Functions

        private void updateTimer()
        {
            if (this.properties.hasMaxDuration)
            {
                if (this.timeLeft > 0) this.timeLeft -= Time.deltaTime;
                else Deactivate();

                if (this.timeLeftValue != null) this.timeLeftValue.SetValue(this.timeLeft / this.properties.maxDuration);
            }
        }

        private void updateIndicator()
        {
            this.ability.ShowTargetingIndicator(this.selectedTargets);
        }


        #endregion

        public void Deactivate()
        {
            this.ability.ResetLockOn();
            this.ability.state = AbilityState.onCooldown;
            this.gameObject.SetActive(false);
        }


        private IEnumerator delayCo()
        {
            this.inputPossible = false;
            yield return new WaitForSeconds(0.2f);
            this.inputPossible = true;
        }

    }
}
