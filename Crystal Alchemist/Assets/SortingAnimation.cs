
using UnityEngine;
using UnityEngine.Rendering;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(SortingGroup))]
    public class SortingAnimation : MonoBehaviour
    {
        [SerializeField] private int order;

        [SerializeField] private float height;

        [SerializeField] private CharacterValues player;

        private SortingGroup group;

        void Awake() => this.group = this.GetComponent<SortingGroup>();

        private void Start()
        {
            this.transform.localPosition = new Vector3(0, 1, 0) * height;
            this.group.sortingOrder = this.order;
        }

        void LateUpdate()
        {
            if (this.player.currentState == CharacterState.respawning) return;
            this.transform.localPosition = new Vector3(0, 1, 0) * height;
            this.group.sortingOrder = this.order;
        }
    }
}