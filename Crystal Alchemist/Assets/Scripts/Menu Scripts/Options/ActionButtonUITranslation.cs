namespace CrystalAlchemist
{
    public class ActionButtonUITranslation : UITextTranslation
    {
        public override void OnEnable()
        {
            ChangeActionButton();
            base.OnEnable();
        }

        private void ChangeActionButton()
        {
            if (MasterManager.actionButtonText.GetValue() == string.Empty) this.Id = this.gameObject.name;
            else this.Id = MasterManager.actionButtonText.GetValue();
        }
    }
}
