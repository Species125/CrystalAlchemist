using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Values/ByteValue")]
    public class ByteValue : ScriptableObject
    {
        [SerializeField]
        private byte value;

        public byte GetValue()
        {
            return this.value;
        }

        public void SetValue(byte value)
        {
            this.value = value;
        }
    }
}
