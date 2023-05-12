using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] _barPrefab;
    [SerializeField] private List<Ball> _ballList; 
    [SerializeField] private BallColor _currentColor = BallColor.None;
    [SerializeField] private BallAddingHandler _ballAddingHandler;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private UI_Controller _uiController;
    public GameObject[,] _balls;
    private Ball _currentBall;

    private const int COLUMN = 6;
    private const int ROW = 6;
    private const float OFFSET = 0.8f;

    const float DELAY = .1f;
    WaitForSeconds _wait;

    private BallPopManager _popManager;
    private List<BallPopTask> _tasks;
    #endregion

    #region Properties
    public List<Ball> BallList
    {
        get
        {
            return _ballList;
        }

        set 
        {
            _ballList = value;
        }
    }
    public Ball CurrentBall
    {
        set
        {
            _currentBall = value;
        }
    }

    #endregion

    #region Unity

    private void Awake()
    {
        _wait = new WaitForSeconds(DELAY);
        _balls = new GameObject[ROW, COLUMN];
        _tasks = new List<BallPopTask>();
    }
  
    private void Start()
    {
        SortTheBalls();
        _popManager = BallPopManager.Instance;
        _tasks = _popManager.GetTaskList();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FirstBallHandler();
            AddingHandlerScale();
            BallListHandler();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _ballAddingHandler.gameObject.SetActive(false);
            _ballAddingHandler.SetFalseAllBars();
            _ballAddingHandler.CurrentBarList.Clear(); 
            DestroyBalls();
        }
    }

    #endregion

    #region Methods

    private void SortTheBalls()
    {
        Vector3 pos = Vector3.zero;

        for (int column = 0; column < COLUMN; column++)
        {
            for (int row = 0; row < ROW; row++)
            {
                pos.y = row * OFFSET;
                pos.x = column * OFFSET;

                var ball = Instantiate(GetRandomBall(), pos, Quaternion.identity);
                _balls[column, row] = ball;  
            }
        }
    }

    private void FirstBallHandler()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        if (!hit.transform.gameObject.TryGetComponent(out Ball ball)) return;

        if (_ballList.Contains(ball)) return;

        if (_ballList.Count > 0) return;
        
        _ballList.Add(ball);
        _currentColor = _ballList[0].GetBallColor();
        _ballAddingHandler.ChangeMaterial(_currentColor);
        _ballAddingHandler.gameObject.SetActive(true);
        _ballAddingHandler.transform.position = _ballList[0].transform.position;
    }
    private GameObject GetRandomBall()
    {
        int number = Random.Range(0, 5);
        return _barPrefab[number];
    }
    private void ClearList()
    {
        _ballList.Clear();
    }

    private void DestroyBalls()
    {
        int minBallCount = 3;
      
        if (_ballList.Count >= minBallCount)
        {
            for (int i = 0; i < _ballList.Count; i++)
            {
                //_ballList[i].Initialize(Push);
                _ballList[i].gameObject.SetActive(false);
            }
            ManageQuests(_currentColor, _ballList.Count);
            TaskController();
            ClearList();
            SlideDown();
            _levelHandler.MoveCheckher();
            StartCoroutine(CreateNewBalls());
        }
        else
            ClearList();
    
        _currentColor = BallColor.None;
    }
    
    private void AddingHandlerScale()
    {

        if (_ballList.Count == 0) return;

        float minYscale = 0f;
        float maxYscale = 1.3f;

        Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = 0;

        Vector3 yAxisMagnituade = (currentPos - _ballList[^1].transform.position);

        _ballAddingHandler.transform.up = yAxisMagnituade.normalized;

        float yScale = Mathf.Clamp(yAxisMagnituade.magnitude, minYscale, maxYscale);

        _ballAddingHandler.transform.localScale = new Vector3(_ballAddingHandler.transform.localScale.x,
            yScale,
            _ballAddingHandler.transform.localScale.z);
    }

    private void BallListHandler()
    {
        if (_ballList.Contains(_currentBall))
        {
            int index = 0;
            for (int i = 0; i < _ballList.Count; i++)
            {
                if (_ballList[i] == _currentBall)
                {
                    index = ++i;
                    break;
                }
            }

            for (int j = index; j < _ballList.Count; j++)
            {
                _ballList.Remove(_ballList[j]);

                var obje = _ballAddingHandler.CurrentBarList[j - 1];
                obje.gameObject.SetActive(false);
                _ballAddingHandler.CurrentBarList.Remove(obje);
            }
        }

        _ballAddingHandler.SetHandlerPosition();
    }

    private void SlideDown()
    {
        float lerpSpeed = 5f;
        int emptySlots = 0;
        for (int i = 0; i < COLUMN; i++)
        {
            for (int j = 0; j < ROW ; j++)
            {
                if (!_balls[i, j].activeInHierarchy || _balls[i, j] == null)
                {
                    emptySlots++;
                }
                else if (emptySlots > 0)
                {
                    _balls[i, j - emptySlots] = _balls[i, j];
                    _balls[i, j - emptySlots].transform.position = Vector3.Lerp(_balls[i, j - emptySlots].transform.position,
                        new Vector3(i * OFFSET, (j - emptySlots) * OFFSET, 0), lerpSpeed);
                    _balls[i, j] = null;
                }
            }

            emptySlots = 0;
        }
    }

    IEnumerator CreateNewBalls()
    {
        
        yield return _wait;
        
        for (int i = 0; i < COLUMN; i++)
        {
            for (int j = 0; j < ROW; j++)
            {
                if (_balls[i, j] == null || !_balls[i, j].activeInHierarchy)
                {
                    Vector3 ballPos = new Vector3(i * OFFSET, j * OFFSET, 0);

                    GameObject ball = Instantiate(GetRandomBall(), ballPos, Quaternion.identity);
                    _balls[i, j] = ball;
                }
            }
        }
    }

    private void Push(Ball ball)
    {
        //Effect
    }

    private void ManageQuests(BallColor color, int count)
    {
        if(_tasks.Count != 0)
        {
            for (int i = 0; i < _tasks.Count; i++)
            {                
                if(_tasks[i].Status == TaskStatus.Success) continue;
                if(_tasks[i].Color != color) continue;
               
                _tasks[i].Count -= count;
                _tasks[i].Count = Mathf.Max(_tasks[i].Count, 0);
                _uiController.Ball_UI_UpdateText(_tasks[i].Color, _tasks[i].Count);

                if (_tasks[i].Count <= 0)
                {
                    _tasks[i].Status = TaskStatus.Success;
                    _uiController.Ball_UI_Tick(_tasks[i].Color);
                }               
            }
        }
        
        if(_popManager.GetTask() != null)
        {
            var task = _popManager.GetTask();

            if (!_tasks.Contains(task))
                _tasks.Add(task);   

            if (task.Status == TaskStatus.Success) return;

            task.Count -= count;
            task.Count = Mathf.Max(task.Count, 0);
            _uiController.Ball_UI_UpdateText(BallColor.None, task.Count);

            if (task.Count <= 0)
            {
                task.Status = TaskStatus.Success;
                _uiController.Ball_UI_Tick(BallColor.None);
            }
        }

 
    }

    private void TaskController() 
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            if (_tasks[i].Status == TaskStatus.NotSuccess) return;
        }
        
        _uiController.LevelEndPanel(true);
    }

    #endregion

    #region Getters

    public BallColor GetCurrentColor()
    {
        return _currentColor;   
    }

    #endregion

}
