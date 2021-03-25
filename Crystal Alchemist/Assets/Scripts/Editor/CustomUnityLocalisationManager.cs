using UnityEngine;
using System;
using UnityEditor;

namespace CrystalAlchemist
{
    public class CustomUnityLocalisationManager : EditorWindow
    {
        [MenuItem("Alchemist Menu/Tools/Localisation")]
        public static void ItemWizard()
        {
            CustomUnityLocalisationManager window =
                (CustomUnityLocalisationManager)GetWindow(typeof(CustomUnityLocalisationManager), true, "Localisation");
        }

        private void OnGUI()
        {
            
        }
    }
}
