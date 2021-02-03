using UnityEngine;

namespace CrystalAlchemist {
    public class SkillArenaSize : MonoBehaviour
    {
        [SerializeField]
        private FloatValue sizeValue;

        [SerializeField]
        private float factor = 1f;

        private void Awake()
        {
            this.transform.localScale = Vector2.one * sizeValue.GetValue()*this.factor;
        }
    }
}
