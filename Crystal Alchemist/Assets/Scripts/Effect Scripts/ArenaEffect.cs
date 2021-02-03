using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Collider2D))]
    public class ArenaEffect : MonoBehaviour
    {
        [SerializeField]
        [MinValue(1)]
        private int amount = 1;

        [SerializeField]
        private GameObject prefab;

        void Start()
        {
            List<Vector2> positions = UnityUtil.GetRandomVectors(this.GetComponent<Collider2D>(), amount);
            foreach (Vector2 position in positions)
            {
                Instantiate(this.prefab, position, Quaternion.identity);
            }
        }
    }
}
