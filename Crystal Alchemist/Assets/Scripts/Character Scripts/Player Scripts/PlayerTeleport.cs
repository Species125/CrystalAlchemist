using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

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

    public void NetworkInitialize()
    {
        base.Initialize();

        this.teleportList.Initialize();
        StartCoroutine(MaterializePlayer());
    }

    private void OnDestroy()
    {
        GameEvents.current.OnTeleport -= SwitchScene;
        GameEvents.current.OnHasReturn -= HasReturn;
    }

    [Button("Teleport Player")]
    public void SwitchScene() => StartCoroutine(DematerializePlayer());    
    
    private void LoadScene()
    {
        GameEvents.current.DoChangeScene(this.teleportList.GetNextTeleport().scene);
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
            //SpawnTeleportEffect(true);
            yield return new WaitForSeconds(respawnObject.getAnimationLength());
        }
        else
        {
            this.player.SetCharacterSprites(false);            
        }

        LoadScene();
    }

    private IEnumerator MaterializePlayer()
    {
        this.player.SetCharacterSprites(false);
        this.player.SpawnOut(); //Disable Player

        if(this.teleportList.GetNextTeleport() == null)
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
            RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, new Vector2(position.x, position.y+0.5f), Quaternion.identity);
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
    public void RpcSpawnTeleportEffect(string prefabPath, int targetID, bool reverse)
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
