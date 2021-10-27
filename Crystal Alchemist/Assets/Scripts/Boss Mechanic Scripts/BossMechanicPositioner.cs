using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BossMechanicPositioner : BossMechanicBase
    {
        [SerializeField]
        [HideLabel]
        [BoxGroup("Main")]
        [InfoBox("Zur Positionierung (inkl. Rotation) des GameObjects")]
        private SequenceProperty selfProperty;

        private void Start()
        {
            //this.selfProperty.AddSpawnPoints(this.transform);

            GameObject spawnpoint = GetSpawnPosition(this.selfProperty)[0];
            this.transform.position = spawnpoint.transform.position;
            this.transform.rotation = this.GetRotation(this.selfProperty.rotationType, this.selfProperty.rotationFactor, this.selfProperty.GetOffset());

            Destroy(spawnpoint, 0.3f);

            this.enabled = false;
        }
    }
}
