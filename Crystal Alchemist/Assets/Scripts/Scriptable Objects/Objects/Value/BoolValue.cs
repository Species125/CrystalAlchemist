using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Values/BoolValue")]
    public class BoolValue : ScriptableObject
    {
        [SerializeField]
        private bool value;

        public bool GetValue()
        {
            return this.value;
        }

        public void SetValue(bool value)
        {
            this.value = value;
        }
    }
}
