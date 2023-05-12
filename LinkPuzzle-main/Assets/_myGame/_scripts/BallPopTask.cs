public class BallPopTask
{
    public BallColor Color { get; set; }
    public int Count { get; set; }
    public bool GeneralBall { get;}
    public TaskStatus Status { get; set; } = TaskStatus.NotSuccess;

    public BallPopTask(BallColor color, int count)
    {
        Color = color;
        Count = count;
        GeneralBall = false;
    }

    public BallPopTask(int count)
    {
        Count = count;
        GeneralBall = true;
    }
}


