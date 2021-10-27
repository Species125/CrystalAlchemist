using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/AI/AI Phase")]
    public class AIPhase : NetworkScriptableObject
    {
        [BoxGroup("Action Sequence")]
        public List<AIAction> actions;

        [BoxGroup("Action Sequence")]
        public bool loopActions;

        [BoxGroup("Events")]
        public List<AIEvent> events;
        
        public void Initialize()
        {
            foreach (AIEvent aiEvent in this.events) aiEvent.Initialize();
        }
    }
}
