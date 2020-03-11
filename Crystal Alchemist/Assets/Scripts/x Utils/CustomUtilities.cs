﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;


#region Objects
[System.Serializable]
public class LootTable
{
    [VerticalGroup("Split")]
    public Item item;

    [HorizontalGroup("Split/Box", 50)]
    [ShowIf("item")]
    [Range(0, 100)]
    public int dropRate = 100;

    [ShowIf("item")]
    [HorizontalGroup("Split/Box", 50)]
    [Range(1, 99)]
    public int amount = 1;

    [VerticalGroup("Split2")]
    [Tooltip("Ersetzt den Schlüsselgegenstände wenn diese bereits vorhanden sind")]
    public Item alternativeLoot;

    [HorizontalGroup("Split2/Box", 50)]
    [ShowIf("alternativeLoot")]
    [Range(1, 99)]
    public int alternativeAmount;

    public LootTable(LootTable table)
    {
        this.dropRate = table.dropRate;
        this.amount = table.amount;
        this.alternativeAmount = table.alternativeAmount;
    }
}

[System.Serializable]
public class affectedResource
{
    public ResourceType resourceType;

    [ShowIf("resourceType", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [Range(-CustomUtilities.maxFloatInfinite, CustomUtilities.maxFloatInfinite)]
    public float amount;
}

public enum ResourceType
{
    none,
    life,
    mana,
    item,
    skill,
    statuseffect
}

#endregion

public class CustomUtilities : MonoBehaviour
{
    #region Konstanten
    public const float maxFloatInfinite = 9999;
    public const int maxIntInfinite = 9999;
    public const float minFloat = 0.1f;

    public const float maxFloatSmall = 100;
    public const int maxIntSmall = 100;

    public const float maxFloatPercent = 1000;
    public const float minFloatPercent = -100;
    #endregion


    public static class UnityFunctions
    {
        public static void initLoot(GameObject main, GameObject lootParentGameObject,
                                    List<LootTable> lootTable, List<LootTable> lootTableInternal,
                                    bool multiLoot, List<Item> inventory)
        {
            createLootParent(main, lootParentGameObject); //erstelle Parent wenn nicht vorhanden
            UpdateItemsInEditor(lootTable, lootTableInternal, lootParentGameObject); //instanziiere Items in den Parent als Vorlage
            Items.setItem(lootTableInternal, multiLoot, inventory, lootParentGameObject); //setze Items ins Inventar (LOOT)
        }

        public static void createLootParent(GameObject main, GameObject lootParentObject)
        {
            if (lootParentObject == null)
            {
                GameObject lootparent = new GameObject();
                lootparent.transform.SetParent(main.transform);
                lootParentObject = lootparent;
            }
        }

        public static void UpdateItemsInEditor(List<LootTable> lootTable, List<LootTable> lootTableInternal, GameObject lootParentObject)
        {
            if (lootTableInternal.Count == 0)
            {
                lootTableInternal.Clear();
                for (int i = 0; i < lootParentObject.transform.childCount; i++)
                {
                    Destroy(lootParentObject.transform.GetChild(i).gameObject);
                }

                if (lootTable.Count > 0 && lootParentObject != null)
                {
                    for (int i = 0; i < lootTable.Count; i++)
                    {
                        bool useAlternative = lootTable[i].item.checkIfAlreadyThere();

                        if (!useAlternative || lootTable[i].alternativeLoot != null)
                        {
                            LootTable table = new LootTable(lootTable[i]);

                            table.item = instantiateItem(lootTable[i].item, lootTable[i].alternativeLoot, lootParentObject, useAlternative);
                            table.item.amount = lootTable[i].amount;
                            if (lootTable[i].alternativeLoot != null && useAlternative) table.item.amount = lootTable[i].alternativeAmount;

                            lootTableInternal.Add(table);
                        }
                    }
                }
            }
        }

        public static void UpdateItemsInEditor(List<MiniGameMatch> matches, List<MiniGameMatch> internalMatches, GameObject lootParentObject)
        {
            internalMatches.Clear();
            for (int i = 0; i < lootParentObject.transform.childCount; i++)
            {
                Destroy(lootParentObject.transform.GetChild(i).gameObject);
            }

            if (matches.Count > 0 && lootParentObject != null)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    bool useAlternative = matches[i].loot.checkIfAlreadyThere();

                    if (!useAlternative || matches[i].alternativeLoot != null)
                    {
                        MiniGameMatch table = new MiniGameMatch(matches[i]);

                        table.loot = instantiateItem(matches[i].loot, matches[i].alternativeLoot, lootParentObject, useAlternative);
                        table.loot.amount = matches[i].amount;
                        if (matches[i].alternativeLoot != null && useAlternative) table.loot.amount = matches[i].alternativeAmount;

                        internalMatches.Add(table);
                    }
                }
            }
        }

        private static Item instantiateItem(Item item, Item alternative, GameObject lootParentObject, bool useAlternative)
        {
            Item temp = item;
            if (alternative != null && useAlternative) temp = alternative;

            GameObject itemGO = Instantiate(temp.gameObject, lootParentObject.transform.position, Quaternion.identity, lootParentObject.transform) as GameObject;
            itemGO.SetActive(false);
            itemGO.name = itemGO.name.Replace("(Clone)", "");
            Item result = itemGO.GetComponent<Item>();
            return result;
        }

        public static void GetChildObjects<T>(Transform transform, List<T> childObjects)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<T>() != null) childObjects.Add(child.GetComponent<T>());
                GetChildObjects(child, childObjects);
            }
        }
    }



    public static class Resources
    {
        public static float setResource(float resource, float max, float addResource)
        {
            if (addResource != 0)
            {
                if (resource + addResource > max) addResource = max - resource;
                else if (resource + addResource < 0) resource = 0;

                resource += addResource;
            }

            return resource;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Audio
    {
        private static Dictionary<AudioClip, float> soundsAlreadyPlayed = new Dictionary<AudioClip, float>();
        private static AudioSource blub;

        public static void playSoundEffect(AudioClip soundeffect)
        {
            playSoundEffect(null, soundeffect);
        }

        public static void playSoundEffect(AudioClip soundeffect, float volume)
        {
            playSoundEffect(null, soundeffect, volume);
        }

        public static void playSoundEffect(AudioClip music, AudioSource source, float volume)
        {
            source.volume = volume;
            source.clip = music;
            source.playOnAwake = false;
            source.loop = false;
            source.Play();
        }

        public static void playSoundEffect(GameObject gameObject, AudioClip soundeffect)
        {
            playSoundEffect(gameObject, soundeffect, GlobalValues.soundEffectVolume);
        }

        public static void playSoundEffect(GameObject gameObject, AudioClip soundeffect, float volume)
        {
            if (soundeffect != null)
            {
                GameObject parent = GameObject.FindWithTag("Audio");

                if (canPlaySound(soundeffect))
                {
                    GameObject temp = new GameObject(soundeffect.name);
                    if (parent != null) temp.transform.SetParent(parent.transform);

                    AudioSource source = temp.AddComponent<AudioSource>();
                    source.pitch = GlobalValues.soundEffectPitch;
                    source.volume = volume;
                    source.clip = soundeffect;

                    if (gameObject != null)
                    {
                        temp.transform.position = gameObject.transform.position;
                        source.maxDistance = 100f;
                        source.spatialBlend = 1f;
                        source.rolloffMode = AudioRolloffMode.Linear;
                        source.dopplerLevel = 0f;
                    }
                    source.Play();
                    Destroy(temp, soundeffect.length);
                }
            }
        }

        private static bool canPlaySound(AudioClip clip)
        {
            if (soundsAlreadyPlayed.ContainsKey(clip))
            {
                float lastTimePlayed = soundsAlreadyPlayed[clip];
                float maximum = .05f;
                if (lastTimePlayed + maximum < Time.time)
                {
                    soundsAlreadyPlayed[clip] = Time.time;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                soundsAlreadyPlayed.Add(clip, Time.time);
                return true;
            }
        }

        /*
        public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect, bool temp)
        {
            playSoundEffect(audioSource, soundeffect, GlobalValues.soundEffectVolume);
        }
        

        public static void playSoundEffect(AudioSource audioSource, AudioClip soundeffect, float volume)
        {
            if (soundeffect != null && audioSource != null)
            {
                audioSource.pitch = GlobalValues.soundEffectPitch;

                audioSource.PlayOneShot(soundeffect, volume);
            }
        }*/
    }

    ///////////////////////////////////////////////////////////////

    public static class Collisions
    {
        public static bool checkDistance(Character character, GameObject gameObject, float min, float max, float startDistance, float distanceNeeded, bool useStartDistance, bool useRange)
        {
            float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);

            //Debug.Log(startDistance + " : " + distanceNeeded + " : "+distance);

            if (useStartDistance && distanceNeeded > 0)
            {
                if (distance > (startDistance + distanceNeeded)) return true;
            }
            else if (!useStartDistance && distanceNeeded > 0)
            {
                if (distance > distanceNeeded) return true;
            }
            else if (useRange)
            {
                if (distance >= min && distance <= max) return true;
            }

            return false;
        }

        public static float checkDistanceReduce(Character character, GameObject gameObject, float deadDistance, float saveDistance)
        {
            float distance = Vector3.Distance(character.transform.position, gameObject.transform.position);
            float percentage = 100 - (100 / (saveDistance - deadDistance) * (distance - deadDistance));

            percentage = Mathf.Round(percentage / 25) * 25;

            if (percentage > 100) percentage = 100;
            else if (percentage < 0) percentage = 0;

            return percentage;
        }

        public static bool checkBehindObstacle(Character character, GameObject gameObject)
        {
            float offset = 0.1f;
            Vector2 targetPosition = new Vector2(character.transform.position.x - (character.direction.x * offset),
                                                     character.transform.position.y - (character.direction.y * offset));


            if (character.shadowRenderer != null)
            {
                targetPosition = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                             character.shadowRenderer.transform.position.y - (character.direction.y * offset));
            }

            Vector2 start = gameObject.transform.position;
            Vector2 direction = (targetPosition - start).normalized;

            RaycastHit2D hit = Physics2D.Raycast(start, direction, 100f);

            if (hit && !hit.collider.isTrigger)
            {
                if (hit.collider.gameObject != character.gameObject)
                {
                    //Debug.DrawLine(start, hit.transform.position, Color.green);
                    return true;
                }
                else
                {
                    //Debug.DrawLine(start, hit.transform.position, Color.red);
                    return false;
                }
            }


            return true;
        }

        public static bool checkAffections(Character character, bool affectOther, bool affectSame, bool affectNeutral, Collider2D hittedCharacter)
        {
            Character target = hittedCharacter.GetComponent<Character>();

            if (!hittedCharacter.isTrigger
                && target != null
                && target.currentState != CharacterState.dead
                && target.currentState != CharacterState.respawning
           && (
               (affectOther && (character.CompareTag("Player") || character.CompareTag("NPC")) && target.CompareTag("Enemy"))
            || (affectOther && (character.CompareTag("Enemy") && (target.CompareTag("Player") || target.CompareTag("NPC"))))
            || (affectSame && (character.CompareTag("Player") == target.CompareTag("Player") || character.CompareTag("Player") == target.CompareTag("NPC")))
            || (affectSame && (character.CompareTag("NPC") == target.CompareTag("NPC") || character.CompareTag("NPC") == target.CompareTag("Player")))
            || (affectSame && (character.CompareTag("Enemy") == target.CompareTag("Enemy")))
            || (affectNeutral && target.CompareTag("Object")))) return true;

            return false;
        }

        public static List<Character> getAffectedCharacters(Skill skill)
        {
            List<GameObject> result = new List<GameObject>();
            List<Character> targets = new List<Character>();
            SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

            if (targetModule != null)
            {
                if ((skill.sender.CompareTag("Player") || skill.sender.CompareTag("NPC")) && targetModule.affectOther)
                {
                    result.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
                }
                if (skill.sender.CompareTag("Enemy") && targetModule.affectOther)
                {
                    result.AddRange(GameObject.FindGameObjectsWithTag("Player"));
                    result.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
                }
                if (targetModule.affectSame)
                {
                    result.AddRange(GameObject.FindGameObjectsWithTag(skill.sender.tag));
                }
                if (targetModule.affectNeutral)
                {
                    result.AddRange(GameObject.FindGameObjectsWithTag("Other"));
                }
            }

            foreach(GameObject res in result)
            {
                if (res.GetComponent<Character>() != null) targets.Add(res.GetComponent<Character>());
            }

            return targets;
        }

        private static bool skillAffected(Collider2D hittedCharacter, Skill skill)
        {
            Skill tempSkill = Skills.getSkillByCollision(hittedCharacter.gameObject);

            if (tempSkill != null)
            {
                if (skill.GetComponent<SkillTargetModule>() != null
                && skill.GetComponent<SkillTargetModule>().affectSkills
                && tempSkill.CompareTag("Skill")
                && tempSkill.skillName != skill.skillName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
        {
            return checkCollision(hittedCharacter, skill, skill.sender);
        }

        public static bool checkCollision(Collider2D hittedCharacter, Skill skill, Character sender)
        {
            if (skill != null && skill.GetTriggerActive())
            {
                if (sender != null && hittedCharacter.gameObject == sender.gameObject)
                {
                    if (skill.GetComponent<SkillTargetModule>() != null
                        && skill.GetComponent<SkillTargetModule>().affectSelf) return true;
                    else return false;
                }
                else
                {
                    if (skillAffected(hittedCharacter, skill))
                    {
                        return true;
                    }
                    else if (!hittedCharacter.isTrigger)
                    {
                        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();
                        if (targetModule != null
                            && checkAffections(sender, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral, hittedCharacter))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject target, int range)
        {
            Vector2 direction = character.direction;
            Vector2 temp = (character.transform.position - target.transform.position).normalized;

            float direction_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float temp_angle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;

            float angle = Mathf.Abs(direction_angle - temp_angle);

            float min = 180 - range;
            float max = 180 + range;

            //Debug.Log(angle + ">>" + min + ":" + max);

            if (angle >= min && angle <= max) return true;
            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject)
        {
            return checkIfGameObjectIsViewed(character, gameObject, 1f);
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject, float distance)
        {
            if (character != null && character.shadowRenderer == null)
            {
                Debug.Log("Schatten-Objekt ist leer!");
                return false;
            }

            float width = 0.2f;
            float offset = 0.1f;

            Vector2 position = new Vector2(character.shadowRenderer.transform.position.x - (character.direction.x * offset),
                                           character.shadowRenderer.transform.position.y - (character.direction.y * offset));

            RaycastHit2D[] hit = Physics2D.CircleCastAll(position, width, character.direction, distance);

            foreach (RaycastHit2D hitted in hit)
            {
                if (hitted != false 
                    && !hitted.collider.isTrigger 
                    && hitted.collider.transform.parent != null
                    && hitted.collider.transform.parent.gameObject == gameObject) return true;
            }

            return false;
        }
    }





    ///////////////////////////////////////////////////////////////

    public static class UnityUtils
    {
        public static GameObject hasChildWithTag(Character character, string searchTag)
        {
            GameObject result = null;

            for (int i = 0; i < character.transform.childCount; i++)
            {
                if (character.transform.GetChild(i).tag == searchTag)
                {
                    result = character.transform.GetChild(i).gameObject;
                    return result;
                }
            }

            return result;
        }

        public static void SetAnimDirection(Vector2 direction, Animator animator)
        {
            if (animator != null)
            {
                SetAnimatorParameter(animator, "moveX", direction.x);
                SetAnimatorParameter(animator, "moveY", direction.y);
            }
        }

        public static void SetAnimatorParameter(List<Animator> animators, string parameter, bool value)
        {
            foreach (Animator animator in animators)
            {
                SetAnimatorParameter(animator, parameter, value);
            }
        }

        /// <summary>
        /// set bool value for Animator
        /// </summary>
        public static void SetAnimatorParameter(Animator animator, string parameter, bool value)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) animator.SetBool(parameter, value);
                }
            }
        }

        public static void SetAnimatorParameter(List<Animator> animators, string parameter)
        {
            foreach (Animator animator in animators)
            {
                SetAnimatorParameter(animator, parameter);
            }
        }

        public static void SetAnimatorParameter(Animator animator, string parameter)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) animator.SetTrigger(parameter);
                }
            }
        }

        public static void SetAnimatorParameter(List<Animator> animators, string parameter, float value)
        {
            foreach (Animator animator in animators)
            {
                SetAnimatorParameter(animator, parameter, value);
            }
        }

        public static void SetAnimatorParameter(Animator animator, string parameter, float value)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) animator.SetFloat(parameter, value);
                }
            }
        }

        public static void enableAnimator(List<Animator> animators, bool value)
        {
            foreach (Animator animator in animators)
            {
                animator.enabled = value;
            }
        }

        public static void SetAnimatorSpeed(List<Animator> animators, float value)
        {
            foreach (Animator animator in animators)
            {
                animator.speed = value;
            }
        }

        public static bool HasParameter(Animator animator, string parameter)
        {
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (param.name == parameter) return true;
                }
            }

            return false;
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Items
    {
        public static void setItem(List<LootTable> lootTable, bool multiLoot, List<Item> items, GameObject lootParentObject)
        {
            int rng = UnityEngine.Random.Range(1, CustomUtilities.maxIntSmall);
            int lowestDropRate = 101;

            if (lootTable.Count > 0)
            {
                foreach (LootTable loot in lootTable)
                {
                    if (rng <= loot.dropRate)
                    {
                        if (!multiLoot)
                        {
                            if (lowestDropRate > loot.dropRate)
                            {
                                lowestDropRate = loot.dropRate;

                                items.Clear();
                                items.Add(loot.item);
                            }
                        }
                        else
                        {
                            items.Add(loot.item);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < lootParentObject.transform.childCount; i++)
                {
                    Item item = lootParentObject.transform.GetChild(i).GetComponent<Item>();
                    if (item != null) items.Add(item);
                }
            }

            for (int i = 0; i < lootParentObject.transform.childCount; i++)
            {
                lootParentObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            //if (this.items.Count > 0) this.text = this.text.Replace("%XY%", this.items[0].GetComponent<Item>().amount + " " + this.items[0].GetComponent<Item>().name);
        }

        public static bool hasEnoughCurrency(ResourceType currency, Player player, Item item, int price)
        {
            bool result = false;

            if (currency == ResourceType.none) result = true;
            else if (currency != ResourceType.skill)
            {
                if (player.getResource(currency, item) - price >= 0) result = true;
                else result = false;
            }

            return result;
        }

        public static int getAmountFromInventory(Item item, List<Item> inventory)
        {
            int amount = 0;

            foreach (Item elem in inventory)
            {
                if (!item.isKeyItem || item.useItemGroup)
                {
                    if (item.itemGroup.ToUpper() == elem.itemGroup.ToUpper()) amount += elem.amount;
                }
                else
                {
                    if (item.gameObject.name.ToUpper() == elem.gameObject.name.ToUpper()) amount += elem.amount;
                }
            }
            return amount;
        }

        public static Item getItemFromInventory(Item item, List<Item> inventory)
        {
            foreach (Item elem in inventory)
            {
                if (item.isKeyItem)
                {
                    if (item.gameObject.name.ToUpper() == elem.gameObject.name.ToUpper()) return elem;
                }
                else
                {
                    if (item.itemGroup.ToUpper() == elem.itemGroup.ToUpper()) return elem;
                }
            }
            return null;
        }

        public static bool hasKeyItemAlready(Item item, List<Item> inventory)
        {
            if (item.isKeyItem)
            {
                foreach (Item elem in inventory)
                {
                    if (item.gameObject.name.ToUpper() == elem.gameObject.name.ToUpper()) return true;
                }
            }
            return false;
        }

        public static void updateInventory(Item item, Character character, int amount)
        {
            if (item != null)
            {
                Item found = getItemFromInventory(item, character.inventory);

                if (found == null)
                {
                    GameObject temp = Instantiate(item.gameObject, character.transform);
                    temp.name = item.name;
                    temp.SetActive(false);
                    temp.hideFlags = HideFlags.HideInHierarchy;
                    character.inventory.Add(temp.GetComponent<Item>());
                }
                else
                {
                    if (!item.isKeyItem)
                    {
                        found.amount += amount;

                        if (found.amount <= 0)
                        {
                            character.inventory.Remove(found);
                        }
                    }
                }
            }
        }

        public static bool canOpenAndUpdateResource(ResourceType currency, Item item, Player player, int price)
        {
            if (player != null
                && player.currentState != CharacterState.inDialog
                && player.currentState != CharacterState.respawning
                && player.currentState != CharacterState.inMenu)
            {
                if (hasEnoughCurrency(currency, player, item, price))
                {
                    reduceCurrency(currency, item, player, price);
                    return true;
                }
            }

            return false;
        }

        public static void reduceCurrency(ResourceType currency, Item item, Player player, int price)
        {
            if (player != null 
                && ((item != null && !item.isKeyItem) || item == null))
                player.updateResource(currency, item, -price);
        }

        public static Item getItemByID(List<Item> inventory, int ID, bool isKeyItem)
        {
            foreach (Item item in inventory)
            {
                if (item.isKeyItem == isKeyItem && item.itemSlot == ID) return item;
            }

            return null;
        }

        public static bool hasItemGroup(string itemGroup, List<Item> inventory)
        {
            foreach (Item elem in inventory)
            {
                if (itemGroup.ToUpper() == elem.itemGroup.ToUpper()) return true;
            }

            return false;
        }

        public static void setItemImage(Image image, Item item)
        {
            if (item.itemSpriteInventory != null) image.sprite = item.itemSpriteInventory;
            else image.sprite = item.itemSprite;
        }

        public static List<string> getMaps(Player player)
        {
            List<string> result = new List<string>();

            foreach (Item item in player.inventory)
            {
                if (item.isMap) result.Add(item.mapName);
            }

            return result;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Input
    {
        public static string getButton(enumButton button)
        {
            return button.ToString();
        }
    }





    public static class StatusEffectUtil
    {
        public static bool isCharacterStunned(Character character)
        {
            foreach (StatusEffect debuff in character.debuffs)
            {
                if (debuff.stunTarget) return true;
            }

            return false;
        }

        public static void RemoveAllStatusEffects(List<StatusEffect> statusEffects)
        {
            List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

            //Store in temp List to avoid Enumeration Exception
            foreach (StatusEffect effect in statusEffects)
            {
                dispellStatusEffects.Add(effect);
            }

            foreach (StatusEffect effect in dispellStatusEffects)
            {
                effect.DestroyIt();
            }

            dispellStatusEffects.Clear();
        }

        public static void RemoveStatusEffect(StatusEffect statusEffect, bool allTheSame, Character character)
        {
            List<StatusEffect> statusEffects = null;
            List<StatusEffect> dispellStatusEffects = new List<StatusEffect>();

            if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.debuffs;
            else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.buffs;

            //Store in temp List to avoid Enumeration Exception
            foreach (StatusEffect effect in statusEffects)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    dispellStatusEffects.Add(effect);
                    if (!allTheSame) break;
                }
            }

            foreach (StatusEffect effect in dispellStatusEffects)
            {
                effect.DestroyIt();
            }

            dispellStatusEffects.Clear();
        }

        public static void AddStatusEffect(StatusEffect statusEffect, Character character)
        {
            if (statusEffect != null && character.stats.characterType != CharacterType.Object)
            {
                bool isImmune = false;

                if (character.stats.isImmuneToAllDebuffs
                    && statusEffect.statusEffectType == StatusEffectType.debuff) isImmune = true;
                else
                {
                    for (int i = 0; i < character.stats.immunityToStatusEffects.Count; i++)
                    {
                        StatusEffect immunityEffect = character.stats.immunityToStatusEffects[i];
                        if (statusEffect.statusEffectName == immunityEffect.statusEffectName)
                        {
                            isImmune = true;
                            break;
                        }
                    }
                }

                if (!isImmune)
                {
                    List<StatusEffect> statusEffects = null;
                    List<StatusEffect> result = new List<StatusEffect>();

                    //add to list for better reference
                    if (statusEffect.statusEffectType == StatusEffectType.debuff) statusEffects = character.debuffs;
                    else if (statusEffect.statusEffectType == StatusEffectType.buff) statusEffects = character.buffs;

                    for (int i = 0; i < statusEffects.Count; i++)
                    {
                        if (statusEffects[i].statusEffectName == statusEffect.statusEffectName)
                        {
                            //Hole alle gleichnamigen Effekte aus der Liste
                            result.Add(statusEffects[i]);
                        }
                    }

                    //TODO, das geht noch besser
                    if (result.Count < statusEffect.maxStacks)
                    {
                        //Wenn der Effekte die maximale Anzahl Stacks nicht überschritten hat -> Hinzufügen
                        instantiateStatusEffect(statusEffect, character);
                    }
                    else
                    {
                        if (statusEffect.canOverride)
                        {
                            //Wenn der Effekt überschreiben kann, soll der Effekt mit der kürzesten Dauer entfernt werden
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();

                            instantiateStatusEffect(statusEffect, character);
                        }
                        else if (statusEffect.canDeactivateIt)
                        {
                            StatusEffect toDestroy = result[0];
                            toDestroy.DestroyIt();
                        }
                    }
                }
            }
        }

        private static void instantiateStatusEffect(StatusEffect statusEffect, Character character)
        {
            StatusEffect statusEffectClone = Instantiate(statusEffect, character.transform.position, Quaternion.identity, character.transform);
            statusEffectClone.setTarget(character);
        }

        public static List<StatusEffect> GetStatusEffect(StatusEffect statusEffect, Character character, bool getAll)
        {
            List<StatusEffect> result = new List<StatusEffect>();

            foreach (StatusEffect effect in character.buffs)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    result.Add(effect);
                    if (!getAll) break;
                }
            }

            foreach (StatusEffect effect in character.debuffs)
            {
                if (effect.statusEffectName == statusEffect.statusEffectName)
                {
                    result.Add(effect);
                    if (!getAll) break;
                }
            }

            return result;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Preset
    {
        public static void setPreset(CharacterPreset source, CharacterPreset target)
        {
            target.setRace(source.getRace());
            target.characterName = source.characterName;
            target.AddColorGroupRange(source.GetColorGroupRange());
            target.AddCharacterPartDataRange(source.GetCharacterPartDataRange());
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Format
    {
        public static void SetButtonColor(Button button, Color newColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = newColor;
            button.colors = cb;
        }

        public static string getLanguageDialogText(string originalText, string alternativeText)
        {
            if (GlobalValues.useAlternativeLanguage && alternativeText.Replace(" ", "").Length > 1) return alternativeText;
            else return originalText;
        }

        public static string formatFloatToString(float value, float schwelle)
        {
            if (Mathf.Abs(value) >= 10) return value.ToString("N0");
            else if (value % 1 == 0) return value.ToString("N0");

            return value.ToString("N1");
        }

        public static string setDurationToString(float value)
        {
            int rounded = Mathf.RoundToInt(value);

            if (rounded > 59) return (rounded / 60).ToString("0") + "m";
            else return rounded.ToString("0") + "s";
        }

        public static string formatString(float value, float maxValue)
        {
            string formatString = "";

            for (int i = 0; i < maxValue.ToString().Length; i++)
            {
                formatString += "0";
            }

            if (value == 0) return formatString;
            else return value.ToString(formatString);
        }

        public static void set3DText(TextMeshPro tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
        {
            if (tmp != null)
            {
                if (text != null) tmp.text = text + "";
                if (bold) tmp.fontStyle = FontStyles.Bold;
                if (outlineColor != null) tmp.outlineColor = outlineColor;
                if (fontColor != null) tmp.color = fontColor;
                if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
            }
        }

        public static void set3DText(TextMeshProUGUI tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
        {
            if (tmp != null)
            {
                if (text != null) tmp.text = text + "";
                if (bold) tmp.fontStyle = FontStyles.Bold;
                if (outlineColor != null) tmp.outlineColor = outlineColor;
                if (fontColor != null) tmp.color = fontColor;
                if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
            }
        }

        public static void getStartTime(float factor, out int hour, out int minute)
        {
            DateTime origin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now - origin;
            double difference = Math.Floor(diff.TotalSeconds);
            float minutes = (float)(difference / (double)factor); //elapsed ingame minutes
            float fhour = ((minutes / 60f) % 24f);
            float fminute = (minutes % 60f);

            if (fhour >= 24) fhour = 0;
            if (fminute >= 60) fminute = 0;

            hour = Mathf.RoundToInt(fhour);
            minute = Mathf.RoundToInt(fminute);
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class DialogBox
    {
        public static void showDialog(Interactable interactable, Player player)
        {
            showDialog(interactable, player, null);
        }

        public static void showDialog(Interactable interactable, Player player, DialogTextTrigger trigger)
        {
            showDialog(interactable, player, trigger, null);
        }

        public static void showDialog(Interactable interactable, Player player, Item loot)
        {
            if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.GetComponent<DialogSystem>().show(player, interactable, loot);
        }

        public static void showDialog(Interactable interactable, Player player, DialogTextTrigger trigger, Item loot)
        {
            if (interactable.gameObject.GetComponent<DialogSystem>() != null) interactable.gameObject.GetComponent<DialogSystem>().show(player, trigger, interactable, loot);
        }
    }


    ///////////////////////////////////////////////////////////////

    public static class Rotation
    {
        public static Quaternion getRotation(Vector2 direction)
        {
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Vector3 rotation = new Vector3(0, 0, angle);
            return Quaternion.Euler(rotation);
        }

        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        public static void rotateCollider(Character character, GameObject gameObject)
        {
            float angle = (Mathf.Atan2(character.direction.y, character.direction.x) * Mathf.Rad2Deg) + 90;

            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public static void setDirectionAndRotation(Skill skill, out float angle, out Vector2 start, out Vector2 direction, out Vector3 rotation)
        {
            float snapRotationInDegrees = 0;
            float rotationModifier = 0;
            float positionOffset = skill.positionOffset;
            float positionHeight = 0;

            SkillRotationModule rotationModule = skill.GetComponent<SkillRotationModule>();
            SkillPositionZModule positionModule = skill.GetComponent<SkillPositionZModule>();

            if (rotationModule != null)
            {
                snapRotationInDegrees = rotationModule.snapRotationInDegrees;
                rotationModifier = rotationModule.rotationModifier;
            }

            if (positionModule != null)
            {
                positionHeight = positionModule.positionHeight;
            }

            start = (Vector2)skill.sender.transform.position;

            if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null) direction = (Vector2)skill.sender.GetComponent<AI>().target.transform.position - start;
            else if (skill.target != null) direction = (Vector2)skill.target.transform.position - start;
            else direction = skill.sender.direction.normalized;

            float positionX = skill.sender.skillStartPosition.transform.position.x + (direction.x * positionOffset);
            float positionY = skill.sender.skillStartPosition.transform.position.y + (direction.y * positionOffset) + positionHeight;

            //if (useCustomPosition) positionY = skill.sender.shootingPosition.transform.position.y + (direction.y * positionOffset);

            start = new Vector2(positionX, positionY);
            if (skill.sender.GetComponent<AI>() != null && skill.sender.GetComponent<AI>().target != null) direction = (Vector2)skill.sender.GetComponent<AI>().target.transform.position - start;
            else if (skill.target != null) direction = (Vector2)skill.target.transform.position - start;


            float temp_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction = CustomUtilities.Rotation.DegreeToVector2(temp_angle);

            angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + rotationModifier;

            if (snapRotationInDegrees > 0)
            {
                angle = Mathf.Round(angle / snapRotationInDegrees) * snapRotationInDegrees;
                direction = CustomUtilities.Rotation.DegreeToVector2(angle);
            }

            rotation = new Vector3(0, 0, angle);
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Skills
    {
        public static Skill setSkill(Character character, Skill prefab)
        {
            /*
            Skill skillInstance = MonoBehaviour.Instantiate(prefab, character.skillSetParent.transform) as Skill;
            skillInstance.sender = character;
            skillInstance.gameObject.SetActive(false);

            return skillInstance;*/
            return prefab;
        }

        public static Skill instantiateSkill(Skill skill, Character sender)
        {
            return instantiateSkill(skill, sender, null, 1);
        }

        public static Skill instantiateSkill(Skill skill, Character sender, Character target)
        {
            return instantiateSkill(skill, sender, target, 1);
        }

        public static Skill instantiateSkill(Skill skill, Character sender, Character target, float reduce)
        {
            if (skill != null
                && sender.currentState != CharacterState.attack
                && sender.currentState != CharacterState.defend)
            {
                Skill activeSkill = Instantiate(skill, sender.transform.position, Quaternion.identity);
                SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
                SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();

                if (skill.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;

                if (target != null) activeSkill.target = target;
                activeSkill.sender = sender;

                List<affectedResource> temp = new List<affectedResource>();

                if (targetModule != null)
                {
                    for (int i = 0; i < targetModule.affectedResources.Count; i++)
                    {
                        affectedResource elem = targetModule.affectedResources[i];
                        elem.amount /= reduce;
                        temp.Add(elem);
                    }

                    targetModule.affectedResources = temp;
                    if (sendermodule != null) sendermodule.addResourceSender /= reduce;
                }

                sender.activeSkills.Add(activeSkill);
                return activeSkill.GetComponent<Skill>();
            }

            return null;
        }

        public static Skill getSkillByCollision(GameObject collision)
        {
            return collision.GetComponentInParent<Skill>();
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class Helper
    {
        public static void checkIfHelperDeactivate(PlayerAbilities playerAbilities)
        {
            /*
            if (!checkIfHelperActivated(playerAbilities.AButton)
                && !checkIfHelperActivated(playerAbilities.BButton)
                && !checkIfHelperActivated(playerAbilities.XButton)
                && !checkIfHelperActivated(playerAbilities.YButton)
                && !checkIfHelperActivated(playerAbilities.RBButton)
                && !checkIfHelperActivated(playerAbilities.LBButton)) playerAbilities.setTargetHelperActive(false);
            else playerAbilities.setTargetHelperActive(true);*/
        }

        private static bool checkIfHelperActivated(Ability ability)
        {
            if (ability != null
                && ability.skill.GetComponent<SkillAimingModule>() != null) return true;
            else return false;
        }
    }

    ///////////////////////////////////////////////////////////////

    public static class UI
    {
        public static void ShowMenu(GameObject newActiveMenu, List<GameObject> menues)
        {
            foreach (GameObject gameObject in menues)
            {
                gameObject.SetActive(false);
            }

            if (newActiveMenu != null && menues.Count > 0)
            {
                newActiveMenu.SetActive(true);

                for (int i = 0; i < newActiveMenu.transform.childCount; i++)
                {
                    ButtonExtension temp = newActiveMenu.transform.GetChild(i).GetComponent<ButtonExtension>();
                    if (temp != null && temp.setFirstSelected)
                    {
                        temp.setFirst();
                        break;
                    }
                }
            }
        }
    }
}
