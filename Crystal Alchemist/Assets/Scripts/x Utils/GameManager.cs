﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string UI;

    [SerializeField]
    private string Menues;

    [SerializeField]
    private GameObject blackScreen;

    private void Awake()
    {
        SceneManager.LoadScene(UI, LoadSceneMode.Additive);
        SceneManager.LoadScene(Menues, LoadSceneMode.Additive);
        Destroy(this.blackScreen, 0.1f);
    }
}
