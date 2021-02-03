

using UnityEngine;

namespace CrystalAlchemist
{
    public class HUDCutScene : MonoBehaviour
    {
        [SerializeField]
        private BoolValue CutSceneValue;

        private void Awake() => this.CutSceneValue.SetValue(false);

        private void Start()
        {
            GameEvents.current.OnCutScene += this.SetCutScene;
            SetCutScene();
        }

        private void OnDestroy() => GameEvents.current.OnCutScene += this.SetCutScene;

        private void SetCutScene() => this.gameObject.SetActive(!this.CutSceneValue.GetValue());
    }
}
