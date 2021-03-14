using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class AnalyseEnemy : MonoBehaviour
    {
        [BoxGroup("Text")]
        [SerializeField]
        private TMP_Text TMPcharacterName;

        [BoxGroup("UI")]
        [SerializeField]
        private Image ImageitemPreview;
        [BoxGroup("UI")]
        [SerializeField]
        private Image healthbar;

        [BoxGroup("Local")]
        [SerializeField]
        private SpriteRenderer ImageitemPreviewOLD;
        [BoxGroup("Local")]
        [SerializeField]
        private SpriteFillBar healthbarOLD;

        [BoxGroup]
        [SerializeField]
        private StatusEffectBar statusEffectBar;

        private AI npc;

        public void Initialize(AI enemy)
        {
            //set type of Analyse
            this.npc = enemy;
        }

        private void LateUpdate()
        {
            showEnemyInfo();
        }

        private void showEnemyInfo()
        {
            if (ImageitemPreview != null) this.ImageitemPreview.gameObject.SetActive(false);
            if (TMPcharacterName != null) this.TMPcharacterName.text = this.npc.GetCharacterName();
            if (healthbar != null) this.healthbar.fillAmount = this.npc.values.life / this.npc.values.maxLife;

            if (ImageitemPreviewOLD != null) this.ImageitemPreviewOLD.gameObject.SetActive(false);
            if (healthbarOLD != null) this.healthbarOLD.fillAmount(this.npc.values.life / this.npc.values.maxLife);

            this.statusEffectBar.setCharacter(this.npc.values);

            ItemDrop drop = GameUtil.GetHighestDrop(this.npc.values.itemDrops);

            if (drop != null
                && this.npc.values.currentState != CharacterState.dead
                && this.npc.values.currentState != CharacterState.respawning)
            {
                if (ImageitemPreview != null) this.ImageitemPreview.gameObject.SetActive(true);
                if (ImageitemPreview != null) this.ImageitemPreview.sprite = drop.stats.getSprite(); //TODONEW

                if (ImageitemPreviewOLD != null) this.ImageitemPreviewOLD.gameObject.SetActive(true);
                if (ImageitemPreviewOLD != null) this.ImageitemPreviewOLD.sprite = drop.stats.getSprite(); //TODONEW
            }
        }
    }
}
