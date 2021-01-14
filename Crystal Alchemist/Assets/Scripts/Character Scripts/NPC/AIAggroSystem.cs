using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class AIAggroSystem : MonoBehaviour
    {
        #region attributes
        [SerializeField]
        [Required]
        [BoxGroup("Required")]
        private AggroStats aggroStats;

        [SerializeField]
        [BoxGroup("Required")]
        [Required]
        private AI npc;

        private AggroClue activeClue;
        #endregion


        #region Start und Update

        private void Start()
        {
            this.npc.aggroList.Clear();

            GameEvents.current.OnAggroHit += increaseAggroOnHit;
            GameEvents.current.OnAggroIncrease += increaseAggro;
            GameEvents.current.OnAggroDecrease += decreaseAggro;
            GameEvents.current.OnAggroClear += ClearAggro;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnAggroHit -= increaseAggroOnHit;
            GameEvents.current.OnAggroIncrease -= increaseAggro;
            GameEvents.current.OnAggroDecrease -= decreaseAggro;
            GameEvents.current.OnAggroClear -= ClearAggro;
        }

        private void Update()
        {
            if (this.GetComponent<CircleCollider2D>() == null) RotationUtil.rotateCollider(this.npc, this.gameObject);
            generateAggro();
        }

        #endregion


        #region Aggro-System    

        private void addAggro(Character newTarget, float aggro)
        {
            if (newTarget != null && this.npc.aggroList.ContainsKey(newTarget)) this.npc.aggroList[newTarget][0] += (float)(aggro / 100);
        }

        public void getHighestAggro(out float aggro, out string target)
        {
            aggro = 0;
            target = "";

            foreach (Character character in this.npc.aggroList.Keys)
            {
                float currentAggro = this.npc.aggroList[character][0];

                if (currentAggro > aggro)
                {
                    aggro = currentAggro;
                    target = character.GetCharacterName();
                }
            }
        }

        private void OnDisable()
        {
            this.npc.target = null;
            this.HideClue();
            this.npc.aggroList.Clear();
        }

        private bool IsValidTarget(Character target)
        {
            if (target == null
                || !target.gameObject.activeInHierarchy
                || target.values.isInvincible  //WHY?
                || target.values.currentState == CharacterState.dead
                || target.values.currentState == CharacterState.respawning) return false;

            return true;
        }

        private void generateAggro()
        {
            List<Character> charactersToRemove = new List<Character>();

            if (this.npc.target != null && !IsValidTarget(this.npc.target))
            {
                this.npc.aggroList.Remove(this.npc.target);
                this.npc.target = null;
            }

            foreach (Character character in this.npc.aggroList.Keys)
            {
                //                       amount                         factor
                addAggro(character, this.npc.aggroList[character][1] * (Time.deltaTime * this.npc.values.timeDistortion));

                //if (this.enemy.aggroList[character][0] > 0) Debug.Log(this.characterName + " hat " + this.enemy.aggroList[character][0] + " Aggro auf" + character.characterName);

                if (this.npc.aggroList[character][0] >= (this.aggroStats.aggroNeededToTarget / 100))
                {
                    //this.enemy.aggroList[character][0] = 1f; //aggro                             

                    //Aggro max, Target!
                    if (this.npc.target == null)
                    {
                        StartCoroutine(switchTargetCo(0, character));
                    }
                    else
                    {
                        if (this.npc.aggroList[this.npc.target][1] < this.npc.aggroList[character][1])
                        {
                            StartCoroutine(switchTargetCo(this.aggroStats.targetChangeDelay, character));
                        }
                    }
                }
                else if (this.npc.aggroList[character][0] <= 0)
                {
                    this.npc.aggroList[character][0] = 0; //aggro   

                    //Aggro lost, Target lost
                    if (this.npc.target == character) this.npc.target = null;
                    charactersToRemove.Add(character);
                    HideClue();
                }
            }

            foreach (Character character in charactersToRemove)
            {
                this.removeFromAggroList(character);
            }
        }


        private IEnumerator switchTargetCo(float delay, Character character)
        {
            yield return new WaitForSeconds(delay);
            this.npc.target = character;
            //AggroArrow temp = Instantiate(MasterManager.aggroArrow, this.npc.GetHeadPosition(), Quaternion.identity);
            //temp.SetTarget(this.npc.target);
            ShowClue(MasterManager.markAttack);
        }

        private void ShowClue(AggroClue clue)
        {
            if (this.activeClue == null) this.activeClue = Instantiate(clue, this.npc.GetHeadPosition(), Quaternion.identity, this.npc.transform);
        }

        private void HideClue()
        {
            if (this.activeClue != null) this.activeClue.Hide();
        }

        private void addToAggroList(Character character)
        {
            float aggroAmount = 0f;
            float factor = 0f;

            if (!this.npc.aggroList.ContainsKey(character))
            {
                this.npc.aggroList.Add(character, new float[] { aggroAmount, factor });
            }
        }

        private void removeFromAggroList(Character character)
        {
            if (this.npc.aggroList.ContainsKey(character)) this.npc.aggroList.Remove(character);
        }

        private void setParameterOfAggrolist(Character character, float amount)
        {
            if (this.npc.aggroList.ContainsKey(character))
            {
                this.npc.aggroList[character][1] = amount; //set factor of increase/decreasing aggro            
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.aggroStats.affections.IsAffected(this.npc, collision))
            {
                increaseAggro(this.npc, collision.GetComponent<Character>(), this.aggroStats.aggroIncreaseFactor);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            decreaseAggro(this.npc, collision.GetComponent<Character>(), this.aggroStats.aggroDecreaseFactor);
        }

        private void increaseAggroOnHit(Character character, Character newTarget, float damage)
        {
            if (newTarget != null && character == this.npc)
            {
                addToAggroList(newTarget);

                addAggro(newTarget, (this.aggroStats.aggroOnHitIncreaseFactor));
                if (this.npc.aggroList.Count == 1 && this.aggroStats.firstHitMaxAggro) addAggro(newTarget, (this.aggroStats.aggroNeededToTarget + (this.aggroStats.aggroDecreaseFactor * (-1))));

                if (this.npc.aggroList[newTarget][1] == 0) setParameterOfAggrolist(newTarget, this.aggroStats.aggroDecreaseFactor);
                if (this.npc.target == null) ShowClue(MasterManager.markTarget);
            }
        }


        private void increaseAggro(Character character, Character newTarget, float aggroIncrease)
        {
            if (newTarget != null && IsValidTarget(newTarget) && character == this.npc)
            {
                addToAggroList(newTarget);
                setParameterOfAggrolist(newTarget, aggroIncrease);
                if (this.npc.target == null) ShowClue(MasterManager.markTarget);
            }
        }

        private void decreaseAggro(Character character, Character newTarget, float aggroDecrease)
        {
            if (newTarget != null && character == this.npc) setParameterOfAggrolist(newTarget, aggroDecrease);
        }

        private void ClearAggro(Character character)
        {
            if (character == this.npc) this.npc.aggroList.Clear();
        }

        #endregion
    }
}
