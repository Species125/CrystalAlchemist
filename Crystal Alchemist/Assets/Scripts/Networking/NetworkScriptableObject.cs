using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class NetworkScriptableObject : ScriptableObject
    {
        [BoxGroup("Inspector")]
        [ReadOnly]
        public string path;

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.path = UnityUtil.GetResourcePath(this);
        }
#endif
    }
}