﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class TitleScreen : BasicMenu
{        
    [SerializeField]
    private GameObject mainFrame;
    [SerializeField]
    private GameObject darkFrame;

    public override void Start()
    {
        this.mainFrame.SetActive(true);
        if (this.darkFrame != null) this.darkFrame.SetActive(false);

        Cursor.visible = true;
        base.Start();
    }

    private void LateUpdate()
    {
        if (!Cursor.visible) Cursor.visible = true;
    }
       
    public void exitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }    

    public override void OnDisable()
    {
        base.Start();
        this.mainFrame.SetActive(true);
    }

    public void showBackground(bool dark)
    {
        this.mainFrame.SetActive(false);
        if(this.darkFrame != null) this.darkFrame.SetActive(false);

        if (dark && this.darkFrame != null) this.darkFrame.SetActive(true);
        else if(!dark) this.mainFrame.SetActive(true);
    }
}
