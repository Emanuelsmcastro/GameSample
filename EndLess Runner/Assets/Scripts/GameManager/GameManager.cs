using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core.Singleton;
using Items;
using Cinemachine;
using DG.Tweening;
using Core.StateMachine;

public class GameManager : Singleton<GameManager>
{
    public enum State
    {
        MENU,
        START,
        RESTART,
        QUIT
    }

    [Header("General")]
    public LoadSceneHelper loadSceneHelper;
    public StateMachine<State> stateMachine;
    [Header("Player")]
    [SerializeField][ReadOnly]private float _playerZPostion;
    [SerializeField][ReadOnly]private Player _player;
    public GameObject playerPrefab;
    public Transform playerSpawn;
    public GameObject mainCamera;
    public TextMeshProUGUI scoreTextMesh;
    public ItemType coinType = ItemType.COIN;
    public float animationSpawnPlayerDuration = 1f;
    public Ease easeSpwanPlayer = Ease.OutBack;
    private Tween _spawnPlayerTween;

    [Header("Plataform Generator")]
    [SerializeField][ReadOnly]private float _lastPlataformZPosition;
    [SerializeField][ReadOnly]private PlataformGenerator _lastPlataform;
    public float distanceToCreateNewPlataform = 10f;

    [Header("Ui Components")]
    public GameObject canvasMainUi;
    public GameObject canvasRestartUi;
    public UiCoinBase uiCoinBase;

    private ItemManager _itemManager;
    private ItemSetup _itemSetup;
    private void OnValidate()
    {
        if(loadSceneHelper == null) loadSceneHelper = GetComponent<LoadSceneHelper>();
    }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<State>();
        stateMachine.Init();
        stateMachine.RegisterStates(State.MENU, new GameManagerStateMenu());
        stateMachine.RegisterStates(State.START, new GameManagerStateStart());
        stateMachine.RegisterStates(State.RESTART, new GameManagerStateRestart());
    }

    private void Start()
    {
        _itemManager = ItemManager.instance;
        _player = Player.instance;
        _lastPlataform = PlataformGenerator.instance;
        stateMachine.SwitchState(State.MENU);
    }

    private void Update()
    {
        if (_player)
        {
            GeneratePlaform();
            if(scoreTextMesh != null)
            {
                scoreTextMesh.text = ((int)_player.GetPlayerTravelledDistance()).ToString();
            }
        }
    }

    [Button]
    public void SpawnPlayer()
    {
        if (_player) return;
        GameObject player = Instantiate(playerPrefab);
        _player = Player.instance;
        TweenSpawnPlayer(player);
    }

    public void OnClickButtonMainUI()
    {
        SpawnPlayer();
        canvasMainUi.SetActive(false);
        stateMachine.SwitchState(State.START);
    }

    public void OnClickButtonRestartUI()
    {
        loadSceneHelper.LoadLevel(0);
    }

    private void TweenSpawnPlayer(GameObject player)
    {
        if (_spawnPlayerTween != null) _spawnPlayerTween = null;
        _spawnPlayerTween = player.transform.DOScale(0, animationSpawnPlayerDuration).SetEase(easeSpwanPlayer).From();
        _spawnPlayerTween.OnComplete(() =>
        {
            SpawnMainCamera(player);
        });
    }

    public void UpdateCurrentUICoin()
    {
        _itemSetup = _itemManager.GetItemSetupByType(coinType);
        uiCoinBase?.UpdateUi(_itemSetup.currentValue);
    }

    public void UpdateTotalUICoin()
    {
        _itemSetup = _itemManager.GetItemSetupByType(coinType);
        uiCoinBase?.UpdateUi(_itemSetup.soInt.value);
    }

    private void SpawnMainCamera(GameObject player)
    {
        GameObject camera = Instantiate(mainCamera);
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        _player.canMove = true;
    }

    private void GeneratePlaform()
    {
        _playerZPostion = _player.GetCurrentZPosition();
        _lastPlataformZPosition = _lastPlataform.GetLastEndZPositionPlataform();
        if ((_lastPlataformZPosition - _playerZPostion) <= distanceToCreateNewPlataform)
        {
            PlataformGenerator.instance.Generate();
        }
    }
}
