using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameAnimationHelper : MonoBehaviour
    {
        [SerializeField]
        private MiniGameRound miniGameRound;

        public void Check()
        {
            this.miniGameRound.Check();
        }
    }
}
