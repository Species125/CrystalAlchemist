namespace CrystalAlchemist
{
    public class Partyfinder : Interactable
    {
        public override void DoOnSubmit()
        {
            this.player.UpdateResource(CostType.life, this.player.values.maxLife);
            this.player.UpdateResource(CostType.mana, this.player.values.maxMana);

            MenuEvents.current.OpenOnlineMenu();
        }
    }
}
