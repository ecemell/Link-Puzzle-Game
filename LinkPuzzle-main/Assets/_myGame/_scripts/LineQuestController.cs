using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineQuestController : MonoBehaviour
{
    [SerializeField] private LineUI_Controller _uiController;
    [SerializeField] private int _requiredLineCount;
    [SerializeField] private int _moveCount;
    public int RequiredLineCount{set => _requiredLineCount = value;}

    private void Start()
    {   
        _uiController.UpdateMoveText(_moveCount);
    }

    public void LineHasDone(bool isDone)
    {
        if(isDone)
        {
            if(IsLevelFinished())
            {
                 Debug.Log("Finished");
                 _uiController.LevelEndPanel(true);
            }
        }
        else   
            _requiredLineCount++;
                           
        Debug.Log(_requiredLineCount);
    }

    private bool IsLevelFinished()
    {
        _requiredLineCount--;
        return _requiredLineCount <= 0;   
    }

    public int DecreaseMoveCount()
    {
        _uiController.UpdateMoveText(--_moveCount);
        return _moveCount;
    }
}
