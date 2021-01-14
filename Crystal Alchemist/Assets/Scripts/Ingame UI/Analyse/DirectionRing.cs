
using UnityEngine;

namespace CrystalAlchemist
{
    public class DirectionRing : MonoBehaviour
    {
        [SerializeField]
        private Character character;

        // Update is called once per frame
        void FixedUpdate()
        {
            float angle = (Mathf.Atan2(this.character.values.direction.y, this.character.values.direction.x) * Mathf.Rad2Deg) + 90;
            Vector3 rotation = new Vector3(0, 0, angle);

            this.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
