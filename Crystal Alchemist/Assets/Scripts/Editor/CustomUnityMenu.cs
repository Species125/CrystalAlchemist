using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class CustomUnityMenu : MonoBehaviour
{
    private static bool toggleGodMode = false;

    [MenuItem("Alchemist Menu/Menues/Map")]
    public static void OpenMap()
    {
        MenuEvents.current.OpenMap();
    }

    [MenuItem("Alchemist Menu/Cheats/MenuDialogBox -> YES!")]
    public static void OverwriteMenuDialogBox()
    {
        MenuDialogBox box = FindObjectOfType<MenuDialogBox>(true);
        box.Yes();
    }

    [MenuItem("Alchemist Menu/Cheats/Skip Phase within 2m")]
    public static void SkipPhase()
    {
        Player player = FindObjectOfType<Player>();
        AI[] enemies = FindObjectsOfType<AI>();
        foreach (AI enemy in enemies)
        {
            if (Vector2.Distance(enemy.GetGroundPosition(), player.GetGroundPosition()) < 2
                && enemy.GetComponent<AICombat>()) enemy.GetComponent<AICombat>().SkipPhase();
        }
    }

    [MenuItem("Alchemist Menu/Cheats/Kill Enemies within 2m")]
    public static void KillCloseEnemies()
    {
        Player player = FindObjectOfType<Player>();
        AI[] enemies = FindObjectsOfType<AI>();
        foreach (AI enemy in enemies)
        {
            if (Vector2.Distance(enemy.GetGroundPosition(), player.GetGroundPosition()) < 2) enemy.KillIt();
        }
    }

    [MenuItem("Alchemist Menu/Cheats/Kill all Enemies")]
    public static void KillAllEnemies()
    {
        AI[] enemies = FindObjectsOfType<AI>();
        foreach (AI enemy in enemies) enemy.KillIt();
    }

    [MenuItem("Alchemist Menu/Cheats/Toggle Slow Motion")]
    public static void ToggleSlowMo()
    {
        if (Time.timeScale == 1) Time.timeScale = 0.25f;
        else Time.timeScale = 1f;
    }

    /*
    [MenuItem("Alchemist Menu/Cheats/Toggle God Mode")]
    public static void GodMode()
    {
      //WHY?
        Player player = FindObjectOfType<Player>(true);

        if (!toggleGodMode) toggleGodMode = true;
        else toggleGodMode = false;

        player.GodMode(toggleGodMode);
    }
    */

    [MenuItem("Alchemist Menu/Misc/Set Cursor To Empty Buttons")]
    public static void SetCursor()
    {
        ButtonExtension[] buttons = FindObjectsOfType<ButtonExtension>(true);
        CustomCursor cursor = FindObjectOfType<CustomCursor>();
        foreach (ButtonExtension ext in buttons) if (ext.cursor == null) { ext.cursor = cursor; Debug.Log(ext.gameObject.name); }
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    /*
    private static void SetLocalisation()
    {
        List<StatusEffect> temp = new List<StatusEffect>();
        temp.AddRange(Resources.LoadAll<StatusEffect>("Scriptable Objects/StatusEffects"));

        foreach (StatusEffect effect in temp)
        {
            //UpdateLocalisation(effect.nameValue, effect.name + "_Name", effect.statusEffectName, effect.statusEffectNameEnglish, LocalisationFileType.statuseffects);
            //UpdateLocalisation(effect.nameValue, effect.name + "_Description", effect.statusEffectDescription, effect.statusEffectDescriptionEnglish, LocalisationFileType.statuseffects);

        }

        AssetDatabase.Refresh();
        Debug.Log("Done");
    }

    
    private static void UpdateLocalisation(LocalisationValue value, string key, string german, string english, LocalisationFileType type)
    {
        List<string> temp = LocalisationSystem.GetLines(type);
        List<string> result = new List<string>();

        string line = string.Format("\"{0}\",\"{1}\",\"{2}\",", key, german, english);
        value.key = key;
        value.type = type;

        if (!temp.Contains(line)) LocalisationSystem.AddLine(type, line);
    }*/
}
