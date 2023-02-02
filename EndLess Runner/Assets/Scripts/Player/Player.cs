using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using Core.Singleton;
using NaughtyAttributes;
using DG.Tweening;
using Cinemachine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player> 
{
    public enum State
    {
        IDLE,
        RUNNING,
        JUMP,
        DEATH
    }
    [Header("Configs")]
    public CharacterController characterController;
    public StateMachine<State> stateMachine;
    public Transform checkPoint;
    public Animator animator;
    public CinemachineVirtualCamera virtualCamera;
    public Blink blink;
    public bool reviveAfterDeath;
    public bool destroyOnDeath;
    public float timeToDestroy = 1f;
    [SerializeField][ReadOnly] private bool _isDead = false;
    [Header("SFX")]
    public SFXPool mySFXPool;
    public SFXType SFXDeath;
    public SFXType SFXJump;

    [Header("Movements")]
    public bool canMove;
    public float speed = 3;
    public float jumpSpeed = 15f;
    [ReadOnly] public Vector3 moveVelocity;
    [ReadOnly] public float startZPosition;


    [Header("Gravity")]
    public float gravity = -20f;
    
    // privates //
    private bool _once;
    private bool _runningOnceAnimation = true;
    private GameManager _gameManager;

    private void OnValidate()
    {
        characterController = GetComponent<CharacterController>();
        blink = GetComponent<Blink>();
        mySFXPool= GetComponent<SFXPool>();
    }

    protected override void Awake()
    {
        base.Awake();
        blink.finishBlink += DestroyPlayer;
        startZPosition = transform.position.z;
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine<State>();
        stateMachine.Init();
        stateMachine.RegisterStates(State.IDLE, new PlayerStateIdle());
        stateMachine.RegisterStates(State.RUNNING, new PlayerStateRunning(speed, transform));
        stateMachine.RegisterStates(State.JUMP, new PlayerStateJump(jumpSpeed));
        stateMachine.RegisterStates(State.DEATH, new PlayerStateDead());
    }
    // Start is called before the first frame update
    void Start()
    {
        InitStateMachine();
        _gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Movements();
    }

    private void Movements()
    {
        if (!_isDead)
        {
            if (canMove)
            {
                _once = true;
                if (IsGrounded())
                {
                    if (_runningOnceAnimation)
                    {
                        _runningOnceAnimation = false;
                        stateMachine.SwitchState(State.RUNNING);
                    }

                    if (Input.GetButtonDown("Jump") | Input.touchCount > 0)
                    {
                        stateMachine.SwitchState(State.JUMP);
                    }
                }
            }
            else if (_once)
            {
                stateMachine.SwitchState(State.IDLE);
                _once = false;
            }
        }

        if (!IsGrounded())
        {
            moveVelocity.y += gravity * Time.deltaTime;
        }
        else if (!canMove)
        {
            moveVelocity.y = 0f;
        }

        characterController.Move(moveVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DeathZone") OnDeath();
    }


    [Button]
    public void OnDeath()
    {
        if (_isDead) return;
        blink.StartBlink();
        stateMachine.SwitchState(State.DEATH);
        _isDead = true;
        if(virtualCamera != null)
        {
            Invoke(nameof(DisablePlayerVirtualCamera), .3f);
        }        
        if(reviveAfterDeath)
        {
            Invoke(nameof(Revive), 1f);
        }
        _gameManager.stateMachine.SwitchState(GameManager.State.RESTART);
    }

    private void DestroyPlayer(Blink blink)
    {
        if (destroyOnDeath && !reviveAfterDeath)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }

    private void DisablePlayerVirtualCamera()
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
        }
    }

    private void EnableVirtualCamera()
    {
        if(virtualCamera != null)
        {
            virtualCamera.enabled = true;
        }
    }

    [Button]
    public void Revive()
    {
        if (!_isDead) return;
        transform.DOMove(checkPoint.position, 1f).SetEase(Ease.OutBack);
        _isDead = false;
        _runningOnceAnimation = true;
        canMove = true;
        transform.DOScale(0, 1f).SetEase(Ease.OutBack).From();
        Invoke(nameof(EnableVirtualCamera), .4f);
    }

    public float GetPlayerTravelledDistance() => transform.position.z - startZPosition;
    public float GetCurrentZPosition() => transform.position.z;
    public void SetOnceAnimationBool(bool value) => _runningOnceAnimation= value;
    public bool GetOnceRunningAnimaton() => _runningOnceAnimation;
    private bool IsGrounded() => characterController.isGrounded;
}
