using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Fountain : Interactable
    {
        [BoxGroup("Resource")]
        [SerializeField]
        private List<CharacterResource> resources = new List<CharacterResource>();

        public override void DoOnSubmit()
        {
            foreach (CharacterResource resource in this.resources) this.player.updateResource(resource.resourceType, resource.item, resource.amount, true);
        }
    }
}
