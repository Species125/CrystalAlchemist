using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Vendor : BasicCharacter
    {
        [BoxGroup("Vendor")]
        [SerializeField]
        private CharacterType characterType = CharacterType.Friend;

        [BoxGroup("Vendor")]
        [SerializeField]
        [Required]
        private ShopData data;

        [BoxGroup("Vendor")]
        [SerializeField]
        private ShopWindowType type;

        [BoxGroup("Vendor")]
        [SerializeField]
        [Required]
        private ShopList shopList;

        public override CharacterType GetCharacterType()
        {
            return this.characterType;
        }

        public void ShowShop()
        {
            data.SetData(this.type, this.shopList);
            MenuEvents.current.OpenShop();
        }
    }
}
