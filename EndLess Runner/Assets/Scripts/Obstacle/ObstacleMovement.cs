using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Configs")]
    public List<Transform> positions;
    public GameObject graph;
    [Header("Animation")]
    public float timeToMove = 1f;
    public Ease ease = Ease.OutBack;


    private int _currentPosition = 0;
    private Tween _currenTween;

    private void Update()
    {
        if (positions.Count == 0) return;
        if (_currenTween == null) 
        {
            _currenTween = graph.transform.DOMove(positions[_currentPosition].position, timeToMove).SetEase(ease);
            _currenTween.onComplete = FinishTween;
        }
    }

    public void FinishTween()
    {
        _currenTween = null;
        _currentPosition += 1;

        if(_currentPosition >= positions.Count)
        {
            _currentPosition = 0;
        }
    }
}
