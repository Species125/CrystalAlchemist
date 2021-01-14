
using UnityEngine;

namespace CrystalAlchemist
{
    public static class LineRenderUtil
    {
        public static void RenderLine(Character sender, Character target, Vector2 direction, float distance, SpriteRenderer spriteRenderer, Vector2 startpoint,
            out Collider2D hitted, out Vector2 hitPoint)
        {
            spriteRenderer.enabled = true;
            hitted = null;
            hitPoint = Vector2.zero;

            if (target != null)
            {
                //draw Laser to target
                hitPoint = target.GetShootingPosition();
                hitted = getCollider(target);
                drawLaserToTarget(startpoint, hitPoint, spriteRenderer);
            }
            else
            {
                //draw normal Laser            
                RaycastHit2D hitInfo = new RaycastHit2D();

                if (sender != null)
                {
                    int layerMask = 1 << sender.gameObject.layer;
                    layerMask = ~layerMask;
                    hitInfo = Physics2D.CircleCast(startpoint, spriteRenderer.size.y / 5, direction, distance, layerMask);
                }
                else
                {
                    hitInfo = Physics2D.CircleCast(startpoint, spriteRenderer.size.y / 5, direction, distance);
                }

                hitted = hitInfo.collider;
                hitPoint = hitInfo.point;

                if (hitInfo && !hitInfo.collider.isTrigger)
                {
                    drawLaserToTarget(startpoint, hitPoint, spriteRenderer);
                }
                else
                {
                    drawLongLaser(startpoint, direction, distance, spriteRenderer);
                }
            }
        }

        private static Collider2D getCollider(Character character)
        {
            //Übernehme Position, wenn ein Ziel vorhanden ist
            foreach (Collider2D collider in character.GetComponentsInChildren<Collider2D>(false))
            {
                if (!collider.isTrigger) return collider;
            }

            return null;
        }

        private static void drawLaserToTarget(Vector2 startpoint, Vector2 hitpoint, SpriteRenderer spriteRenderer)
        {
            float distance = Vector3.Distance(hitpoint, startpoint);
            Vector2 direction = (hitpoint - startpoint).normalized;
            Vector2 position = new Vector2(direction.x * (distance / 2), direction.y * (distance / 2)) + startpoint;

            setLaserSprite(position, distance, GetRotation(direction), spriteRenderer);
        }

        private static void drawLongLaser(Vector2 startpoint, Vector2 direction, float distance, SpriteRenderer spriteRenderer)
        {
            Vector2 position = new Vector2(direction.x * (distance / 2), direction.y * (distance / 2)) + startpoint;

            setLaserSprite(position, distance, GetRotation(direction), spriteRenderer);
        }

        private static void setLaserSprite(Vector2 position, float distance, Vector3 rotation, SpriteRenderer spriteRenderer)
        {
            spriteRenderer.transform.position = position;
            spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
            spriteRenderer.transform.rotation = Quaternion.Euler(rotation);
        }

        public static void Renderempty(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.size = new Vector2(0, 0);
        }

        private static Vector3 GetRotation(Vector2 direction)
        {
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            return new Vector3(0, 0, angle);
        }
    }
}
