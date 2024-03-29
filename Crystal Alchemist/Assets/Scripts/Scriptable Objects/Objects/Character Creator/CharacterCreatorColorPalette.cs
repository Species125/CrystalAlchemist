﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/CharacterCreation/Color Palette")]
    public class CharacterCreatorColorPalette : ScriptableObject
    {
        [BoxGroup("Color Options")]
        public bool canRemove = false;

        [BoxGroup("Color Options")]
        [HideIf("canRemove")]
        public Color defaultColor = Color.white;

        [BoxGroup("Color Options")]
        public List<Color> colors = new List<Color>();
    }
}
