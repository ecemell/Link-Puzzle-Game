using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private int _moves;
    [SerializeField] private UI_Controller _uiController;

    private void Start()
    {
        _uiController.UpdateMoveText(_moves);
    }

    private int DecreaseMoveCount()
    {
        _uiController.UpdateMoveText(--_moves);
        return _moves;
    }

    public void MoveCheckher()
    {
        if(DecreaseMoveCount() <= 0)
        {
            _uiController.LevelEndPanel(false);
        }    
    } 
}
