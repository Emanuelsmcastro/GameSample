using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManagerStateMenu : StateBase
{
    private GameManager _gameManager = GameManager.instance;
    private GameObject _mainUI;
    private UiCoinBase _coinBase;
    private Tween _currentTween;
    private Tween _currentTweenButtonStart;
    private Button _startButton;
    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _coinBase = _gameManager.uiCoinBase;
        _mainUI = _gameManager.canvasMainUi;
        _mainUI.SetActive(true);
        StartAnimationUICoinBase();
        _gameManager.UpdateTotalUICoin();
        _startButton = _gameManager.canvasMainUi.GetComponentInChildren<Button>();
        ButtonAnimation();
        _startButton.onClick.AddListener(_gameManager.OnClickButtonMainUI);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _mainUI.SetActive(false);
    }

    private void StartAnimationUICoinBase()
    {
        if(_currentTweenButtonStart == null)
        {
            _currentTweenButtonStart = _coinBase.gameObject.transform.DOScale(0, 1f).SetEase(Ease.OutBack).From();
            _currentTweenButtonStart.OnComplete(() =>
            {
                _currentTweenButtonStart = null;
            });
        }
    }

    private void ButtonAnimation()
    {
        if (_currentTween == null)
        {
            _currentTween = _startButton.gameObject.transform.DOScale(0, .5f).SetEase(Ease.OutBack).From();
            _currentTween.OnComplete(() =>
            {
                _currentTween = null;
            });
        }
    }
}
