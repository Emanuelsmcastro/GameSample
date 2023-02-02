using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

public class PlataformGenerator : Singleton<PlataformGenerator>
{
    [Header("Configs")]
    public List<PlataformBase> plataformBaseList;
    public PlataformBase startPlataform;
    public int totalPlataformBuffer = 5;
    [SerializeField][ReadOnly]private List<PlataformBase> _lastPlataformList;

    [Header("Current Plataform")]
    [SerializeField][ReadOnly]private PlataformBase _currentPlataform;
    [SerializeField][ReadOnly]private float _ZEndPosition;
    [SerializeField][ReadOnly]private float _ZStartPosition;

    [Header("Last Plataform")]
    [SerializeField][ReadOnly] private PlataformBase _lastPlataform;
    [SerializeField][ReadOnly] private float _lastZEndPosition;
    [SerializeField][ReadOnly] private float _lastZStartPosition;

    private void Start()
    {
        StartGenerate();
    }

    public void StartGenerate()
    {
        _lastPlataform = startPlataform;
        _lastZEndPosition = _lastPlataform.EndPosition.transform.position.z;
        _lastPlataformList.Add(_lastPlataform);
    }

    [Button]
    public void Generate()
    {
        if (plataformBaseList.Count <= 0) return;
        float plataformWidth = 0;

        _currentPlataform = plataformBaseList[Random.Range(0, plataformBaseList.Count)];
        _ZStartPosition = _currentPlataform.StartPosition.transform.position.z;
        _ZEndPosition = _currentPlataform.EndPosition.transform.position.z;
        plataformWidth = _ZEndPosition - _ZStartPosition;
        

        var newPlataform = Instantiate(_currentPlataform, transform);

        newPlataform.transform.position = new Vector3(newPlataform.transform.position.x, newPlataform.transform.position.y, _lastZEndPosition);
        newPlataform.transform.position += new Vector3(0, 0, plataformWidth / 2);

        _lastPlataform = newPlataform;
        _lastZEndPosition = newPlataform.EndPosition.transform.position.z;
        _lastPlataformList.Add(_lastPlataform);
    }

    public float GetLastEndZPositionPlataform()
    {
        return _lastZEndPosition;
    }

    private void Update()
    {
        if(_lastPlataformList.Count > totalPlataformBuffer)
        {
            PlataformBase plataformToRemove = _lastPlataformList[0];
            _lastPlataformList.Remove(plataformToRemove);
            Destroy(plataformToRemove.gameObject);

        }
    }

}

