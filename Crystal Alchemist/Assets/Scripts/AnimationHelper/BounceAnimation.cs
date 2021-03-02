using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BounceAnimation : MonoBehaviour
    {
        private float yValue;
        private float ySpeed;
        private float gravity;
        private int bounces;
        private bool alreadyBouncing = false;

        [Button]
        public void BounceItem()
        {
            this.alreadyBouncing = false;
            Bounce();
        }

        public void Bounce()
        {
            if (alreadyBouncing) return;

            this.yValue = 0;
            this.ySpeed = 0.1f;
            this.bounces = 0;
            this.gravity = 0.005f;
            StartCoroutine(bounceCo());
            alreadyBouncing = true;
        }

        private IEnumerator bounceCo()
        {
            while (this.bounces < 3)
            {
                if (this.yValue > 0) this.ySpeed -= this.gravity;
                this.yValue += this.ySpeed;

                if (this.yValue < 0)
                {
                    this.yValue = 0;
                    this.ySpeed *= -1;
                    this.gravity *= 2;
                    this.bounces++;
                }
                this.transform.localPosition = new Vector2(0, 1) * yValue;
                yield return new WaitForFixedUpdate();
            }
            this.transform.localPosition = Vector2.zero;
        }

        private void OnDisable()
        {
            this.alreadyBouncing = false;
        }
    }
}
