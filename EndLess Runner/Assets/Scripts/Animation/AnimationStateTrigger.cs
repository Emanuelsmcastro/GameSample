using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationTrigger
{
    public enum AnimationState
    {
        IDLE,
        RUN,
        JUMP,
        DEATH

    }
    public class AnimationStateTrigger : MonoBehaviour
    {
        public List<AnimationSetup> animationsSetups;

        public AnimationSetup GetAnimationTriggersSetupByEnumState(AnimationState stateType)
        {
            return animationsSetups.Find(setup => setup.stateType == stateType);
        }
    }

    [System.Serializable]
    public class AnimationSetup
    {
        public AnimationState stateType;
        public List<string> animationTriggers;
    }

}
