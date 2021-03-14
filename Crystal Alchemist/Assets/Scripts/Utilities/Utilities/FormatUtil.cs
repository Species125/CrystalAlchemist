using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public static class FormatUtil
    {
        public static string GetLocalisedText(string ID, LocalisationFileType type, List<object> list)
        {
            string result = GetLocalisedText(ID, type);

            foreach (object obj in list)
            {
                if (obj == null) continue;

                if (obj.GetType() == typeof(TeleportStats))
                {
                    TeleportStats temp = (TeleportStats)obj;
                    result = result.Replace("<savepoint>", temp.GetTeleportName());
                }
                else if (obj.GetType() == typeof(Player))
                {
                    Player temp = (Player)obj;
                    result = result.Replace("<player>", temp.GetCharacterName());
                }
                else if (obj.GetType() == typeof(Treasure))
                {
                    string temp = GetLocalisedText("Treasure", LocalisationFileType.objects);
                    result = result.Replace("<interactable>", temp);
                }
                else if (obj.GetType() == typeof(Door))
                {
                    string temp = GetLocalisedText("Door", LocalisationFileType.objects);
                    result = result.Replace("<interactable>", temp);
                }
                else if (obj.GetType() == typeof(ItemStats))
                {
                    ItemStats temp = (ItemStats)obj;
                    result = result.Replace("<item name>", temp.getName());
                    result = result.Replace("<item amount>", temp.amount + "");
                    result = result.Replace("<item value>", temp.GetTotalAmount() + "");
                }
                else if (obj.GetType() == typeof(InventoryItem))
                {
                    InventoryItem temp = (InventoryItem)obj;
                    result = result.Replace("<item name>", temp.getName());
                    result = result.Replace("<item amount>", temp.GetAmount() + "");
                }
                else if (obj.GetType() == typeof(Costs))
                {
                    Costs temp = (Costs)obj;
                    result = result.Replace("<price>", temp.amount + "");

                    if (temp.resourceType == CostType.item && temp.item != null)
                    {
                        result = result.Replace("<price item name>", temp.item.getName());
                        result = result.Replace("<price item amount>", temp.item.GetAmount() + "");
                    }
                    else if (temp.resourceType == CostType.keyItem && temp.keyItem != null)
                    {
                        result = result.Replace("<price item name>", temp.keyItem.stats.getName());
                    }
                }
            }

            return result;
        }




        public static void SetButtonColor(Button button, Color newColor)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = newColor;
            button.colors = cb;
        }

        public static string GetLocalisedText(string key, LocalisationFileType type)
        {
            return LocalisationSystem.GetLocalisedValue(key, type);
        }

        public static string FormatFloatToString(float value, float schwelle)
        {
            if (Mathf.Abs(value) >= 10) return value.ToString("N0");
            else if (value % 1 == 0) return value.ToString("N0");

            return value.ToString("N1");
        }

        public static string ConvertToResourceValue(float value)
        {
            if (value > 0) return (value * 4).ToString("N1");
            return "0";
        }

        public static string ConvertToResourceValueMenu(float value)
        {
            if (value != 0) return (value * 4).ToString("N1");
            return "0";
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

        public static void set3DText(TMP_Text tmp, string text, bool bold, Color fontColor, Color outlineColor, float outlineWidth)
        {
            if (tmp == null) return;

            if (text != null) tmp.text = text + "";
            if (bold) tmp.fontStyle = FontStyles.Bold;
            if (outlineColor != null) tmp.outlineColor = outlineColor;
            if (fontColor != null) tmp.color = fontColor;
            if (outlineWidth > 0) tmp.outlineWidth = outlineWidth;
        }
    }
}
