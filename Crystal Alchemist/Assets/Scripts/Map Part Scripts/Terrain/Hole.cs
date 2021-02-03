using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Hole : MonoBehaviour
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
            Character character = collision.GetComponent<Character>();

            if (character != null) StartCoroutine(animationCo(character));
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
            }
            else
            {
                character.KillIt(false);
            }
        }
    }
}
