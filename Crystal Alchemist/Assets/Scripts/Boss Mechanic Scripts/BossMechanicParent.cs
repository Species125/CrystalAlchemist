using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BossMechanicParent : BossMechanicProperty
    {
        [SerializeField]
        [HideLabel]
        [BoxGroup("Main")]
        private SequenceProperty selfProperty;

        private void Start()
        {
            this.selfProperty.AddSpawnPoints(this.transform);

            GameObject spawnpoint = GetSpawnPosition(this.selfProperty);
            this.transform.position = spawnpoint.transform.position;

            this.transform.rotation = this.GetRotation(this.selfProperty.rotationType, this.selfProperty.rotationFactor, this.selfProperty.GetOffset());

            Destroy(spawnpoint, 0.3f);
            this.enabled = false;
        }

    }
}
