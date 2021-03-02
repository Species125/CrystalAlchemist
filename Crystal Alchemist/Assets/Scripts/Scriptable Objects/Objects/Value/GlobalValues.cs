using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Game Static Values")]
    public class GlobalValues : ScriptableObject
    {
        [BoxGroup("Colors")]
        public Color[] red = new Color[] { new Color32(255, 0, 0, 255), new Color32(125, 0, 0, 255) };
        [BoxGroup("Colors")]
        public Color[] blue = new Color[] { new Color32(0, 125, 255, 255), new Color32(0, 25, 125, 255) };
        [BoxGroup("Colors")]
        public Color[] yellow = new Color[] { new Color32(0, 125, 255, 255), new Color32(0, 25, 125, 255) };
        [BoxGroup("Colors")]
        public Color[] green = new Color[] { new Color32(0, 255, 0, 255), new Color32(0, 125, 0, 255) };

        [Space(10)]
        [BoxGroup("Colors")]
        public Color buttonSelect = new Color32(255, 222, 0, 255);
        [BoxGroup("Colors")]
        public Color buttonNotActive = new Color32(200, 200, 200, 120);

        [Space(10)]
        [BoxGroup("Colors")]
        public Color common;
        [BoxGroup("Colors")]
        public Color uncommon;
        [BoxGroup("Colors")]
        public Color rare;
        [BoxGroup("Colors")]
        public Color epic;
        [BoxGroup("Colors")]
        public Color legendary;
        [BoxGroup("Colors")]
        public Color unique;

        [BoxGroup("Misc")]
        public Vector3 nullVector = new Vector3(0, 0, 999);
        [BoxGroup("Misc")]
        public string saveGameFiletype = "dat";

        [BoxGroup("Menues")]
        public CharacterState lastState;
        [BoxGroup("Menues")]
        public List<GameObject> openedMenues = new List<GameObject>();
    }
}
