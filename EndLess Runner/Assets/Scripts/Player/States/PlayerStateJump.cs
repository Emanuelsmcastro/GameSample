using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using AnimationTrigger;

public class PlayerStateJump : StateBase
{
    private float _jumpSpeed;
    private Player _player = Player.instance;
    private AnimationStateTrigger _animationStateTrigger;
    private List<string> _animationJumpTriggers;
    private SFXPool _mySFXPool;

    public PlayerStateJump(float jumpSpeed)
    {
        _jumpSpeed= jumpSpeed;
        _animationStateTrigger = _player.GetComponent<AnimationStateTrigger>();
        _animationJumpTriggers = _animationStateTrigger.GetAnimationTriggersSetupByEnumState(AnimationTrigger.AnimationState.JUMP).animationTriggers;
        _mySFXPool = _player.mySFXPool;
    }
    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _player.moveVelocity.y = _jumpSpeed;
        PlayAnimation(_animationJumpTriggers[Random.Range(0, _animationJumpTriggers.Count)]);
        _mySFXPool.Play(_player.SFXJump);
        
    }
        
    private void PlayAnimation(string trigger)
    {
        _player.animator.SetTrigger(trigger);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        if (!_player.canMove)
        {
            PlayAnimation("normal");
        }
    }
}
