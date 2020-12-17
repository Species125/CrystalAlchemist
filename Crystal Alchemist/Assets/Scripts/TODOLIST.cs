using UnityEngine;

public class TODOLIST : MonoBehaviour
{
    //TODO (0.2.5.9)
    //REWORK: Soundeffects and Graphics
    //REWORK: Charakter Creation (Wings, Halo, etc) 

    //TODO: Multiloot (lootbox?)
    //TODO: Aggro weiter leiten (Enemy Alarm)
    //TODO: Item Manager
    //TODO: Rework Resource Type
    //TODO: Shopprice scriptalbe object

    //SKILL: Tornado
    //SKILL: Protect
    //REWORK: Windklinge

    //GRAPHICS: Sky
    //GRAPHICS: New Outfits (Sci Fi, Visor, Mundschutz, Devine Robe, Halo)
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

    //-----------------------------------------------------------------------------

    //TODO (0.2.6.0)

    //REWORK: Skills with Impact (Hitzeblitz, Eissplitter, Giftpfeil, etc)
    //REWORK: Map (no lighting, dynamic size, maybe plain texture instead)

    //TODO: Intro
    //TODO: Glasses (HUD)
    //TODO: Tutorial und Tutorial Log
    //TODO: Quests and Quest Log

    //CONTENT: Collectable Skills
    //CONTENT: Collectable Skins
    //CONTENT: Collectable Races (DNA)

    //UTIL: Item Assistent

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
}
