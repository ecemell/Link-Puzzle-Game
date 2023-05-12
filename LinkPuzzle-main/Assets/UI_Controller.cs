using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private TMP_Text _moves;
    [SerializeField] private GameObject[] _levelEndPanels;
    [SerializeField] private Ball_UI[] _uiBalls;
    
    [Serializable] 
    private struct Ball_UI
    {
        public GameObject ball;
        public GameObject tick;
        public TMP_Text text;
    } 
    
    public void Ball_UI_Starter(BallColor color, int count)
    {
        switch (color)
        {
            case BallColor.Red:
                _uiBalls[0].ball.SetActive(true);
                _uiBalls[0].text.text = count.ToString();
                break;
            case BallColor.Blue:
                _uiBalls[1].ball.SetActive(true);
                _uiBalls[1].text.text = count.ToString();
                break;
            case BallColor.Orange:
                _uiBalls[2].ball.SetActive(true);
                _uiBalls[2].text.text = count.ToString();
                break;
            case BallColor.Purple:
               _uiBalls[3].ball.SetActive(true);
               _uiBalls[3].text.text = count.ToString();
                break;
            case BallColor.Green:
               _uiBalls[4].ball.SetActive(true);
               _uiBalls[4].text.text = count.ToString();
                break;
            default:
               _uiBalls[5].ball.SetActive(true);
               _uiBalls[5].text.text = count.ToString();
                break;
        }
    }

    public void Ball_UI_UpdateText(BallColor color, int count)
    {
        switch (color)
        {
            case BallColor.Red:
                _uiBalls[0].text.text = count.ToString();
                break;
            case BallColor.Blue:
                _uiBalls[1].text.text = count.ToString();
                break;
            case BallColor.Orange:
                _uiBalls[2].text.text = count.ToString();
                break;
            case BallColor.Purple:
               _uiBalls[3].text.text = count.ToString();
                break;
            case BallColor.Green:
               _uiBalls[4].text.text = count.ToString();
                break;
            default:
               _uiBalls[5].text.text = count.ToString();
                break;
        }
    }

    public void Ball_UI_Tick(BallColor color)
    {
        switch (color)
        {
            case BallColor.Red:
                 _uiBalls[0].text.gameObject.SetActive(false);
                 _uiBalls[0].tick.SetActive(true);
                break;
            case BallColor.Blue:
                 _uiBalls[1].text.gameObject.SetActive(false);
                 _uiBalls[1].tick.SetActive(true);
                break;
            case BallColor.Orange:
                 _uiBalls[2].text.gameObject.SetActive(false);
                 _uiBalls[2].tick.SetActive(true);
                break;
            case BallColor.Purple:
                 _uiBalls[3].text.gameObject.SetActive(false);
                 _uiBalls[3].tick.SetActive(true);
                break;
            case BallColor.Green:
                 _uiBalls[4].text.gameObject.SetActive(false);
                 _uiBalls[4].tick.SetActive(true);
                break;
            case BallColor.None:
                 _uiBalls[5].text.gameObject.SetActive(false);
                 _uiBalls[5].tick.SetActive(true);
                break;
        }
    }

    public void UpdateMoveText(int moves)
    {
        _moves.text = moves.ToString();
    }

    public void LevelEndPanel(bool isLevelDone)
    {
        if(isLevelDone)  _levelEndPanels[0].SetActive(true);
        else _levelEndPanels[1].SetActive(true);
    }
}
