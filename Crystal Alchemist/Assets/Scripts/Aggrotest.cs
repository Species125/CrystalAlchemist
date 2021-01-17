using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace CrystalAlchemist
{
    public class Aggrotest : MonoBehaviour
    {
        public List<Character> characters = new List<Character>();
        public List<Vector2> positions = new List<Vector2>();
        public float speed = 1;

        public List<Vector2> start = new List<Vector2>();

        private void Start()
        {
            foreach(Character character in characters)
            {
                start.Add(character.transform.position);
            }
        }

        [Button]
        public void Go()
        {
            for(int i = 0; i < characters.Count; i++)
            {
                characters[i].myRigidbody.DOMove(positions[i], speed);
            }
        }

        [Button]
        public void Leave()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].myRigidbody.DOMove(start[i], speed);
            }
        }
    }
}