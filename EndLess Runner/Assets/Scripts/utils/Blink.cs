using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public float blinkTime = .5f;
    public int blinkAmout = 10;
    public float blinkMultPerTime = .7f;
    public Action<Blink> finishBlink;

    private Coroutine _currentCoroutine;

    [Button]
    public void StartBlink()
    {
        if(_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(BlinkCoroutine());
        }
        else
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            SetActiveGameObjects(true);
            _currentCoroutine = StartCoroutine(BlinkCoroutine());
        }
    }

    IEnumerator BlinkCoroutine()
    {
        float time = blinkTime;
        for(var i = 0; i < blinkAmout; i++)
        {

            SetActiveGameObjects(false);
            SetActiveGameObjects(false);

            yield return new WaitForSeconds(time);

            SetActiveGameObjects(true);

            yield return new WaitForSeconds(time);

            time *= .7f;
        }
        _currentCoroutine = null;
        finishBlink?.Invoke(this);
    }

    private void SetActiveGameObjects(bool state)
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(state);
        }
    }
}
