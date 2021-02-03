using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(AI))]
    public class AIComponent : MonoBehaviour
    {
        [HideInInspector]
        public AI npc;

        public virtual void Initialize()
        {
            this.npc = this.GetComponent<AI>();
        }

        public virtual void Updating()
        {

        }
    }
}
