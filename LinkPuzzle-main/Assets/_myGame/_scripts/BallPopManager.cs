using System.Collections.Generic;
using UnityEngine;

public class BallPopManager : Singleton<BallPopManager> 
{
    private List<BallColor> _ballColors = new List<BallColor> { BallColor.Red, BallColor.Blue, BallColor.Orange, BallColor.Purple, BallColor.Green };
    private List<BallPopTask> _taskList = null;
    private BallPopTask _popTask = null;
   
    [SerializeField] private UI_Controller _uiController;
    protected override void Awake()
    {
        base.Awake();
        _taskList = new List<BallPopTask>();
        int questType = Random.Range(0, 4);
        QuestType quest = (QuestType)questType;
        SetQuest(quest, 10,15,50);
    }

    private void OnDisable()
    {
        _taskList.Clear(); 
        _popTask = null;    
    }

    private void SetQuest(QuestType quest, int minBalls, int maxBalls,int totalBallCount)
    {
        switch (quest)
        {
            case QuestType.OnlyColor:
                _taskList = GenerateRandomColorTasks(minBalls, maxBalls);
                for (int i = 0; i < _taskList.Count; i++)
                {
                    _uiController.Ball_UI_Starter(_taskList[i].Color,_taskList[i].Count);
                }
                break;
            case QuestType.AllColorsSameCount:
                _taskList = GenerateRandomColorSameCountTasks(minBalls, maxBalls);
                for (int i = 0; i < _taskList.Count; i++)
                {
                    _uiController.Ball_UI_Starter(_taskList[i].Color,_taskList[i].Count);
                }
                break;
            case QuestType.TotalBall:
                 _popTask = GenerateTotalBallCount(totalBallCount, totalBallCount + 20);
                 _uiController.Ball_UI_Starter(BallColor.None, _popTask.Count);
                break;
            case QuestType.TotalBallAndRandomBalls:
                _taskList = GenerateRandomColorTasks(3, 5);
                _popTask =  GenerateTotalBallCount(minBalls, maxBalls);
                for (int i = 0; i < _taskList.Count; i++)
                {
                    _uiController.Ball_UI_Starter(_taskList[i].Color,_taskList[i].Count);
                }
                _uiController.Ball_UI_Starter (BallColor.None, _popTask.Count);
                break;
        }

    }

    private List<BallPopTask> GenerateRandomColorTasks(int minBalls, int maxBalls)
    {
        List<BallPopTask> tasks = new List<BallPopTask>();
        List<BallColor> selectedColors = new List<BallColor>();

        // Rastgele renk say�s� se�in (minimum 2,maximum 5)
        int colorCount = Random.Range(1, _ballColors.Count + 1);

        for (int i = 0; i < colorCount; i++)
        {
            // Rastgele bir renk se�in ve listeden ��kar�n
            int randomIndex = Random.Range(0, _ballColors.Count);
            BallColor selectedColor = _ballColors[randomIndex];
            _ballColors.RemoveAt(randomIndex);

            // Rastgele bir top say�s� belirleyin
            int ballCount = Random.Range(minBalls, maxBalls +1);

            // Yeni bir g�rev olu�turun ve listeye ekleyin
            BallPopTask task = new BallPopTask(selectedColor, ballCount);
            tasks.Add(task);

            // Se�ilen renkleri ba�ka bir listeye ekleyin
            selectedColors.Add(selectedColor);
        }

        // Se�ilen renkleri tekrar ana renkler listesine ekleyin
        _ballColors.AddRange(selectedColors);

        return tasks;
    }

    public BallPopTask GenerateTotalBallCount(int minBalls, int maxBalls)
    {
        int ballCount = Random.Range(minBalls, maxBalls + 1);        
        BallPopTask task = new BallPopTask(ballCount);
                   
        return task;
    }

    public List<BallPopTask> GenerateRandomColorSameCountTasks(int minBalls, int maxBalls)
    {
        List<BallPopTask> tasks = new List<BallPopTask>();
        List<BallColor> selectedColors = new List<BallColor>();

        int ballCount = Random.Range(minBalls, maxBalls + 1);

        // Rastgele renk say�s� se�in (minimum 2,maximum 5)
        int colorCount = Random.Range(2, _ballColors.Count + 1);

        for (int i = 0; i < colorCount; i++)
        {
            // Rastgele bir renk se�in ve listeden ��kar�n
            int randomIndex = Random.Range(0, _ballColors.Count);
            BallColor selectedColor = _ballColors[randomIndex];
            _ballColors.RemoveAt(randomIndex);
         
            // Yeni bir g�rev olu�turun ve listeye ekleyin
            BallPopTask task = new BallPopTask(selectedColor, ballCount);
            tasks.Add(task);

            // Se�ilen renkleri ba�ka bir listeye ekleyin
            selectedColors.Add(selectedColor);
        }

        // Se�ilen renkleri tekrar ana renkler listesine ekleyin
        _ballColors.AddRange(selectedColors);

        return tasks;
    }

    public List<BallPopTask> GetTaskList()
    {
        return _taskList;
    }

    public BallPopTask GetTask()
    {
        return _popTask;
    }


}

