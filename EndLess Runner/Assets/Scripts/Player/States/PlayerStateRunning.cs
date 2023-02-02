using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using System.IO;
using AnimationTrigger;

public class PlayerStateRunning : StateBase
{
    private float _speed;
    private Transform _playerTransform;
    private Player _player = Player.instance;
    private AnimationStateTrigger _animationStateTrigger;
    private List<string> _animationRunningTriggers;
    public PlayerStateRunning(float speed, Transform playeTransform)
    {
        _speed = speed;
        _playerTransform = playeTransform;
        _animationStateTrigger = _player.GetComponent<AnimationStateTrigger>();
        _animationRunningTriggers = _animationStateTrigger.GetAnimationTriggersSetupByEnumState(AnimationTrigger.AnimationState.RUN).animationTriggers;
    }
    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _player.moveVelocity = _playerTransform.forward * _speed;
        if(_player.canMove)
            PlayAnimation(_animationRunningTriggers[Random.Range(0, _animationRunningTriggers.Count)]);
    }

    private void PlayAnimation(string trigger)
    {
        _player.animator.SetTrigger(trigger);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        if(_player.characterController.isGrounded)
             _player.SetOnceAnimationBool(true);
    }
}
