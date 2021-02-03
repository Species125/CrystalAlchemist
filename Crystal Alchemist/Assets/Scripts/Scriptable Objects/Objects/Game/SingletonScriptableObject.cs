using System.Linq;
using UnityEngine;

namespace CrystalAlchemist
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return _instance;
            }
        }
    }
}
