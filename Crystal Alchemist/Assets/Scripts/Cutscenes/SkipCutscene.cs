using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace CrystalAlchemist
{
    public class SkipCutscene : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector director;

        [Button]
        public void Skip(float time)
        {
            this.director.time = time;
        }
    }
}
