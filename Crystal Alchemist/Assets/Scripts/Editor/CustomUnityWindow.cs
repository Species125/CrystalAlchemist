using UnityEngine;
using UnityEditor;

namespace CrystalAlchemist
{
    public class CustomUnityWindow : EditorWindow
    {
        [MenuItem("Alchemist Menu/Testing/Player Animation Update")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CustomUnityWindow));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Update Player Animation"))
            {
                if (EditorUtility.DisplayDialog("Update Animation",
                    "Do you want to update all animations?\nThis could take a while... .", "Do it", "Nope"))
                    UpdateAnimations();
            }
        }

        private void UpdateAnimations()
        {
            PlayerSpriteSheet sheet =
                Resources.Load<PlayerSpriteSheet>("Scriptable Objects/Editor/Player Sprite Sheet");
            if (sheet != null) sheet.UpdateSpritesAndAnimations();
        }
    }
}