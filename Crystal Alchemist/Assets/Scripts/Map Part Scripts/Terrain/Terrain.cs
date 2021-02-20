using UnityEngine;

namespace CrystalAlchemist
{
    public class Terrain : MonoBehaviour
    {
        public bool IsCharacter(Collider2D collision)
        {
            return (!collision.isTrigger 
                && collision.GetComponent<Character>() != null 
                && !collision.GetComponent<Character>().IsGuestPlayer());
        }
    }
}
