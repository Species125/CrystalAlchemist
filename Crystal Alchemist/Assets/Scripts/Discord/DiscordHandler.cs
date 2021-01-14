using Discord;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
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

        [SerializeField]
        private bool debug = false;

        private global::Discord.Discord discord;
        private ActivityManager activityManager;


        private void Start()
        {
            if (!Application.isEditor)
                Init();
        }

        public void Init()
        {
            try
            {
                discord = new global::Discord.Discord(long.Parse(applicationId), (long)CreateFlags.NoRequireDiscord);
                activityManager = discord.GetActivityManager();
                InvokeRepeating("UpdateActivity", 0.3f, 10f);
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
            }

            activityManager.UpdateActivity(activity, Callback);
        }

        private void Callback(Result result)
        {
            if (debug)
            {
                if (result != Result.Ok)
                    Debug.LogError("Error from discord (" + result.ToString() + ")");
                else
                    Debug.Log("Discord Result = " + result.ToString());
            }
        }

        private void Update()
        {
            if (discord == null) return;

            discord.RunCallbacks();
        }
    }
}
