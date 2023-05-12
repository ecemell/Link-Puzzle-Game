using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallColor _ballColor;
    private Action<Ball> _returnToPool;
    private Line _line = null;
    public Line Line { get => _line; set => _line = value; }
   
    public TaskStatus Status {get;set;} = TaskStatus.NotSuccess;
    private void OnDisable()
    {
        ReturnToPool();
    }

    public BallColor GetBallColor()
    {
        return _ballColor;
    }

    public void Initialize(Action<Ball> returnAction)
    {
        _returnToPool = returnAction;   
    }

    public void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }

}


