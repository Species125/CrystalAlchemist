using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class PlayerTeleport : PlayerComponent
    {
        [SerializeField]
        private PlayerTeleportList teleportList;

        public override void Initialize()
        {
            base.Initialize();

            GameEvents.current.OnTeleport += SwitchScene;
            GameEvents.current.OnHasReturn += HasReturn;
            this.teleportList.Initialize();
            StartCoroutine(MaterializePlayer());
        }

        private void OnDestroy()
        {
            GameEvents.current.OnTeleport -= SwitchScene;
            GameEvents.current.OnHasReturn -= HasReturn;
        }

        public void SwitchScene()
        {
            if (!this.player.isLocalPlayer) 
            {
                //Simulate Despawn for Guest Players
                StartCoroutine(DematerializePlayer(false));
                return;
            }

            if (NetworkUtil.IsMaster()) //only master is allowed to send RPC "Scene Change" to all
            {
                this.player.photonView.RPC("RpcSwitchScene", RpcTarget.All);
            }
        }

        [PunRPC]
        public void RpcSwitchScene(PhotonMessageInfo info)
        {
            StartCoroutine(DematerializePlayer(true));
        }

        private void SetPosition(Vector2 position)
        {
            this.player.transform.position = position;
            this.player.ChangeDirection(this.player.values.direction);
        }

        private IEnumerator DematerializePlayer(bool changeScene)
        {
            this.player.SpawnOut(); //Disable Player        
            bool animation = this.teleportList.GetShowSpawnOut();

            if (this.player.respawnAnimation != null && animation) //Show Animation for DEspawn
            {
                this.player.SetDefaultDirection();
                RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, this.player.GetShootingPosition(), Quaternion.identity);
                respawnObject.Reverse(this.player);  //reverse
                //SpawnTeleportEffect(true);
                yield return new WaitForSeconds(respawnObject.getAnimationLength());
            }
            else
            {
                this.player.SetCharacterSprites(false);
                yield return new WaitForEndOfFrame();
            }
               
            //For Network Scene-change, only LocalPlayer and Master are allowed to change scene
            if (changeScene && NetworkUtil.IsMaster()) GameEvents.current.DoChangeScene(this.teleportList.GetNextTeleport().scene);
        }

        private IEnumerator MaterializePlayer()
        {
            this.player.SetCharacterSprites(false);
            this.player.SpawnOut(); //Disable Player

            if (this.teleportList.GetNextTeleport() == null)
            {
                this.player.SetCharacterSprites(true);
                this.player.SpawnIn();
                yield break;
            }

            Vector2 position = this.teleportList.GetNextTeleport().position;
            bool animation = this.teleportList.GetShowSpawnIn();

            SetPosition(position);

            if (this.player.respawnAnimation != null && animation)
            {
                this.player.SetDefaultDirection();
                yield return new WaitForSeconds(2f);
                RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, new Vector2(position.x, position.y + 0.5f), Quaternion.identity);
                respawnObject.Initialize(this.player);
                //SpawnTeleportEffect(false);
            }
            else
            {
                this.player.SetCharacterSprites(true);
                this.player.SpawnIn();
            }
        }

        public void SpawnTeleportEffect(bool reverse)
        {
            string prefabPath = this.player.respawnAnimation.path;
            int targetID = this.player.photonView.ViewID;

            this.player.photonView.RPC("RpcSpawnTeleportEffect", RpcTarget.Others, prefabPath, targetID, reverse);
        }

        [PunRPC]
        public void RpcSpawnTeleportEffect(string prefabPath, int targetID, bool reverse, PhotonMessageInfo info)
        {
            Player player = PhotonView.Find(targetID).GetComponent<Player>();
            RespawnAnimation animation = Resources.Load<RespawnAnimation>(prefabPath);
            RespawnAnimation temp = Instantiate(animation, player.GetShootingPosition(), Quaternion.identity);

            if (reverse) temp.Reverse(player);
            else temp.Initialize(player);
        }

        public bool HasReturn()
        {
            return this.teleportList.HasLast();
        }
    }
}
