﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    private GameObject activeMenu;
    private List<GameObject> activeMenuChildren = new List<GameObject>();

    public GameObject mainMenu;
    public GameObject optionMenu;
    public AudioClip music;

    public GameObject cursor = null;
    public Color cursorColor;
    public float cursorOffset = 35f;
    public AudioClip cursorSound;
    public FloatValue soundEffectVolume;
    public FloatValue musicVolume;

    private AudioSource audioSource;
    private AudioSource musicSource;

    private GameObject currentChoice = null;
    private int index = 0;
    private bool isInputPossible = true;
    private bool setVolume = false;
    private int tempChange = 0;

    // Start is called before the first frame update
    void Start()
    {
        changeLayer(this.mainMenu);

        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        if (this.music != null)
        {
            this.musicSource = this.transform.gameObject.AddComponent<AudioSource>();
            this.musicSource.clip = this.music;
            this.musicSource.volume = this.musicVolume.value;
            this.musicSource.loop = true;
            this.musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            string text = this.currentChoice.GetComponent<TextMeshProUGUI>().text;

            if (text.Contains("Optionen"))
            {
                this.currentChoice.GetComponent<TextMeshProUGUI>().color = this.cursorColor;
                changeLayer(this.optionMenu);
            }
            else if (text.Contains("starten"))
            {
                SceneManager.LoadScene(1);
            }
            else if (text.Contains("Zurück"))
            {
                this.currentChoice.GetComponent<TextMeshProUGUI>().color = this.cursorColor;
                changeLayer(this.mainMenu);
            }
            else if (text.Contains("beenden"))
            {
                Application.Quit();
            }
            else if (text.Contains("Lautstärke"))
            {
                if (!this.setVolume) this.setVolume = true;
                else this.setVolume = false;
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            string text = this.currentChoice.GetComponent<TextMeshProUGUI>().text;

            if (text.Contains("Lautstärke") && this.setVolume)
            {
                this.setVolume = false;
            }
            else if (this.activeMenu == this.optionMenu)
            {
                this.currentChoice.GetComponent<TextMeshProUGUI>().color = this.cursorColor;
                changeLayer(this.mainMenu);
            }
        }

        if (this.isInputPossible)
        {
            if (setVolume)
            {
                soundOptions(true);
            }
            else
            {
                soundOptions(false);

                
                int change = (int)(getInput("Vertical"));

                if (this.tempChange != change)
                {
                    this.tempChange = change;

                    if (this.index - change >= 0
                        && this.index - change < activeMenuChildren.Count
                        && change != 0)
                    {
                        this.index -= change;
                        setCursor();
                    }
                }
            }

            StartCoroutine(temp());
        }
    }

    private float getInput(string axis)
    {
        float changeAnalogStick = Mathf.RoundToInt(Input.GetAxisRaw(axis));
        float changeDPad = Input.GetAxisRaw("Cursor "+axis);
        if (changeAnalogStick != 0) return changeAnalogStick;
        else if (changeDPad != 0) return changeDPad;
        return 0;
    }

    private void soundOptions(bool marker)
    {
        TextMeshProUGUI ugui = this.currentChoice.GetComponent<TextMeshProUGUI>();

        float changeX = getInput("Horizontal");

        if (ugui.text.Contains("Effekt"))
        {
            if (marker) addVolume(this.soundEffectVolume, changeX);
            showVolume(ugui, this.soundEffectVolume, marker);
        }
        else if (ugui.text.Contains("Musik"))
        {
            if (marker) addVolume(this.musicVolume, changeX);
            showVolume(ugui, this.musicVolume, marker);
            this.musicSource.volume = this.musicVolume.value;
        }
    }

    private void showVolume(TextMeshProUGUI ugui, FloatValue volume, bool marker)
    {
        if (marker) ugui.text = ugui.text.Split(' ')[0] + " < " + Mathf.RoundToInt(volume.value * 100) + "% >";
        else ugui.text = ugui.text.Split(' ')[0] + " " + Mathf.RoundToInt(volume.value * 100) + "%";
    }

    private void addVolume(FloatValue volume, float addvolume)
    {
        if (addvolume != 0)
        {
            if (this.cursorSound != null) Utilities.playSoundEffect(this.audioSource, this.cursorSound, this.soundEffectVolume);
            volume.value += (addvolume / 100);
            if (volume.value < 0) volume.value = 0;
            else if (volume.value > 2f) volume.value = 2f;
        }
    }

    private void changeLayer(GameObject newActiveMenu)
    {
        if (this.activeMenu != null) this.activeMenu.SetActive(false);

        this.activeMenu = newActiveMenu;
        this.activeMenu.SetActive(true);
        getChildren(this.activeMenu);

        this.index = 0;
        this.currentChoice = null;

        setCursor();
    }

    private IEnumerator temp()
    {
        this.isInputPossible = false;
        yield return new WaitForSeconds(0.1f);
        this.isInputPossible = true;
    }

    private void getChildren(GameObject parent)
    {
        this.activeMenuChildren.Clear();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            this.activeMenuChildren.Add(child);

            //Bei Sound-Optionen
            TextMeshProUGUI ugui = child.GetComponent<TextMeshProUGUI>();
            if (ugui.text.Contains("Effekt")) showVolume(ugui, this.soundEffectVolume, false);
            else if (ugui.text.Contains("Musik")) showVolume(ugui, this.musicVolume, false);
        }
    }

    private void setCursor()
    {
        if (this.currentChoice != null)
        {
            this.currentChoice.GetComponent<TextMeshProUGUI>().color = this.cursorColor;
            if (this.cursorSound != null) Utilities.playSoundEffect(this.audioSource, this.cursorSound, this.soundEffectVolume);
        }

        this.currentChoice = this.activeMenuChildren[this.index];

        this.cursor.transform.position = new Vector3(this.cursor.transform.position.x, this.currentChoice.transform.position.y + this.cursorOffset);
        this.currentChoice.GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
