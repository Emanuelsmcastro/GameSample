using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using TMPro;
using AnimationTrigger;

public class PlayerStateIdle : StateBase
{
    private List<string> _animationTriggers;
    private AnimationStateTrigger _animationStateTrigger;
    private Player _player = Player.instance;

    public PlayerStateIdle()
    {
        _animationStateTrigger = _player.GetComponent<AnimationStateTrigger>();
        _animationTriggers = _animationStateTrigger.GetAnimationTriggersSetupByEnumState(AnimationTrigger.AnimationState.IDLE).animationTriggers;
    }

    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        Player.instance.moveVelocity.z = 0f;
        if(_animationTriggers != null)
        {
            PlayIdleAnimation(_animationTriggers[Random.Range(0, _animationTriggers.Count)]);
        }
    }

    private void PlayIdleAnimation(string trigger)
    {
        Player.instance.animator.SetTrigger(trigger);
    }
}
