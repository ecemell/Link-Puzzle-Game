using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer _renderer;
    private Vector3 _previousPos;
    private BallColor _currentBallColor;
    public TaskStatus Status {get;set;} = TaskStatus.NotSuccess;

    public BallColor CurrentBallColor 
    { 
        get => _currentBallColor;
        set
        {
            _currentBallColor = value;  
            SetLineColor(_currentBallColor);
        }
    }

    private Color _lineColor;
    private List<Vector2> _points = new List<Vector2>();

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _renderer.positionCount = 1;
        _renderer.startWidth = _renderer.endWidth = LineDrawer.Instance.GetWidth();
        _previousPos = transform.position;
    }

    public void SetPosition(Vector2 pos)
    {
        if(!CanAppend(pos)) return;

        _points.Add(pos);

        if (_previousPos == transform.position) 
        {
            _renderer.SetPosition(0, pos);
        }
        else
        {
            _renderer.positionCount++;
            _renderer.SetPosition(_renderer.positionCount - 1, pos);
        }

        _previousPos = pos; 

    }

    private bool CanAppend(Vector2 pos)
    {
        if(_renderer.positionCount == 0) return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > LineDrawer.RESOLUTION;
    }

    private void SetLineColor(BallColor color)
    {
        switch (color)
        {
            case BallColor.Red:
                _lineColor = Color.red;
                break;
            case BallColor.Blue:
                _lineColor = Color.blue;
                break;
            case BallColor.Orange:
                _lineColor = new Color(1, 0.15f, 0);
                break;
            case BallColor.Purple:
                _lineColor = new Color(0.5f, 0, 0.5f);
                break;
            case BallColor.Green:
                _lineColor = Color.green;
                break;
            case BallColor.None:
                break;
            default:
                break;
        }

        _renderer.startColor = _lineColor;
        _renderer.endColor = _lineColor;
    }

    public bool CheckCollisionWithOtherLine(Line otherLine)
    {
        const float collisionThreshold = 0.17f;
        float thresholdSqr = collisionThreshold * collisionThreshold;

        for (int i = 0; i < _points.Count; i++)
        {
            for (int j = 0; j < otherLine._points.Count; j++)
            {

                Vector2 difference = _points[i] - otherLine._points[j];
                float distanceSqr = difference.sqrMagnitude;
                
                if (distanceSqr < thresholdSqr) return true;                
            }
        }
        return false;
    }
}
