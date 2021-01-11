using UnityEngine;

public static class EditorUtil
{
    public static string GetResourcePath(Object obj)
    {
        return UnityEditor.AssetDatabase.GetAssetPath(obj).Replace("Assets/Resources/", "").Split('.')[0];
    }
}
