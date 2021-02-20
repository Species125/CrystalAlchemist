
using UnityEngine;

namespace CrystalAlchemist
{
    public class Warp : Terrain
    {
        [SerializeField]
        private Vector2 position;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsCharacter(collision)) return;

            AnimatorUtil.ShowSmoke(this.transform);
            collision.gameObject.transform.position = position;            
        }
    }
}
