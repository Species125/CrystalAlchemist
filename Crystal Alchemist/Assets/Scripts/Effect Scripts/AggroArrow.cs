using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AggroArrow : MonoBehaviour
    {
        private Character target;
        private Rigidbody2D rigidbody2D;

        [SerializeField]
        private float speed = 5f;

        private void Start()
        {
            this.rigidbody2D = this.GetComponent<Rigidbody2D>();
            //this.speed = Vector2.Distance(this.transform.position, this.target.GetShootingPosition()) / 2;
        }

        public void SetTarget(Character target)
        {
            this.target = target;
        }

        private void LateUpdate()
        {
            float distance = Vector2.Distance(this.transform.position, this.target.GetShootingPosition());
            if (distance > 0.5f)
            {
                Vector2 direction = (this.target.GetShootingPosition() - (Vector2)this.transform.position).normalized;
                this.rigidbody2D.velocity = (direction * speed);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
