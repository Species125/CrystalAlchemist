using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    /*
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    public void UpdatePreview()
    {        
        List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();
        UnityUtil.GetChildObjects<CharacterCreatorPart>(this.transform, parts);

        foreach (CharacterCreatorPart part in parts)
        {
            if (part != null)
            {
                part.gameObject.SetActive(false); //set inactive

                Image image = part.GetComponent<Image>();
                CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(part.property.parentName);

                if (data != null || part.property.mandatory()) part.gameObject.SetActive(true);

                if (part.gameObject.activeInHierarchy && image != null) //set Image and Color when active
                {
                    if (data != null)
                    {
                        CharacterCreatorPartProperty property = this.mainMenu.GetProperty(data.name, data.parentName);
                        part.UpdatePreview(property);
                    }

                    List<Color> colors = this.mainMenu.creatorPreset.getColors(part.property.GetColorTable());
                    part.SetColors(colors);
                }
            }
        }
    }     */


}
