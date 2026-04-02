using Sirenix.OdinInspector;

public class TeeBoxController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"0: {(ballSpawns[0].isOccupied ? $"Ball {ballSpawns[0].ballID}" : "No Ball")}\n";
        info += $"1: {(ballSpawns[1].isOccupied ? $"Ball {ballSpawns[1].ballID}" : "No Ball")}\n";
        info += $"2: {(ballSpawns[2].isOccupied ? $"Ball {ballSpawns[2].ballID}" : "No Ball")}\n";
        info += $"3: {(ballSpawns[3].isOccupied ? $"Ball {ballSpawns[3].ballID}" : "No Ball")}\n";
        info += $"4: {(ballSpawns[4].isOccupied ? $"Ball {ballSpawns[4].ballID}" : "No Ball")}\n";
        return info;
    }

    [Title("Ball Spawn Positions")]
    public TeeBoxBallPosition[] ballSpawns;

    public TeeBoxBallPosition GetInitialSpawn()
    {
        return ballSpawns[2];
    }

    public TeeBoxBallPosition GetNextSpawn(int ballID)
    {
        int currentIndex = -1;
        for (int i = 0; i < ballSpawns.Length; i++)
        {
            if (ballSpawns[i].ballID == ballID)
            {
                currentIndex = i;
                break;
            }
        }

        int nextIndex = (currentIndex + 1) % ballSpawns.Length;

        return ballSpawns[nextIndex];
    }

    public TeeBoxBallPosition GetPreviousSpawn(int ballID)
    {
        int currentIndex = -1;
        for (int i = 0; i < ballSpawns.Length; i++)
        {
            if (ballSpawns[i].ballID == ballID)
            {
                currentIndex = i;
                break;
            }
        }
        int previousIndex = (currentIndex - 1 + ballSpawns.Length) % ballSpawns.Length;

        return ballSpawns[previousIndex];
    }
}
