using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Values/IntValue")]
    public class IntValue : ScriptableObject
    {
        [SerializeField]
        private int value;

        public int GetValue()
        {
            return this.value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }
    }
}
