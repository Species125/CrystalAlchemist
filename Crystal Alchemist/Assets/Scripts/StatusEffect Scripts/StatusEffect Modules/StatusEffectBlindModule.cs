using UnityEngine;

namespace CrystalAlchemist
{
    public class StatusEffectBlindModule : MonoBehaviour, StatusEffectModule
    {
        public GameObject instantiatNewGameObject;
        private GameObject panel;

        public void DoAction() => this.panel = Instantiate(this.instantiatNewGameObject);

        public void DoDestroy() => Destroy(this.panel, 2f);
    }
}
