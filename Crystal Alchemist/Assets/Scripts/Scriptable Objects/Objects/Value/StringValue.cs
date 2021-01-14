using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Values/StringValue")]
    public class StringValue : ScriptableObject
    {
        [SerializeField]
        private string value;

        public string GetValue()
        {
            return this.value;
        }

        public void SetValue(string value)
        {
            this.value = value;
        }
    }
}
