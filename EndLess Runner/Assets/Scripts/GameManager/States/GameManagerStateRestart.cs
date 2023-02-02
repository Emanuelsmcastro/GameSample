using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.StateMachine;
using DG.Tweening;

public class GameManagerStateRestart : StateBase
{
    private GameManager _gameManager = GameManager.instance;
    private GameObject _restartUI;
    private Tween _currentTween;
    private Button _restartButton;
    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _restartUI = _gameManager.canvasRestartUi;
        _restartUI.SetActive(true);
        _restartButton = _gameManager.canvasRestartUi.GetComponentInChildren<Button>();
        ButtonAnimation();
        _restartButton.onClick.AddListener(_gameManager.OnClickButtonRestartUI);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _restartUI.SetActive(false);
    }

    private void ButtonAnimation()
    {
        if(_currentTween == null)
        {
            _currentTween = _restartButton.gameObject.transform.DOScale(0, .5f).SetEase(Ease.OutBack).From();
            _currentTween.OnComplete(() =>
            {
                _currentTween = null;
            });
        }
    }
}
