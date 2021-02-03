


using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Jukebox : Interactable
    {
        [BoxGroup("JukeBox")]
        [SerializeField]
        private BackgroundMusic defaultMusic;

        public override void DoOnSubmit()
        {
            MenuEvents.current.OpenJukeBox();
        }
    }
}
