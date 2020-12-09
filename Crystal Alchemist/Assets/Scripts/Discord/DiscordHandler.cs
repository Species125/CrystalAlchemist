using Discord;
using UnityEngine;
using Sirenix.OdinInspector;

public class DiscordHandler : MonoBehaviour
{
    [SerializeField]
    private bool details = true;

    [ShowIf("details")]
    [SerializeField]
    private StringValue locationID;

    [ShowIf("details")]
    [SerializeField]
    private CharacterPreset preset;

    [SerializeField]
    private string applicationId = "786040495158853673";

    private Discord.Discord discord;
    private ActivityManager activityManager;


    private void Start()
    {
        if(!Application.isEditor) Init();
    }

    public void Init()
    {
        try
        {
            discord = new Discord.Discord(long.Parse(applicationId), (long)CreateFlags.NoRequireDiscord);
            activityManager = discord.GetActivityManager();
            UpdateActivity();
        }
        catch
        { }
    }

    private void UpdateActivity()
    {       
        Activity activity = new Activity
        {
            Assets =
                {
                    LargeImage = "icon",
                    LargeText = "Crystal Alchemist"
                }
        };

        /*
        if (details)
        {
            activity = new Activity
            {
                State = "Playing as " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(preset.getRace().ToString().ToLower()),
                Details = "On " + FormatUtil.GetLocalisedText(this.locationID.GetValue(), LocalisationFileType.maps),
                Assets =
                {
                    LargeImage = "icon",
                    LargeText = "Crystal Alchemist"
                }
            };
        }*/

        activityManager.UpdateActivity(activity, result =>
        {
            if (result != Result.Ok)
                Debug.LogError("Error from discord (" + result.ToString() + ")");
            else
                Debug.Log("Discord Result = " + result.ToString());
        });
    }

    private void Update()
    {
        if (discord == null) return;

        discord.RunCallbacks();
    }

    /*
    private void OnApplicationQuit()
    {
        if (discord == null) return;

        activityManager.ClearActivity((result) =>
            {
                if (result == Discord.Result.Ok) Debug.Log("Success!");
                else Debug.LogError("Failed");
            });
    }*/
}
