﻿using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Hole : Terrain
    {
        [HideLabel]
        [SerializeField]
        [BoxGroup("Costs")]
        private Costs cost;

        [BoxGroup("Hole")]
        [SerializeField]
        private Vector2 position;

        [BoxGroup("Hole")]
        [SerializeField]
        private float duration = 1.5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsCharacter(collision)) return;

            Character character = collision.GetComponent<Character>();            
            StartCoroutine(animationCo(character));
        }

        private IEnumerator animationCo(Character character)
        {
            character.transform.DOScale(0, this.duration);
            character.EnableScripts(false);

            yield return new WaitForSeconds(this.duration);

            if (character.GetComponent<Player>() != null)
            {                
                character.transform.position = this.position;
                character.transform.DOScale(1, 0f);
                character.EnableScripts(true);
                character.UpdateResource(cost.resourceType, -cost.amount);
                character.SetCannotHit(1f, false);
            }
            else
            {
                character.KillIt(false);
            }
        }
    }
}
