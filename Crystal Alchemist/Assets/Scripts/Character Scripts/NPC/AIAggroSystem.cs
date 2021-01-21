using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

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
        private float needed;

        [SerializeField]
        [BoxGroup("Debug")]
        private string mainTarget;

        [SerializeField]
        [BoxGroup("Debug")]
        private List<string> aggrodata = new List<string>();



        private int ID;

        #endregion


        #region Start und Update

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingAggroEvent;
        }

        private void Start()
        {
            this.ID = this.npc.photonView.ViewID;
            this.npc.aggroList.Clear();
            this.needed = this.aggroStats.aggroNeededToTarget / 100f;

            //if (!NetworkUtil.IsMaster()) return;

            GameEvents.current.OnAggroHit += increaseAggroOnHit;
            GameEvents.current.OnAggroIncrease += increaseAggro;
            GameEvents.current.OnAggroDecrease += decreaseAggro;
            GameEvents.current.OnAggroClear += ClearAggro;
        }

        private void OnDestroy()
        {
            //if (!NetworkUtil.IsMaster()) return;

            GameEvents.current.OnAggroHit -= increaseAggroOnHit;
            GameEvents.current.OnAggroIncrease -= increaseAggro;
            GameEvents.current.OnAggroDecrease -= decreaseAggro;
            GameEvents.current.OnAggroClear -= ClearAggro;
        }

        private void Update()
        {
            //if (!NetworkUtil.IsMaster()) return;

            if (this.GetComponent<CircleCollider2D>() == null) RotationUtil.rotateCollider(this.npc, this.gameObject);
            generateAggro();
        }

        #endregion


        #region Aggro-System    

        private void addAggro(int newTarget, float aggro)
        {
            float add = (float)(aggro / 100);

            if (newTarget > 0 && this.npc.aggroList.ContainsKey(newTarget))
            {
                this.npc.aggroList[newTarget][0] += add;

                if (this.npc.aggroList[newTarget][0] > this.needed) this.npc.aggroList[newTarget][0] = this.needed;
                else if (this.npc.aggroList[newTarget][0] < 0) this.npc.aggroList[newTarget][0] = 0;
            }

        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingAggroEvent;

            this.npc.targetID = 0;
            this.HideClue();
            this.npc.aggroList.Clear();
        }

        private bool IsValidTarget(int ID)
        {
            Character target = NetworkUtil.GetCharacter(ID);

            if (target == null
                || !target.gameObject.activeInHierarchy
                || target.values.isInvincible  //WHY?
                || target.values.currentState == CharacterState.dead
                || target.values.currentState == CharacterState.respawning) return false;

            return true;
        }

        private void generateAggro()
        {
            List<int> charactersToRemove = new List<int>();
            int currentTargetID = this.npc.targetID;

            Character c = NetworkUtil.GetCharacter(currentTargetID);
            if (c != null) this.mainTarget = c.name;
            else this.mainTarget = "none";

            if (this.npc.targetID > 0 && !IsValidTarget(currentTargetID))
            {
                this.npc.aggroList.Remove(currentTargetID);
                this.npc.targetID = 0;
            }

            this.aggrodata.Clear();

            foreach (KeyValuePair<int, float[]> entry in this.npc.aggroList)
            {
                int character = entry.Key;

                //                       amount                         factor
                addAggro(character, this.npc.aggroList[character][1] * (Time.deltaTime * this.npc.values.timeDistortion));

                this.aggrodata.Add(NetworkUtil.GetCharacter(entry.Key).name + " -> Amount: " + entry.Value[0] + " Factor: " + entry.Value[1]);
                

                //if (this.npc.aggroList[character][0] > 0) Debug.Log(this.npc.GetCharacterName() + " hat " + this.npc.aggroList[character][0] + " Aggro auf" + character);

                if (this.npc.aggroList[character][0] >= this.needed)
                {
                    //this.enemy.aggroList[character][0] = 1f; //aggro                             

                    //Aggro max, Target!
                    if (this.npc.targetID == 0)
                    {
                        StartCoroutine(switchTargetCo(0, character));
                    }
                    else
                    {
                        if (this.npc.aggroList[currentTargetID][1] < this.npc.aggroList[character][1])
                        {
                            StartCoroutine(switchTargetCo(this.aggroStats.targetChangeDelay, character));
                        }
                    }
                }
                else if (this.npc.aggroList[character][0] <= 0)
                {
                    this.npc.aggroList[character][0] = 0; //aggro   

                    //Aggro lost, Target lost
                    if (currentTargetID == character) this.npc.targetID = 0;
                    charactersToRemove.Add(character);
                    HideClue();
                }
            }

            foreach (int character in charactersToRemove)
            {
                this.removeFromAggroList(character);
            }
        }


        private IEnumerator switchTargetCo(float delay, int character)
        {
            yield return new WaitForSeconds(delay);
            this.npc.targetID = character;
            //AggroArrow temp = Instantiate(MasterManager.aggroArrow, this.npc.GetHeadPosition(), Quaternion.identity);
            //temp.SetTarget(this.npc.target);
            ShowClue(MasterManager.markAttack);
        }

        private void ShowClue(AggroClue clue)
        {
            if (clue == this.activeClue) return;

            HideClue();
            this.activeClue = Instantiate(clue, this.npc.GetHeadPosition(), Quaternion.identity, this.npc.transform);
        }

        private void HideClue()
        {
            if (this.activeClue != null) this.activeClue.Hide();
        }

        private void addToAggroList(int character)
        {
            float aggroAmount = 0f;
            float factor = 0f;

            if (!this.npc.aggroList.ContainsKey(character))
            {
                this.npc.aggroList.Add(character, new float[] { aggroAmount, factor });
            }
        }

        private void removeFromAggroList(int character)
        {
            if (this.npc.aggroList.ContainsKey(character)) this.npc.aggroList.Remove(character);
        }

        private void setParameterOfAggrolist(int character, float amount)
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

        private void ClearAggro(Character character)
        {
            if (character == this.npc) this.npc.aggroList.Clear();
        }

        #endregion


        #region Networking

        private void RaiseAggroEvent(int ID, Character newTarget, float damage, byte code)
        {
            int targetID = NetworkUtil.GetID(newTarget);

            object[] datas = new object[] { ID, targetID, damage };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(code, datas, options, SendOptions.SendUnreliable);
        }

        private void NetworkingAggroEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.AGGRO_ON_HIT)
            {
                object[] datas = (object[])obj.CustomData;
                int characterID = (int)datas[0];
                int targetID = (int)datas[1];
                float aggro = (float)datas[2];

                IncreaseAggroOnHit(characterID, targetID, aggro);
            }
            else if (obj.Code == NetworkUtil.AGGRO_INCREASE)
            {
                object[] datas = (object[])obj.CustomData;
                int characterID = (int)datas[0];
                int targetID = (int)datas[1];
                float aggro = (float)datas[2];

                IncreaseAggro(characterID, targetID, aggro);
            }
            else if (obj.Code == NetworkUtil.AGGRO_DECREASE)
            {
                object[] datas = (object[])obj.CustomData;
                int characterID = (int)datas[0];
                int targetID = (int)datas[1];
                float aggro = (float)datas[2];

                DecreaseAggro(characterID, targetID, aggro);
            }
        }

        private void increaseAggroOnHit(Character npc, Character newTarget, float damage)
        {
            if (!IsValidTarget(npc, newTarget)) return;
            RaiseAggroEvent(this.ID, newTarget, damage, NetworkUtil.AGGRO_ON_HIT);
        }

        private void IncreaseAggroOnHit(int characterID, int targetID, float damage)
        {
            Character character = NetworkUtil.GetCharacter(characterID);

            if (targetID > 0 && character == this.npc)
            {
                addToAggroList(targetID);

                addAggro(targetID, (this.aggroStats.aggroOnHitIncreaseFactor*damage));
                if (this.npc.aggroList.Count == 1 && this.aggroStats.firstHitMaxAggro) addAggro(targetID, (this.aggroStats.aggroNeededToTarget + (this.aggroStats.aggroDecreaseFactor * (-1))));

                if (this.npc.aggroList[targetID][1] == 0) setParameterOfAggrolist(targetID, this.aggroStats.aggroDecreaseFactor);
                if (this.npc.targetID == 0) ShowClue(MasterManager.markTarget);
            }
        }

        private void increaseAggro(Character npc, Character newTarget, float aggro)
        {
            if (!IsValidTarget(npc, newTarget)) return;
            RaiseAggroEvent(this.ID, newTarget, aggro, NetworkUtil.AGGRO_INCREASE);
        }


        private void IncreaseAggro(int characterID, int targetID, float aggroIncrease)
        {
            Character character = NetworkUtil.GetCharacter(characterID);

            if (targetID > 0 && IsValidTarget(targetID) && character == this.npc)
            {
                addToAggroList(targetID);
                setParameterOfAggrolist(targetID, aggroIncrease);
                if (this.npc.targetID == 0) ShowClue(MasterManager.markTarget);
            }
        }

        private void decreaseAggro(Character npc, Character newTarget, float aggro)
        {
            if (!IsValidTarget(npc, newTarget)) return;
            RaiseAggroEvent(this.ID, newTarget, aggro, NetworkUtil.AGGRO_DECREASE);
        }

        private void DecreaseAggro(int characterID, int targetID, float aggroDecrease)
        {
            Character character = NetworkUtil.GetCharacter(characterID);
            if (targetID > 0 && character == this.npc) setParameterOfAggrolist(targetID, aggroDecrease);
        }


        private bool IsValidTarget(Character npc, Character newTarget)
        {
            return (!IsGuestPlayer(newTarget) && npc == this.npc);
        }

        private bool IsGuestPlayer(Character character)
        {
            if (character.GetComponent<Player>() != null && !character.GetComponent<Player>().isLocalPlayer) return true;
            return false;
        }

        #endregion
    }
}
