using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Global Game Objects")]
    public class MasterManager : SingletonScriptableObject<MasterManager>
    {

        public static AggroArrow aggroArrow { get { return Instance._aggroArrow; } }
        public static DamageNumbers damageNumber { get { return Instance._damageNumber; } }
        public static ContextClue contextClue { get { return Instance._contextClue; } }
        public static AggroClue markAttack { get { return Instance._markAttack; } }
        public static AggroClue markTarget { get { return Instance._markTargeting; } }
        public static MiniDialogBox miniDialogBox { get { return Instance._miniDialogBox; } }
        public static CastBar castBar { get { return Instance._castbar; } }
        public static AnalyseInfo analyseInfo { get { return Instance._analyseInfo; } }
        public static GameSettings settings { get { return Instance._gameSettings; } }
        public static DebugSettings debugSettings { get { return Instance._debugSettings; } }
        public static GlobalValues globalValues { get { return Instance._globalValues; } }
        public static List<InventoryItem> inventoryItems { get { return Instance._inventoryItems; } }
        public static List<Ability> abilities { get { return Instance._abilities; } }
        public static List<CharacterCreatorProperty> outfits { get { return Instance._outfits; } }
        public static List<TeleportStats> teleportpoints { get { return Instance._teleportPoints; } }
        public static TargetingSystem targetingSystem { get { return Instance._targetingSystem; } }
        public static TimeValue timeValue { get { return Instance._timeValue; } }
        public static StringValue actionButtonText { get { return Instance._actionButtonText; } }
        public static FloatValue menuDelay { get { return Instance._menuDelay; } }
        public static GameObject itemCollectGlitter { get { return Instance._itemCollectGlitter; } }
        public static GameObject itemDisappearSmoke { get { return Instance._itemDisappearSmoke; } }
        public static InputDeviceInfo inputDeviceInfo { get { return Instance._inputDeviceInfo; } }
        public static TeleportStats defaultTeleport { get { return Instance._defaultTeleport; } }

        [BoxGroup("Interaction")]
        [SerializeField]
        private ContextClue _contextClue;
        [BoxGroup("Interaction")]
        [SerializeField]
        private AnalyseInfo _analyseInfo;

        [BoxGroup("Combat")]
        [SerializeField]
        private DamageNumbers _damageNumber;
        [BoxGroup("Combat")]
        [SerializeField]
        private AggroArrow _aggroArrow;
        [BoxGroup("Combat")]
        [SerializeField]
        private CastBar _castbar;
        [BoxGroup("Combat")]
        [SerializeField]
        private TargetingSystem _targetingSystem;

        [BoxGroup("Values")]
        [SerializeField]
        private TimeValue _timeValue;
        [BoxGroup("Values")]
        [SerializeField]
        private InputDeviceInfo _inputDeviceInfo;
        [BoxGroup("Values")]
        [SerializeField]
        private StringValue _actionButtonText;
        [BoxGroup("Values")]
        [SerializeField]
        private FloatValue _menuDelay;

        [BoxGroup("Bubbles")]
        [SerializeField]
        private MiniDialogBox _miniDialogBox;
        [BoxGroup("Bubbles")]
        [SerializeField]
        private AggroClue _markAttack;
        [BoxGroup("Bubbles")]
        [SerializeField]
        private AggroClue _markTargeting;

        [BoxGroup("Item")]
        [SerializeField]
        private GameObject _itemCollectGlitter;
        [BoxGroup("Item")]
        [SerializeField]
        private GameObject _itemDisappearSmoke;

        [BoxGroup("Settings")]
        [SerializeField]
        private GameSettings _gameSettings;
        [BoxGroup("Settings")]
        [SerializeField]
        private GlobalValues _globalValues;
        [BoxGroup("Settings")]
        [SerializeField]
        private DebugSettings _debugSettings;

        [BoxGroup("Loading")]
        [SerializeField]
        private TeleportStats _defaultTeleport;
        [BoxGroup("Loading")]
        [SerializeField]
        private List<InventoryItem> _inventoryItems = new List<InventoryItem>();
        [BoxGroup("Loading")]
        [SerializeField]
        private List<Ability> _abilities = new List<Ability>();
        [BoxGroup("Loading")]
        [SerializeField]
        private List<CharacterCreatorProperty> _outfits = new List<CharacterCreatorProperty>();
        [BoxGroup("Loading")]
        [SerializeField]
        private List<TeleportStats> _teleportPoints = new List<TeleportStats>();

        [BoxGroup("Loading Config")]
        [SerializeField]
        private List<string> itemPaths = new List<string>();

        [Button]
        public void LoadAll()
        {
            this._inventoryItems.Clear();
            this._abilities.Clear();
            this._teleportPoints.Clear();

            foreach(string path in itemPaths)
            {
                this._inventoryItems.AddRange(Resources.LoadAll<InventoryItem>("Scriptable Objects/Items/Inventory Items/"+path+"/"));
            }            

            this._abilities.AddRange(Resources.LoadAll<Ability>("Scriptable Objects/Abilities/Skills/Player Skills/"));
            this._teleportPoints.AddRange(Resources.LoadAll<TeleportStats>("Scriptable Objects/TeleportPoints/"));
        }

        public static Ability GetAbility(string name)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.name == name) return ability;
            }

            Debug.LogError(name + " not found in Master!");
            return null;
        }

        public static CharacterCreatorProperty GetOutfit(string[] data)
        {
            string bodyPart = data[0];
            string name = data[1];
            BodyPart result;

            bool value = Enum.TryParse<BodyPart>(bodyPart, out result);
            if (!value) return null;

            foreach (CharacterCreatorProperty outfit in outfits)
            {
                if (result == outfit.type && outfit.name == name) return outfit;
            }

            Debug.LogError(name + " not found in Master!");
            return null;
        }

        public static InventoryItem GetItemGroup(string name)
        {
            foreach (InventoryItem group in inventoryItems)
            {
                if (group.name == name) return group;
            }

            Debug.LogError(name + " not found in Master!");
            return null;
        }

        public static TeleportStats GetTeleportStats(string[] data)
        {
            if (data == null || data.Length != 2) return null;

            foreach (TeleportStats teleport in teleportpoints)
            {
                if (teleport.Exists(data[0], data[1])) return teleport;
            }

            return null;
        }
    }
}
