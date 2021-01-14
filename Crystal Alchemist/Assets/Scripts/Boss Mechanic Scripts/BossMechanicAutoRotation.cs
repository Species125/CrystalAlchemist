using UnityEngine;

namespace CrystalAlchemist
{
    public class BossMechanicAutoRotation : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        void Update()
        {
            this.transform.Rotate(0, 0, Time.deltaTime * this.speed);
        }
    }
}
