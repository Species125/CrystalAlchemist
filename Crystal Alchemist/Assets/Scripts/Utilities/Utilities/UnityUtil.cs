﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static void GetActiveChildObjects<T>(Transform transform, List<T> childObjects)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<T>() != null && child.gameObject.activeInHierarchy) childObjects.Add(child.GetComponent<T>());
            GetActiveChildObjects(child, childObjects);
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

    public static bool SceneExists(string scene)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == scene) return true;
        }
        return false;
    }

    public static Vector2 GetRandomVector(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        Vector2 center = bounds.center;
        bool found = false;
        int attempts = 0;

        do
        {
            float x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
            float y = Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
            Vector2 result = new Vector2(x, y);
            attempts++;

            if (collider.OverlapPoint(result)) return result;
        }
        while (!found && attempts < 50);

        return Vector2.zero;
    }

    public static List<Vector2> GetRandomVectors(Collider2D collider, int amount)
    {
        List<Vector2> result = new List<Vector2>();
        Bounds bounds = collider.bounds;
        Vector2 center = bounds.center;        

        for (int i = 0; i < amount; i++)
        {
            bool found = false;
            int attempts = 0;
            do
            {
                float x = Random.Range(center.x - bounds.extents.x, center.x + bounds.extents.x);
                float y = Random.Range(center.y - bounds.extents.y, center.y + bounds.extents.y);
                Vector2 temp = new Vector2(x, y);
                attempts++;

                if (collider.OverlapPoint(temp) && !result.Contains(temp))
                {
                    result.Add(temp);
                    break;
                }
            }
            while (!found && attempts < 50);
        }

        return result;
    }

    public static bool CheckDistances(Vector2 position, float maxDistance, List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && Vector2.Distance(position, list[i].transform.position) < maxDistance) return false;
        }
        return true;
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
