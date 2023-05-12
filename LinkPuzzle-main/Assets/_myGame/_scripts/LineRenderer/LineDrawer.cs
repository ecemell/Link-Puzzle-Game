using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : Singleton<LineDrawer>
{
    #region AboutLines

    [SerializeField] private Line _linePrefab;
    private Line _currentLine;

    public const float RESOLUTION = 0.01f;
    [SerializeField, Range(0f, 0.3f)] private float _width;

    [SerializeField] private List<Line> _lines;
    #endregion

    #region Draw
    private bool _isDrawable;
    private bool _isFirstTouch;
    private bool _isDifferentColor;
    private bool _isComp;
    private bool _isX;
    public bool _isCompleted;
    private Ball _selectedBall;
    private Ball _currentBall;
    #endregion
    [SerializeField] private LineQuestController _questController;
    [SerializeField] private LineUI_Controller _uiController; 

    private void OnEnable()
    {
        _lines = new List<Line>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
             _isComp = false;
             _isX = false;
            _isFirstTouch = true;
            _isDrawable = Drawable();
            if (!_isDrawable) return;
            
            StartDrawing();
        }
                
        if (Input.GetMouseButton(0) && _isDrawable) 
        {
            if (Drawable())
            {
                _isFirstTouch = false;
                if (_selectedBall != _currentBall)
                {
                    if (_selectedBall.GetBallColor() == _currentBall.GetBallColor())
                    {
                        _isDrawable = false;
                        if(!_isDifferentColor)
                        {
                            if(CheckCollisions())
                            {
                                _isX = true;
                            }
                            else
                            {
                              if(_currentBall.Line is null || !_currentBall.Line.gameObject.activeInHierarchy)
                              {
                                    _isCompleted = true;
                                    _questController.LineHasDone(true);
                                    _currentLine.Status = TaskStatus.Success;
                                    //Completed for this color...
                                    _currentBall.Line = _currentLine;
                                    _selectedBall.Line = _currentLine;
                                    _uiController.TickState(_selectedBall.GetBallColor(), true);
                                    MoveCheckher();
                              }
                              else
                              {
                                   _isComp = true;
                              } 

                            }
                        
                             _isDifferentColor = false;           
                        }                                         
                    }
                    else
                    {  
                            
                          _isDifferentColor = true;
                           return;
                    }
                }
            }
            _currentLine.SetPosition(MousePos());
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if(CheckCollisions())
            {
                MoveCheckher();
                if(_isX)
                {
                    DeleteNew();
                    _isFirstTouch = false; 
                    _isDifferentColor = false;
                    return;
                }
                else if(_isDifferentColor)
                {
                    DeleteNew();
                    _isFirstTouch = false; 
                    _isDifferentColor = false;
                    return;
                } 
            }

            _isFirstTouch = false;          
            if(_isDifferentColor){
                DeleteNew();
                MoveCheckher();
            }else if(_isComp){
                DeleteNew();
                MoveCheckher();
            }
            else{
                DeleteLine();
            }                
            _isDifferentColor = false;

        } 
        
    }

    private void StartDrawing()
    {
        if (Drawable())
        {
            _currentLine = Instantiate(_linePrefab, MousePos(), Quaternion.identity);
            _currentLine.CurrentBallColor = _selectedBall.GetBallColor();
            _lines.Add(_currentLine);
        }
        else
        {
            _currentLine = null;
            return;
        }        
    }

    private Vector2 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public float GetWidth()
    {
        return _width;
    }

    private bool CheckCollisions()
    {
        int lineCount = _lines.Count;   
        
        if(lineCount < 2) return false;

        for (int i = 0; i < lineCount - 1; i++)
        {
            for (int j = i + 1; j < lineCount; j++)
            {
                Line line1 = _lines[i];
                Line line2 = _lines[j];

                if (line1.CheckCollisionWithOtherLine(line2))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool Drawable()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit hit)) return false;

        if(!hit.transform.gameObject.TryGetComponent(out Ball ball)) return false;

        if(_isFirstTouch) _selectedBall = _currentBall = ball;
        else _currentBall = ball;

        DeleteCompletedLine();
        
        return true;
    }

    private void DeleteLine()
    {
        if (_selectedBall is null) return;
        
        if (_selectedBall != _currentBall) return;

        if(_lines.Count > 0 )
        {
          _lines[^1].gameObject.SetActive(false);
          _lines.Remove(_lines[^1]);
          _selectedBall.Status = TaskStatus.NotSuccess; 
          _selectedBall = null;
        }         
 
    }

    private void DeleteNew()
    {       
       if(_lines.Count > 0)
       {
         _lines[^1].gameObject.SetActive(false);
         _lines.Remove(_lines[^1]);
        // _selectedBall = null;
        // _currentBall = null;  

       } 
    }

    private void DeleteCompletedLine()
    {        
            if(_selectedBall is not null)
            {
                if(_selectedBall.Line is not null)
                {
                    if(_lines.Count > 0)
                    {
                        if(_selectedBall.Line.gameObject.activeInHierarchy)
                        {
                            _questController.LineHasDone(false); 
                            _selectedBall.Line.Status = TaskStatus.NotSuccess;
                            _uiController.TickState(_selectedBall.GetBallColor(), false);
                            MoveCheckher(); 
                        } 
                        _selectedBall.Line.gameObject.SetActive(false);
                        _lines.Remove(_selectedBall.Line);
                        //_selectedBall.Line = null;
                        //_currentBall.Line = null;
                    }
                }

            }  
    }

    private void MoveCheckher()
    {
        if(_questController.DecreaseMoveCount() <= 0)
        {
            _uiController.LevelEndPanel(false);
        }    
    }
}

