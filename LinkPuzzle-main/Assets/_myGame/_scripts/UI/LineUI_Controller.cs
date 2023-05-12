using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineUI_Controller : MonoBehaviour
{
    [SerializeField] private TMP_Text _moves;
    [SerializeField] private GameObject[] _ticks;
    [SerializeField] private GameObject[] _levelEndPanels;

    public void UpdateMoveText(int moves)
    {
        _moves.text = moves.ToString();
    }

    public void TickState(BallColor color, bool state)
    {
        switch (color)
        {
            case BallColor.Red:
                _ticks[0].SetActive(state);
                break;
            case BallColor.Blue:
                _ticks[1].SetActive(state);
                break;
            case BallColor.Orange:
                _ticks[2].SetActive(state);
                break;
            case BallColor.Purple:
                _ticks[3].SetActive(state);
                break;
            case BallColor.Green:
                _ticks[4].SetActive(state);
                break;
        }
    }

    public void LevelEndPanel(bool isLevelDone)
    {
        if(isLevelDone)  _levelEndPanels[0].SetActive(true);
        else _levelEndPanels[1].SetActive(true);
    }
}
