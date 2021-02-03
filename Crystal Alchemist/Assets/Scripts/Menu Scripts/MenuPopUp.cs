using UnityEngine;
using DG.Tweening;

namespace CrystalAlchemist
{
    public class MenuPopUp : MonoBehaviour
    {
        private Vector3 myScale;

        private void OnEnable()
        {
            this.myScale = this.transform.localScale;

            this.transform.DOScale(Vector3.zero,0);
            this.transform.DOScale(this.myScale, MasterManager.menuDelay.GetValue());
        }
    }
}
