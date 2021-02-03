using UnityEngine;

namespace CrystalAlchemist
{
    public class ArenaSize : MonoBehaviour
    {
        [SerializeField]
        private FloatValue sizeValue;

        [SerializeField]
        private float size;

        private void Awake() => SetSize(this.size);

        public void SetSize(float value) => this.sizeValue.SetValue(value);
    }
}