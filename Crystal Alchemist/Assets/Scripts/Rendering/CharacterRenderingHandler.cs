using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class CharacterRenderingHandler : MonoBehaviour
{
    [SerializeField]
    [Required]
    private GameObject characterSprite;

    private List<CharacterRenderer> colorpalettes = new List<CharacterRenderer>();

#if UNITY_EDITOR
    [Button]
    public void setExtensions()
    {
        if(this.characterSprite != null)
        {
            List<SpriteRenderer> renderers = new List<SpriteRenderer>();
            renderers.Clear();

            UnityUtil.GetChildObjects<SpriteRenderer>(this.characterSprite.transform, renderers);

            foreach(SpriteRenderer renderer in renderers)
            {
                if (renderer.gameObject.GetComponent<CharacterRenderer>() == null)
                {
                    renderer.gameObject.AddComponent<CharacterRenderer>();
                    Debug.Log("Set Extension for " + renderer.name);
                }
            }
        }
    }
#endif

    public void Start()
    {
        this.colorpalettes.Clear();
        UnityUtil.GetActiveChildObjects<CharacterRenderer>(this.characterSprite.transform, this.colorpalettes);
    }

    public void Reset()
    {
        foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.Reset();        
    }

    public void RemoveTint(Color color)
    {
        foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.RemoveTint(color);        
    }

    public void Invert(bool value)
    {
        foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.InvertColors(value);
    }

    public void enableSpriteRenderer(bool value) => this.characterSprite.SetActive(value);    

    public void ChangeTint(Color color)
    {
        foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.ChangeTint(color);        
    }
}
