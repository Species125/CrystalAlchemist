namespace CrystalAlchemist
{
    public class SkillFollow : SkillExtension
    {
        private bool isEnabled = true;

        public override void Updating()
        {
            if (!isEnabled) return;
            this.skill.myRigidbody.position = this.skill.target.GetGroundPosition();
        }

        public void Enable(bool value) => this.isEnabled = value;
    }
}
