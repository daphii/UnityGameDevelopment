using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CourseManager : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Players: {Players.Length}\n";
        info += $"Current Hole: {CurrentHole}/{Holes.Length}\n";
        info += $"{0}: {currentHoleStatus[0]}\n";
        return info;
    }

    public static UnityEvent<int> HoleCompleted = new();
    public static UnityEvent RoundCompleted = new();

    [Title("Players")]
    public PlayerController[] Players;

    [Title("Holes")]
    public CourseHole[] Holes;

    [Title("Balls")]
    [Required]
    public GameObject BallPool;
    [Space]
    public GameObject[] Balls;

    int currentHoleIndex = 0;
    public int CurrentHole => currentHoleIndex + 1;

    bool[] currentHoleStatus;

    List<List<GameObject>> playerBalls = new();

    private void Awake()
    {
        HoleCompleted.AddListener(OnHoleCompleted);
    }

    private void Start()
    {
        SpawnBalls();
        AssignBalls();
        StartRound();
    }

    private void SpawnBalls()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            List<GameObject> ballsForPlayer = new List<GameObject>();
            for (int j = 0; j < Balls.Length; j++)
            {
                Vector3 SpawnPosition = new(j * 2, 0.5f, i * 2);
                GameObject ball = Instantiate(Balls[j], BallPool.transform);
                ball.transform.localPosition = SpawnPosition;
                BallDataReader ballData = ball.GetComponent<BallDataReader>();
                ball.name = $"Player {i + 1} - {ballData.Name}";
                ball.SetActive(false);
                ballsForPlayer.Add(ball);
            }
            playerBalls.Add(ballsForPlayer);
        }
    }

    private void AssignBalls()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            PlayerController player = Players[i];
            List<GameObject> ballsForPlayer = playerBalls[i];
            player.SetBalls(ballsForPlayer);
        }
    }

    private void StartRound()
    {
        currentHoleIndex = 0;
        StartHole();
    }

    private void StartHole()
    {
        Debug.Log($"Starting Hole {CurrentHole}...");
        CourseHole currentHole = Holes[currentHoleIndex];
        SetHoleStatus();
        SetPlayerPosition(currentHole);
    }

    private void SetHoleStatus()
    {
        currentHoleStatus = new bool[Players.Length];
        for (int i = 0; i < currentHoleStatus.Length; i++)
        {
            currentHoleStatus[i] = false;
        }
    }

    private void SetPlayerPosition(CourseHole currentHole)
    {
        Debug.Log($"Setting player position for hole {CurrentHole}...");
        for (int i = 0; i < Players.Length; i++)
        {
            PlayerController player = Players[i];
            player.Ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            TeeBoxBallPosition spawnPosition = currentHole.TeeBox.GetInitialSpawn();
            spawnPosition.Occupy(0);
            player.Ball.transform.position = spawnPosition.Position;
            player.Ball.transform.rotation = spawnPosition.Rotation;
            player.Ball.transform.forward = spawnPosition.transform.forward;
            player.BallController.CurrentState = BallController.BallState.Tee;

            player.PlayerBallOverlay.SetCupTarget(currentHole.cupController.gameObject);
            player.PlayerBallOverlay.transform.forward = spawnPosition.transform.forward;
            player.Ball.SetActive(true);
        }

    }

    private void OnHoleCompleted(int playerID)
    {
        currentHoleStatus[playerID] = true;
        Debug.Log($"Player {playerID} completed hole {CurrentHole}!");

        // Check if all players have completed the hole
        bool allCompleted = true;
        foreach (bool status in currentHoleStatus)
        {
            if (!status)
            {
                allCompleted = false;
                break;
            }
        }

        if (allCompleted)
        {
            currentHoleIndex++;
            if (currentHoleIndex < Holes.Length)
            {
                Debug.Log("All players completed the hole!");
                StartCoroutine(MoveToNextHole());
            }
            else
            {
                RoundCompleted.Invoke();
                Debug.Log("Round Completed!");

                // loop back to first hole for now
                StartCoroutine(ResetCourse());
            }
        }

    }

    IEnumerator MoveToNextHole()
    {
        Debug.Log("Preparing to move to the next hole...");
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before moving to the next hole
        Debug.Log("Starting Next Hole...");
        StartHole();
    }

    IEnumerator ResetCourse()
    {
        Debug.Log("Preparing to reset the course...");
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before resetting the course
        Debug.Log("Resetting Course...");
        StartRound();
    }
}
