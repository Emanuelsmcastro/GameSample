using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UiCoinBase : MonoBehaviour 
{
    [Header("Config")]
    public GameObject coinGameObject;
    public TextMeshProUGUI coinCountTextMesh;
    [Header("Animation")]
    public float rotateSpeed = 3f;
    public float animationDuration = .2f;
    public Ease ease = Ease.OutBack;
    private Tween _currentTween;

    public void UpdateUi(int amount)
    {
        if(coinCountTextMesh != null)
        {
            coinCountTextMesh.text = amount.ToString();
            UIAnimationGetCoin();
        }
    }

    private void UIAnimationGetCoin()
    {
        if (_currentTween == null)
        {
            _currentTween = coinGameObject?.transform.DOScale(coinGameObject.transform.localScale * 1.2f, animationDuration).SetEase(ease).From();
            _currentTween.OnComplete(() =>
            {
                _currentTween = null;
            });
        }
    }

    private void Update()
    {
        coinGameObject.transform.Rotate(new Vector3(0, 10, 0) * rotateSpeed * Time.deltaTime);
    }
}
