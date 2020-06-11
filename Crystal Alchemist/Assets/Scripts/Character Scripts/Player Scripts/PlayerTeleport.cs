﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private TeleportStats nextTeleport;

    [SerializeField]
    private TeleportStats lastTeleport;

    [SerializeField]
    private PlayerTeleportList teleportList;

    [SerializeField]
    private FloatValue fadeDuration;

    [SerializeField]
    private SimpleSignal fadeSignal;

    public void Initialize(Player player)
    {
        this.player = player;
        this.teleportList.Initialize();
        StartCoroutine(MaterializePlayer());
    }

    public void SwitchScene() => StartCoroutine(DematerializePlayer());    
    
    private void LoadScene()
    {
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(this.nextTeleport.scene);
        //asyncOperation.allowSceneActivation = true;
        StartCoroutine(loadSceneCo(this.nextTeleport.scene));
        //SceneLoader.Load(this.nextTeleport.scene);
    }

    private void SetPosition(Vector2 position)
    {
        this.player.transform.position = position;
        this.player.ChangeDirection(this.player.values.direction);
    }

    private IEnumerator DematerializePlayer()
    {
        this.player.SpawnOut(); //Disable Player        
        bool animation = this.nextTeleport.showAnimationOut;

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
        }

        LoadScene();
    }

    private IEnumerator MaterializePlayer()
    {
        this.player.SetCharacterSprites(false);
        this.player.SpawnOut(); //Disable Player
        
        Vector2 position = this.nextTeleport.position;
        bool animation = this.nextTeleport.showAnimationIn;

        SetPosition(position);

        if (this.player.respawnAnimation != null && animation)
        {
            this.player.SetDefaultDirection();
            yield return new WaitForSeconds(2f);
            RespawnAnimation respawnObject = Instantiate(this.player.respawnAnimation, new Vector2(position.x, position.y+0.5f), Quaternion.identity);
            respawnObject.Initialize(this.player);          
        }
        else
        {
            this.player.SetCharacterSprites(true);
            this.player.SpawnIn();            
        }                
    }

    private IEnumerator loadSceneCo(string targetScene)
    {
        this.fadeSignal.Raise();
        yield return new WaitForSeconds(this.fadeDuration.GetValue());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) asyncOperation.allowSceneActivation = true;            
            yield return null;
        }
    }
}
