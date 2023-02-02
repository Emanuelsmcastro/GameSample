using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.StateMachine;
using DG.Tweening;

public class GameManagerStateStart : StateBase
{
    private GameManager _gameManager = GameManager.instance;
    private UiCoinBase _uiCoinBase;
    private Tween _currentTween;

    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter(objs);
        _uiCoinBase = _gameManager.uiCoinBase;
        StartAnimationUICoinBase();
        _uiCoinBase.UpdateUi(0);
    }

    private void StartAnimationUICoinBase()
    {
        if (_currentTween == null)
        {
            _currentTween = _uiCoinBase.gameObject.transform.DOScale(0, 1f).SetEase(Ease.OutBack).From();
            _currentTween.OnComplete(() =>
            {
                _currentTween = null;
            });
        }
    }
}
