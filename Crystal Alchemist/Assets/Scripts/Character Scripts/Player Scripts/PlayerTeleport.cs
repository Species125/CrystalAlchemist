using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace CrystalAlchemist
{
    public class PlayerTeleport : PlayerComponent
    {
        [SerializeField]
        private PlayerTeleportList teleportList;

        public override void Initialize()
        {
            base.Initialize();

            NetworkEvents.current.OnPlayerEntered += OnPlayerEnteredRoom;

            if (!this.player.isLocalPlayer) return;

            GameEvents.current.OnTeleport += SpawnOut;
            GameEvents.current.OnHasReturn += HasReturn;
            GameEvents.current.OnSetTeleportStat += SetNextTeleport;

            this.teleportList.Initialize();            
        }

        private void Start()
        {
            if (!this.player.isLocalPlayer) return;
            SpawnIn();
        }

        private void OnDestroy()
        {
            NetworkEvents.current.OnPlayerEntered -= OnPlayerEnteredRoom;

            if (!this.player.isLocalPlayer) return;

            GameEvents.current.OnTeleport -= SpawnOut;
            GameEvents.current.OnHasReturn -= HasReturn;
            GameEvents.current.OnSetTeleportStat -= SetNextTeleport;
        }

        private void SetNextTeleport(TeleportStats stats) => this.teleportList.SetNextTeleport(stats);

        private void SpawnIn()
        {
            StartCoroutine(MaterializePlayer());
            this.player.photonView.RPC("RpcSpawnIn", RpcTarget.Others);
        }

        private void SpawnOut()
        {
            StartCoroutine(DematerializePlayer());
            this.player.photonView.RPC("RpcSpawnOut", RpcTarget.Others);
        }                    

        private void SetPosition(Vector2 position)
        {
            this.player.transform.position = position;
            this.player.ChangeDirection(this.player.values.direction);            
        }

        private IEnumerator DematerializePlayer()
        {
            this.player.SpawnOut(); //Disable Player        
            bool animation = this.teleportList.GetShowSpawnOut();

            if (this.player.respawnAnimation != null && animation) //Show Animation for DEspawn
            {
                this.player.SetDefaultDirection();
                RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, this.player.GetShootingPosition(), Quaternion.identity);
                respawnObject.Reverse(this.player);  //reverse
                yield return new WaitForSeconds(respawnObject.getAnimationLength());
            }
            else
            {
                this.player.SetCharacterSprites(false);
                yield return new WaitForEndOfFrame();
            }
               
            //For Network Scene-change, only LocalPlayer and Master are allowed to change scene
            if (this.player.isLocalPlayer) GameEvents.current.DoChangeScene(this.teleportList.GetLatestTeleport().scene);
        }

        private IEnumerator MaterializePlayer()
        { 
            this.player.SetCharacterSprites(false);
            this.player.SpawnOut(); //Disable Player

            yield return new WaitForSeconds(0.3f); //Delay to get Teleport from Master

            TeleportStats stats = this.teleportList.GetLatestTeleport();

            if (stats == null)
            {
                this.player.SetCharacterSprites(true);
                this.player.SpawnIn();
                yield break;
            }
            
            Vector2 position = stats.position;
            SetPosition(position);

            bool animation = this.teleportList.GetShowSpawnIn();            

            if (this.player.respawnAnimation != null && animation)
            {
                this.player.characterCollider.enabled = true;
                this.player.SetDefaultDirection();
                yield return new WaitForSeconds(2f);
                RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, new Vector2(position.x, position.y + 0.5f), Quaternion.identity);
                respawnObject.Initialize(this.player);                
            }
            else
            {
                this.player.SetCharacterSprites(true);
                this.player.SpawnIn();
            }
        }        

        public bool HasReturn()
        {
            return this.teleportList.HasReturn();
        }

        private void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            TeleportStats stats = this.teleportList.GetLatestTeleport();

            if (NetworkUtil.IsMaster() && stats != null) this.player.photonView.RPC("RpcSetTeleportStat", newPlayer, stats.path);
        }

        [PunRPC]
        protected void RpcSetTeleportStat(string path)
        {
            Debug.Log("Set Teleport for " + this.gameObject.name + " to " + path);
            TeleportStats stats = Resources.Load<TeleportStats>(path);
            GameEvents.current.DoTeleportStat(stats);
        }

        [PunRPC]
        public void RpcSpawnIn(PhotonMessageInfo info)
        {
            StartCoroutine(MaterializePlayer());
        }

        [PunRPC]
        public void RpcSpawnOut(PhotonMessageInfo info)
        {
            StartCoroutine(DematerializePlayer());
        }
    }
}
