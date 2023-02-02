using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using Unity.VisualScripting;
using AnimationTrigger;

public class PlayerStateDead : StateBase
{
    private Player _player = Player.instance;
    private AnimationStateTrigger _animationStateTrigger;
    private List<string> _animationStateTriggers;
    private SFXPool _mySFXPool;

    public  PlayerStateDead()
    {
        _animationStateTrigger = _player.GetComponent<AnimationStateTrigger>();
        _animationStateTriggers = _animationStateTrigger.GetAnimationTriggersSetupByEnumState(AnimationTrigger.AnimationState.DEATH).animationTriggers;
        _mySFXPool = _player.mySFXPool;
    }

    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _player.moveVelocity.z = 0f;
        PlayAnimation(_animationStateTriggers[Random.Range(0, _animationStateTriggers.Count)]);
        _player.canMove = false;
        _mySFXPool.Play(_player.SFXDeath);
    }

    private void PlayAnimation(string trigger)
    {
        _player.animator.SetTrigger(trigger);
    }
}
