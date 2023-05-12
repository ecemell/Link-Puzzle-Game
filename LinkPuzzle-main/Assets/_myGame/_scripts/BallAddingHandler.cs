using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class BallAddingHandler : MonoBehaviour
{
  
    [SerializeField] private BallController _ballController;
    [SerializeField] private GameObject[] _barPrefab;
    private List<GameObject> _currentBarList;
    [SerializeField] private Material[] _matColorArray;
    [SerializeField] private Renderer _renderer;
    public List<GameObject> CurrentBarList { get => _currentBarList; set=> _currentBarList = value; }

    private void Awake()
    {
        _currentBarList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Ball ball)) return;

        _ballController.CurrentBall = ball;

        if (_ballController.GetCurrentColor() != ball.GetBallColor()) return;

        if (_ballController.BallList.Contains(ball)) return;
        
        _ballController.BallList.Add(ball);
        AddBar(_ballController.GetCurrentColor());
        SetHandlerPosition();
    }

    public void SetHandlerPosition()
    {
        if (_ballController.BallList.Count == 0) return;
        transform.position = _ballController.BallList[^1].transform.position;
    }

    private void AddBar(BallColor color)
    {
        GameObject currentPrefab;
        switch (color)
        {
            case BallColor.Red:
                currentPrefab = _barPrefab[0];
                break;
            case BallColor.Blue:
                currentPrefab = _barPrefab[1];
                break;
            case BallColor.Orange:
                currentPrefab = _barPrefab[2];
                break;
            case BallColor.Purple:
                currentPrefab = _barPrefab[3];
                break;
            case BallColor.Green:
                currentPrefab = _barPrefab[4];
                break;
            default:
                currentPrefab = null;
                break;
        }
       
        GameObject lastBar = Instantiate(currentPrefab, _ballController.BallList[^2].transform.position, Quaternion.identity);
        lastBar.transform.up = (_ballController.BallList[^1].transform.position - _ballController.BallList[^2].transform.position).normalized;
        _currentBarList.Add(lastBar);
    }

    public void SetFalseAllBars()
    {
        if (_currentBarList.Count == 0) return;

        for (int i = 0; i < _currentBarList.Count; i++)
        {
            _currentBarList[i].SetActive(false);
        }
    }

    public void ChangeMaterial(BallColor color)
    {
        Material currentmaterial;
        switch (color)
        {
            case BallColor.Red:
                currentmaterial = _matColorArray[0];
                break;
            case BallColor.Blue:
                currentmaterial = _matColorArray[1];
                break;
            case BallColor.Orange:
                currentmaterial = _matColorArray[2];
                break;
            case BallColor.Purple:
                currentmaterial = _matColorArray[3];
                break;
            case BallColor.Green:
                currentmaterial = _matColorArray[4];
                break;
            default:
                currentmaterial = _matColorArray[0];
                break;
        }

        _renderer.material = currentmaterial;
    }

}
