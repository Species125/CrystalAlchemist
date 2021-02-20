using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class MultilevelLoader : MonoBehaviour
    {
        public string scene;
        
        void Start()
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }        
    }
}
