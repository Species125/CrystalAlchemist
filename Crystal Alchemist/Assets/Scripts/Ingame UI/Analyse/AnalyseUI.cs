


using UnityEngine;

namespace CrystalAlchemist
{
    public class AnalyseUI : MonoBehaviour
    {
        [SerializeField]
        private AnalyseEnemy enemyInfo;
        [SerializeField]
        private AnalyseObject objectInfo;

        public void setTarget(GameObject target)
        {
            this.enemyInfo.gameObject.SetActive(false);
            this.objectInfo.gameObject.SetActive(false);

            if (target.GetComponent<AI>() != null)
            {
                this.enemyInfo.gameObject.SetActive(true);
                this.enemyInfo.Initialize(target.GetComponent<AI>());
            }
            else if (target.GetComponent<Breakable>() != null)
            {
                this.objectInfo.gameObject.SetActive(true);
                this.objectInfo.Initialize(target.GetComponent<Breakable>());
            }
            else if (target.GetComponent<Treasure>() != null)
            {
                this.objectInfo.gameObject.SetActive(true);
                this.objectInfo.Initialize(target.GetComponent<Treasure>());
            }
        }
    }
}
