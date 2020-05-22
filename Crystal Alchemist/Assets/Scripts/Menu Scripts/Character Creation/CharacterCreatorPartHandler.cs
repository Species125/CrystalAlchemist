﻿using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorPartHandler : MonoBehaviour
{
    [SerializeField]
    private CharacterPreset preset;

    [SerializeField]
    private SpriteRendererExtensionHandler handler;

    private List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();

    public void UpdateCharacterParts()
    {
        this.parts.Clear();
        UnityUtil.GetChildObjects<CharacterCreatorPart>(this.transform, this.parts);

        foreach (CharacterCreatorPart part in this.parts)
        {
            part.gameObject.SetActive(false);

            CharacterPartData data = this.preset.GetCharacterPartData(part.property.parentName, part.property.partName);
            if (data != null || part.property.neverDisable) part.gameObject.SetActive(true); 

            if (part.gameObject.activeInHierarchy)
            {
                List<Color> colors = this.preset.getColors(part.property.colorTables);
                part.SetColors(colors);
            }
        }

        this.handler.Initialize();
    }
}
