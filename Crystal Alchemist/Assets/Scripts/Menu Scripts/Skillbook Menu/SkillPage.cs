


using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillPage : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;

        [SerializeField]
        private PlayerSkillset skillSet;

        [SerializeField]
        private SkillType category;

        [SerializeField]
        private int page = 1;

        public void Initialize()
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                SkillSlot slot = parent.transform.GetChild(i).GetComponent<SkillSlot>();
                int ID = slot.Initialize(this.page - 1);

                Ability ability = this.skillSet.getSkillByID(slot.GetComponent<SkillSlot>().ID, category);
                slot.SetSkill(ability);
            }

            if (page > 1) this.gameObject.SetActive(false);
        }
    }
}
