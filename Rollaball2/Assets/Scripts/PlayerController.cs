using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Ball: {(Ball != null ? Ball.name : "No Ball")}\n";
        return info;
    }
    [Title("Player Info")]
    public int PlayerID = -1;
    public BallOverlayController PlayerBallOverlay;

    [Title("Ball Info")]
    [Tooltip("The ball that the player is currently playing with.")]
    [DisplayAsString(15)]
    public GameObject Ball;
    [DisplayAsString(15)]
    public BallController BallController;
    [DisplayAsString(15)]
    public Rigidbody BallRigidbody;

    int currentBallIndex = 0;
    List<GameObject> playerBalls;

    public void SetBalls(List<GameObject> balls)
    {
        playerBalls = balls;
        if (playerBalls != null && balls.Count > 0)
        {
            currentBallIndex = 0;
            SetBall(currentBallIndex);
        }
        else
        {
            Debug.LogWarning($"No balls assigned to player {PlayerID}.");
        }
    }

    void SetBall(int index)
    {
        Ball = playerBalls[index];
        BallController = Ball.GetComponent<BallController>();
        BallRigidbody = Ball.GetComponent<Rigidbody>();
        PlayerBallOverlay.SetBall(Ball);
        BallController.BallOverlay = PlayerBallOverlay.transform;
        BallController.PlayerID = PlayerID;
        BallController.BallActive = true;
    }

    public GameObject GetNextBall()
    {
        if (playerBalls != null && playerBalls.Count > 0)
        {
            currentBallIndex = (currentBallIndex + 1) % playerBalls.Count;
            SetBall(currentBallIndex);
            return Ball;
        }
        else
        {
            Debug.LogWarning($"No balls assigned to player {PlayerID}.");
            return null;
        }
    }

    public GameObject GetPreviousBall()
    {
        if (playerBalls != null && playerBalls.Count > 0)
        {
            currentBallIndex = (currentBallIndex - 1 + playerBalls.Count) % playerBalls.Count;
            SetBall(currentBallIndex);
            return Ball;
        }
        else
        {
            Debug.LogWarning($"No balls assigned to player {PlayerID}.");
            return null;
        }
    }
}
