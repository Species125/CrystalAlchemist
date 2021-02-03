using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CrystalAlchemist
{
    public class CharacterTail : MonoBehaviour
    {
        public enum Mode
        {
            transform,
            rigidbody,
            tweenTransform,
            tweenRigidbody
        }

        public enum UpdateMethod
        {
            Update,
            FixedUpdate,
            LateUpdate
        }

        [SerializeField]
        private Mode mode;

        [SerializeField]
        private UpdateMethod updateMethod;

        [SerializeField]
        private SpriteRenderer main;

        [SerializeField]
        private List<GameObject> parts = new List<GameObject>();

        private List<float> distances = new List<float>();

        private void Start()
        {
            for (int i = 0; i < parts.Count - 1; i++) distances.Add(Vector2.Distance(parts[i + 1].transform.position, parts[i].transform.position));
            //for (int i = 0; i < parts.Count - 1; i++) SetPosition(parts[i + 1], parts[i], distances[i], -values.direction);
        }

        private void Update() => Updating(UpdateMethod.Update);

        private void FixedUpdate() => Updating(UpdateMethod.FixedUpdate);

        private void LateUpdate() => Updating(UpdateMethod.LateUpdate);

        private void Updating(UpdateMethod method)
        {
            if (updateMethod != method) return;
            for (int i = 0; i < parts.Count - 1; i++)
            {
                Vector2 direction = ((Vector2)parts[i].transform.position - (Vector2)parts[i + 1].transform.position).normalized;
                SetPosition(parts[i + 1], parts[i], distances[i], direction);
            }
        }

        private void SetPosition(GameObject part, GameObject parent, float dist, Vector2 direction)
        {
            Vector2 position = (Vector2)parent.transform.position - (direction * dist);
            float distance = Vector2.Distance(position, part.transform.position);
            if (distance > 0)
            {
                switch (mode)
                {
                    case Mode.transform: part.transform.position = position; break;
                    case Mode.rigidbody: part.GetComponent<Rigidbody2D>().MovePosition(position); break;
                    case Mode.tweenTransform: part.transform.DOMove(position, 0); break;
                    case Mode.tweenRigidbody: part.GetComponent<Rigidbody2D>().DOMove(position, 0); break;
                }
            }

            int modify = 0;

            if (parent.transform.position.y > part.transform.position.y) modify = 1;
            else modify = -1;

            if (parent != null && parent.GetComponent<SpriteRenderer>() != null)
                part.GetComponent<SpriteRenderer>().sortingOrder = parent.GetComponent<SpriteRenderer>().sortingOrder + modify;

            if (parent != null && parent.GetComponent<SortingGroup>() != null)
                parent.GetComponent<SortingGroup>().sortingOrder = this.main.sortingOrder + modify;
        }
    }
}
