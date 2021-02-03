

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class GhostEffect : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private GameObject mainObject;

        [SerializeField]
        [Required]
        private GameObject ghost;

        [SerializeField]
        private float delay;

        [SerializeField]
        private float destroyAfter;

        private float ghostDelay;

        private void Update()
        {
            if (this.mainObject != null)
            {
                float distortion = 1;

                if (this.mainObject.GetComponent<Skill>() != null) distortion = this.mainObject.GetComponent<Skill>().getTimeDistortion();
                else if (this.mainObject.GetComponent<Character>() != null) distortion = this.mainObject.GetComponent<Character>().values.timeDistortion;

                if (ghostDelay > 0) this.ghostDelay -= (Time.deltaTime * distortion);
                else
                {
                    GameObject currentGhost = Instantiate(this.ghost, this.mainObject.transform.position, this.mainObject.transform.rotation);
                    this.ghostDelay = this.delay;

                    Destroy(currentGhost, this.destroyAfter);
                }
            }
        }
    }
}
