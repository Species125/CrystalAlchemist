using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class BossMechanicAutoRotator : MonoBehaviour
    {
        [SerializeField]
        [InfoBox("Zur automatisierten Rotation von diesem GameObject")]
        private float speed;

        void Update()
        {
            this.transform.Rotate(0, 0, Time.deltaTime * this.speed);
        }
    }
}
