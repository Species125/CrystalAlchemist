using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterAttributeMenu : MenuBehaviour
    {
        [BoxGroup("Required")]
        [Required]
        public PlayerAttributes attributes;

        [BoxGroup]
        [SerializeField]
        private ItemGroup item;

        [BoxGroup]
        [SerializeField]
        private TextMeshProUGUI pointsField;

        [BoxGroup]
        [SerializeField]
        private AudioClip juwelInSound;

        [BoxGroup]
        [SerializeField]
        private AudioClip juwelOutSound;

        private int attributePoints;
        private int attributePointsMax;

        public override void Start()
        {
            base.Start();
            this.attributePoints = GameEvents.current.GetItemAmount(this.item);
            UpdatePoints();
        }

        public void playJuwelSound(bool insert)
        {
            if (insert) AudioUtil.playSoundEffect(this.gameObject, this.juwelInSound);
            else AudioUtil.playSoundEffect(this.gameObject, this.juwelOutSound);
        }

        public int GetPointsLeft()
        {
            return this.attributePoints - this.attributes.pointsSpent;
        }

        public void UpdatePoints()
        {
            this.attributePointsMax = this.item.maxAmount;
            int pointsLeft = GetPointsLeft();

            string text = FormatUtil.formatString(pointsLeft, this.attributePointsMax) + " / " + FormatUtil.formatString(this.attributePointsMax, this.attributePointsMax);
            this.pointsField.text = text;
        }
    }
}
