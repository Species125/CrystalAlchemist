using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DontDestroyHandler : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> dontDestroy = new List<GameObject>();
        private List<GameObject> activeObjects = new List<GameObject>();

        public static DontDestroyHandler Instance;

        void Awake()
        {/*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            foreach (GameObject gameObject in this.dontDestroy)
            {
                GameObject temp = Instantiate(gameObject);
                temp.name = gameObject.name;
                DontDestroyOnLoad(temp);
                this.activeObjects.Add(temp);
            }
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }  */

            foreach (GameObject gameObject in this.dontDestroy)
            {
                GameObject temp = Instantiate(gameObject);
                temp.name = gameObject.name;
            }
        }

        public void DestroyAll()
        {
            /*
        //called from Signal
        foreach(GameObject temp in this.activeObjects)
        {
            Destroy(temp);
        }
        this.activeObjects.Clear();
        Instance = null;

        Destroy(this.gameObject);*/
        }
    }
}
