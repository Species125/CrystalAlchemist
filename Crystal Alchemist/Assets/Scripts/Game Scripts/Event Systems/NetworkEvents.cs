using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace CrystalAlchemist
{
    public class NetworkEvents : MonoBehaviourPun
    {
        public static NetworkEvents current;

        private void Awake() => current = this;

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEventBoss;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEventBoss;
        }
        

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.SKILL)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int senderID = (int)datas[1];
                int targetID = (int)datas[2];
                float reduce = (float)datas[3];
                Vector2 position = (Vector2)datas[4];
                Quaternion rotation = (Quaternion)datas[5];
                bool standalone = (bool)datas[6];

                InstantiateSkill(path, senderID, targetID, reduce, position, rotation, standalone);
            }
            else if (obj.Code == NetworkUtil.BOSSSKILL)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int senderID = (int)datas[1];
                int targetID = (int)datas[2];

                InstantiateSequence(path, senderID, targetID);
            }
            else if (obj.Code == NetworkUtil.ITEMDROP)
            {
                object[] datas = (object[])obj.CustomData;

                string path = (string)datas[0];
                Vector2 position = (Vector2)datas[1];
                bool bounce = (bool)datas[2];
                Vector2 direction = (Vector2)datas[3];

                ItemDrop drop = Resources.Load<ItemDrop>(path);
                DropItem(drop, position, bounce, direction);
            }
        }

        #region Skills

        public void InstantiateSkill(Ability ability, Character sender, Character target)
        {
            //Normal (Single)
            InstantiateAoESkill(ability, sender, target, 1);
        }

        public void InstantiateAoESkill(Ability ability, Character sender, Character target, float reduce)
        {
            //Reduced (AoE)
            RaiseAbilityEvent(ability, sender, target, reduce, sender.transform.position, Quaternion.identity, false);
        }

        private void RaiseAbilityEvent(Ability ability, Character sender, Character target, float reduce, Vector2 position, Quaternion rotation, bool standalone)
        {
            int characterID = NetworkUtil.GetID(sender);
            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target); 

            object[] datas = new object[] { ability.path, characterID, targetID, reduce, position, rotation, standalone };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.SKILL, datas, options, SendOptions.SendUnreliable);
        }

        public void InstantiateBossSequence(BossMechanic sequence, Character sender, Character target)
        {
            if (!NetworkUtil.IsMaster()) return;

            int characterID = NetworkUtil.GetID(sender);
            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target);

            object[] datas = new object[] { sequence.path, characterID, targetID };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.BOSSSKILL, datas, options, SendOptions.SendUnreliable);
        }

        public void InstantiateSkill(string path, int senderID, int targetID, float reduce, Vector2 position, Quaternion rotation, bool standalone)
        {
            Ability ability = Resources.Load<Ability>(path);
            Character sender = PhotonView.Find(senderID).GetComponent<Character>();

            Character target = null;
            if (targetID >= 0) target = PhotonView.Find(targetID).GetComponent<Character>();
            ability.SetSender(sender);

            AbilityUtil.InstantiateSkill(ability, sender, target, position, reduce, standalone, rotation);
        }

        private void InstantiateSequence(string path, int senderID, int targetID)
        {
            BossMechanic newSequence = Resources.Load<BossMechanic>(path);
            Character sender = PhotonView.Find(senderID).GetComponent<Character>();

            Character target = null;
            if (targetID >= 0) target = PhotonView.Find(targetID).GetComponent<Character>();

            AbilityUtil.InstantiateSequence(newSequence, sender, target);
        }

        #endregion


        #region Boss Mechanics

        //Events for spawning objects to all clients and set them to their parent

        private void NetworkingEventBoss(EventData obj)
        {
            if (obj.Code == NetworkUtil.MECHANIC_OBJECT)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int parentID = (int)datas[1];
                Vector2 position = (Vector2)datas[2];
                Quaternion rotation = (Quaternion)datas[3];
                bool overrideDuration = (bool)datas[4];
                float duration = (float)datas[5];

                InstaniateObject(path, parentID, position, rotation, overrideDuration, duration);
            }
            else if(obj.Code == NetworkUtil.MECHANIC_CHARACTER)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int parentID = (int)datas[1];
                Vector2 position = (Vector2)datas[2];
                Quaternion rotation = (Quaternion)datas[3];
                bool overrideDuration = (bool)datas[4];
                float duration = (float)datas[5];

                InstaniateCharacter(path, parentID, position, rotation, overrideDuration, duration);
            }
            else if (obj.Code == NetworkUtil.MECHANIC_AI)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int targetID = (int)datas[1];
                int parentID = (int)datas[2];
                Vector2 position = (Vector2)datas[3];
                Quaternion rotation = (Quaternion)datas[4];
                bool overrideDuration = (bool)datas[5];
                float duration = (float)datas[6];

                InstaniateAI(path, targetID, parentID, position, rotation, overrideDuration, duration);
            }
            else if (obj.Code == NetworkUtil.MECHANIC_ADDSPAWN)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int targetID = (int)datas[1];
                int parentID = (int)datas[2];
                Vector2 position = (Vector2)datas[3];
                Quaternion rotation = (Quaternion)datas[4];
                bool overrideDuration = (bool)datas[5];
                float duration = (float)datas[6];

                InstaniateAddSpawn(path, targetID, parentID, position, rotation, overrideDuration, duration);
            }
            else if (obj.Code == NetworkUtil.MECHANIC_SKILL)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int senderID = (int)datas[1];
                int targetID = (int)datas[2];
                int parentID = (int)datas[3];
                Vector2 position = (Vector2)datas[4];
                Quaternion rotation = (Quaternion)datas[5];
                bool overrideDuration = (bool)datas[6];
                float duration = (float)datas[7];
                bool addTarget = (bool)datas[8];

                InstantiateBossSkill(path, senderID, targetID, parentID, position, rotation, overrideDuration, duration, addTarget);
            }
            else if (obj.Code == NetworkUtil.MECHANIC_ABILITY)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];
                int senderID = (int)datas[1];
                int targetID = (int)datas[2];
                int parentID = (int)datas[3];
                Vector2 position = (Vector2)datas[4];
                Quaternion rotation = (Quaternion)datas[5];
                bool overrideDuration = (bool)datas[6];
                float duration = (float)datas[7];
                bool addTarget = (bool)datas[8];

                InstantiateBossAbility(path, senderID, targetID, parentID, position, rotation, overrideDuration, duration, addTarget);
            }
        }


        public void RaiseBossObjectSpawnEvent(NetworkBehaviour gameObject, GameObject parent,
                                              Vector2 position, Quaternion rotation,
                                              bool overrideDuration, float duration)
        {
            int parentID = NetworkUtil.GetID(parent);

            object[] datas = new object[] { gameObject.path, parentID, position, rotation, overrideDuration, duration };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_OBJECT, datas, options, SendOptions.SendUnreliable);
        }

        private void InstaniateObject(string path, int parentID,
                                      Vector2 position, Quaternion rotation,
                                      bool overrideDuration, float duration)
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);
            Instantiate(prefab, position, rotation, parent.transform);            
        }

        public void RaiseBossCharacterSpawnEvent(Character character, GameObject parent, 
                                                 Vector2 position, Quaternion rotation, 
                                                 bool overrideDuration, float duration)
        {
            int parentID = NetworkUtil.GetID(parent);

            object[] datas = new object[] { character.path, parentID, position, rotation, overrideDuration, duration };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_CHARACTER, datas, options, SendOptions.SendUnreliable);
        }

        private void InstaniateCharacter(string path, int parentID, 
                                         Vector2 position, Quaternion rotation, 
                                         bool overrideDuration, float duration)
        {
            Character prefab = Resources.Load<Character>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);
            Character character = Instantiate(prefab, position, rotation, parent.transform);

            character.InitializeAddSpawn(overrideDuration, duration);
        }

        public void RaiseBossAISpawnEvent(AI character, Character target, GameObject parent, 
                                          Vector2 position, Quaternion rotation, 
                                          bool overrideDuration, float duration)
        {
            int parentID = NetworkUtil.GetID(parent);

            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target);

            object[] datas = new object[] { character.path, targetID, parentID, position, rotation, overrideDuration, duration };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_AI, datas, options, SendOptions.SendUnreliable);
        }

        private void InstaniateAI(string path, int targetID, int parentID, 
                                  Vector2 position, Quaternion rotation, 
                                  bool overrideDuration, float duration)
        {
            AI prefab = Resources.Load<AI>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);
            AI character = Instantiate(prefab, position, rotation, parent.transform);

            character.InitializeAddSpawn(targetID, overrideDuration, duration);
        }

        public void RaiseBossAddSpawnEvent(AddSpawn addSpawn, Character target, GameObject parent, 
                                           Vector2 position, Quaternion rotation, 
                                           bool overrideDuration, float duration)
        {
            int parentID = NetworkUtil.GetID(parent);

            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target);

            object[] datas = new object[] { addSpawn.path, targetID, parentID, position, rotation, overrideDuration, duration };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_ADDSPAWN, datas, options, SendOptions.SendUnreliable);
        }

        private void InstaniateAddSpawn(string path, int targetID, int parentID, 
                                        Vector2 position, Quaternion rotation, 
                                        bool overrideDuration, float duration)
        {
            AddSpawn prefab = Resources.Load<AddSpawn>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);
            AddSpawn spawn = Instantiate(prefab, position, rotation, parent.transform);

            Character target = NetworkUtil.GetCharacter(targetID);
            spawn.Initialize(target);
        }

        public void RaiseBossSkillSpawnEvent(Skill skill, Character sender, Character target, 
                                             GameObject parent, Vector2 position, Quaternion rotation, 
                                             bool overrideDuration, float duration, bool addTarget)
        {
            int parentID = NetworkUtil.GetID(parent);

            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target);
            int senderID = NetworkUtil.GetID(sender);

            object[] datas = new object[] { skill.path, senderID, targetID, parentID, position, rotation, overrideDuration, duration, addTarget };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_SKILL, datas, options, SendOptions.SendUnreliable);
        }

        private void InstantiateBossSkill(string path, int senderID, int targetID, 
                                          int parentID, Vector2 position, Quaternion rotation, 
                                          bool overrideDuration, float duration, bool addTarget)
        {
            Skill prefab = Resources.Load<Skill>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);
            Skill skill = Instantiate(prefab, position, rotation, parent.transform);

            Character sender = NetworkUtil.GetCharacter(senderID);
            Character target = null;
            if (addTarget) target = NetworkUtil.GetCharacter(targetID);

            skill.InitializeStandAlone(sender, target, rotation);
            if (overrideDuration) skill.SetMaxDuration(true, duration);
        }

        public void RaiseBossAbilitySpawnEvent(Ability ability, Character sender, Character target,
                                             GameObject parent, Vector2 position, Quaternion rotation,
                                             bool overrideDuration, float duration, bool addTarget)
        {
            int parentID = NetworkUtil.GetID(parent);

            int targetID = -1;
            if (target != null) targetID = NetworkUtil.GetID(target);
            int senderID = NetworkUtil.GetID(sender);

            object[] datas = new object[] { ability.path, senderID, targetID, parentID, position, rotation, overrideDuration, duration, addTarget };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.MECHANIC_ABILITY, datas, options, SendOptions.SendUnreliable);
        }

        private void InstantiateBossAbility(string path, int senderID, int targetID, 
                                            int parentID, Vector2 position, Quaternion rotation, 
                                            bool overrideDuration, float duration, bool addTarget)
        {
            Ability ability = Resources.Load<Ability>(path);

            GameObject parent = NetworkUtil.GetGameObject(parentID);

            Character sender = NetworkUtil.GetCharacter(senderID);
            Character target = NetworkUtil.GetCharacter(targetID);

            Skill skill = AbilityUtil.InstantiateSpreadSkill(ability, sender, target, position, rotation);
            skill.transform.SetParent(parent.transform);
            if (overrideDuration) skill.SetMaxDuration(true, duration);
        }

        #endregion


        #region Items

        public void InstantiateItem(ItemDrop drop, Vector2 position)
        {
            InstantiateItem(drop, position, false);
        }

        public void InstantiateItem(ItemDrop drop, Vector2 position, bool bounce)
        {
            InstantiateItem(drop, position, bounce, Vector2.zero);
        }

        public void InstantiateTreasureItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 playerPosition)
        {
            Vector2 direction = position - playerPosition;
            InstantiateItem(drop, position, bounce, direction);
        }

        private void InstantiateItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 direction)
        {            
            object[] datas = new object[] { drop.path, position, bounce, direction };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.ITEMDROP, datas, options, SendOptions.SendUnreliable);
        }

        /*
        public void InstantiateItemMaster(object[] datas)
        {
            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.ITEMDROP_MASTER, datas, options, SendOptions.SendUnreliable);
        }*/

        private void DropItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 direction)
        {
            Collectable temp = Instantiate(drop.collectable, position, Quaternion.identity);
            temp.SetBounce(bounce, direction);
            temp.name = drop.name;
            temp.SetItem(drop);
            temp.SetSelfDestruction(drop.duration, drop.hasSelfDestruction);
        }

        #endregion
    }
}