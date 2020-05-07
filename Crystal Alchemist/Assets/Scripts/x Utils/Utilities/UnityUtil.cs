﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnityUtil
{
    public static void GetChildObjects<T>(Transform transform, List<T> childObjects)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<T>() != null) childObjects.Add(child.GetComponent<T>());
            GetChildObjects(child, childObjects);
        }
    }

    public static void GetChildren(Transform transform, List<GameObject> childObjects)
    {
        foreach (Transform child in transform)
        {
            childObjects.Add(child.gameObject);
            GetChildren(child, childObjects);
        }
    }

    /// <summary>
    /// Change Movement to Pixel Perfect (16 PPU)
    /// </summary>
    /// <param name="moveVector">old position (raw)</param>
    /// <returns>new position pixel perfect</returns>
    public static Vector2 PixelPerfectClamp(Vector2 moveVector)
    {
        float pixelsPerUnit = 16;

        Vector2 vectorInPixels = new Vector2(Mathf.RoundToInt(moveVector.x * pixelsPerUnit),
            Mathf.RoundToInt(moveVector.y * pixelsPerUnit));

        Vector2 result = vectorInPixels / pixelsPerUnit;
        return result;
    }

    public static void SetInteractable(Selectable selectable, bool active)
    {
        selectable.interactable = active;
        if (active) SetColors(selectable, Color.white);
        else SetColors(selectable, MasterManager.staticValues.buttonNotActive);
    }

    public static void SetColors(Selectable selectable, Color disabledColor)
    {
        if (selectable != null)
        {
            ColorBlock colors = selectable.colors;
            colors.disabledColor = disabledColor;
            colors.highlightedColor = Color.white;
            colors.selectedColor = MasterManager.staticValues.buttonSelect;
            selectable.colors = colors;
        }
    }

    public static GameObject hasChildWithTag(Character character, string searchTag)
    {
        GameObject result = null;

        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == searchTag)
            {
                result = character.transform.GetChild(i).gameObject;
                return result;
            }
        }

        return result;
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    public static int ConvertLayerMask(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
