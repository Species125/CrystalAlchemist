using UnityEngine;

namespace CrystalAlchemist
{
    public class TODOLIST : MonoBehaviour
    {
        //TODO (0.2.7)
        //TODO: Enemy Alarm mit Radius
        //TODO: Enemy Spawn on Death (Death Event?) -> Großer Blob zu kleinen Blobs
        //TODO: Autosave, Savemenu anpassen

        //ANIMATION: InMenu

        //MAJOR BUG: wrong enemy kill
        //MAJOR BUG: Key Items still visible  

        //TODO: Spawn Rework (//TODO: Spawn)
        //TODO: Entire AI on Master only
        //TODO: Kill RPC rework
        //TODO: Character Creator RPC
        //TODO: Gruppenliste
        //TODO: Statuseffekte Sync (on Scene change) and dispell
        //TDOO: Skill Sync (RPC on disable)

        //TODO: Online Join Window
        //TODO: Death Screen (only when all players are dead)

        //CHANGES: PLAYERABILITIES Indicator (removed this.player)


        /*     
        GRAPHICS: Booster
        GRAPHICS: Katzenohren und Katzenschwanz
        GRAPHICS: Schuppen (Beine und Körper)
        GRAPHICS: Beine ohne Animation (fürs Fliegen)
        */

        //SKILL: Tornado (Zieht Gegner ran und macht Schaden)
        //SKILL: Geode (Fläche, die Schaden über Zeit macht)
        //SKILL: Protect (Rundum-Schild)
        //REWORK: Windklinge, Feuerball, Eissplitter, Frostspitze, Steinwand

        ////////////////////NEW////////////////////////////
        //REWORK: Character Creation (VR Keyboard, Races, Wings)
        //ONLINE!!!
        //BUGFIXES, BETTER GRAPHICS, MOUSE-CONTROLS, AUTO-UI

        //-----------------------------------------------------------------------------

        //TODO (0.2.6.5)

        //GRAPHICS: Sky
        //GRAPHICS: New Outfits (Sci Fi, Visor, Mundschutz, Devine Robe)
        //GRAPHICS: Player Animations (Casting, Arrow, Spear, Victory)
        //GRAPHICS: New Nitro Animation
        //GRAPHICS: Smoke (Collectable, Interactable, Warp)

        //CONTENT: Jukebox (slow loading)
        //CONTENT: New Enemies (Bat, Bee, Spider, Libelle, Flower, Crystal, Zombie, Ghost, Skeleton, Butterfly, etc)
        //CONTENT: Sin Eater Race (glowing eyes, halo, wings, etc)
        //CONTENT: New Maps (Crystal Cave, Night Forest, Heaven)

        //CONTENT: BOSSFIGHT #2
        //GRAPHICS: Sin Eater
        //MAP: Divine Heaven
        //SKILLS: Soak Puddle
        //SKILLS: Dark Puddle
        //SKILLS: Tower
        //SKILLS: Stack Mark

        //REWORK: Soundeffects and Graphics
        //REWORK: Skills with Impact (Hitzeblitz, Eissplitter, Giftpfeil, etc)

        //TODO: Intro
        //TODO: Glasses (HUD)
        //TODO: Tutorial und Tutorial Log
        //TODO: Quests and Quest Log

        //CONTENT: Collectable Skills
        //CONTENT: Collectable Skins
        //CONTENT: Collectable Races (DNA)

        //TODO: Jahreszeiten

        //-----------------------------------------------------------------------------

        //TODO (1.0)
        //- Cutscenes
        //- Quests und Story
        //- Content!

        //-----------------------------------------------------------------------------

        //TODO (1.5)
        //- Multiplayer!
        //- Pathfinding!

        //-----------------------------------------------------------------------------

        //Wetter
        //- Regen (x)
        //- Schnee (x)
        //- Gewitter
        //- Nebel
        //- Wolken
        //- Grau
        //- Wüste
        //- Unterwasser

        //Visual Graph (Particles)
        //Jobs Pathfinding (Jobs!)
        //Multiplayer (Mirror?)
        //Struct vs Class (int? ref)
        //Documentation Update (How did I... , usefull links)

        /*
public void SetAsTreasureItem(Transform parent)
{
    this.showEffectOnDisable = false;
    this.showEffectOnEnable = false;
    this.transform.parent = parent;
    this.transform.position = parent.position;
    if (this.shadowRenderer != null) this.shadowRenderer.enabled = false;
    this.GetComponent<BoxCollider2D>().enabled = false;
    this.enabled = false;
}*/

        /*
[BoxGroup("Mandatory")]
[Required]
[SerializeField]
private GameObject showItem;
*/

        /*
public override void DoOnUpdate()
{

    if (!this.treasureEnabled
        && ((this.player != null && this.player.values.currentState == CharacterState.interact)
          || this.player == null))
    {
        closeChest(); //close Chest
    }
}
*/

        /*
if (this.treasureEnabled)
{
    if (this.player.canUseIt(this.costs)) openChest(); //open Chest
    else ShowDialog(DialogTextTrigger.failed);
}*/

        /*
    private void openChest()
    {

    if (this.itemDrop != null)
    {
        //Zeige Item
        this.showTreasureItem();
        ShowDialog(DialogTextTrigger.success, this.itemDrop.stats);
    }
    else
    {
        //Kein Item drin
        ShowDialog(DialogTextTrigger.empty);
    }
    }

    private void closeChest()
    {
        //Close when opened
        Destroy(this.itemDrop);        

        if (this.treasureType == TreasureType.lootbox)
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
            this.setLoot();
        }

        //Verstecke gezeigtes Item wieder
        for (int i = 0; i < this.showItem.transform.childCount; i++)
        {
            Destroy(this.showItem.transform.GetChild(i).gameObject);
        }

        this.showItem.SetActive(false);
    }*/



        /*
    public void SetEnabled(bool enable)
    {
        //Animator Events
        this.treasureEnabled = enable;

        if (PlayerCanInteract() && enable) ShowContextClue(true);
        else ShowContextClue(false);
    }

    public void showTreasureItem()
    {
        if(this.treasureMusic != null) MusicEvents.current.PlayMusicAndResume(this.treasureMusic, true, this.fadeOld, this.fadeNew);

        //Item instanziieren und der Liste zurück geben und das Item anzeigen            
        this.showItem.SetActive(true);

        Collectable collectable = this.itemDrop.Instantiate(this.showItem.transform.position);
        collectable.SetAsTreasureItem(this.showItem.transform);

        GameEvents.current.DoCollect(this.itemDrop.stats);
    }*/



        //public const byte RESPAWN_SHOW_EVENT = 1;
        //public const byte RESPAWN_HIDE_EVENT = 2;

        /*
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    
        //object[] datas = new object[] { gameObject, isInit };
        //RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        //PhotonNetwork.RaiseEvent(RESPAWN_SHOW_EVENT, datas, raiseEventOptions, SendOptions.SendReliable);
         
          
    public void OnEvent(EventData obj)
    {
        
        if(obj.Code == RESPAWN_SHOW_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            GameObject gameObject = (GameObject)datas[0];
            bool isInit = (bool)datas[1];

            _ShowGameObject(gameObject, isInit);
        }
        else if(obj.Code == RESPAWN_HIDE_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            GameObject gameObject = (GameObject)datas[0];
            bool isInit = (bool)datas[1];

            _HideGameObject(gameObject, isInit);
        }
    }
    */
    }
}
