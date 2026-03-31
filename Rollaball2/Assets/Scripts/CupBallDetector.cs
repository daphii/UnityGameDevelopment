using System.Collections.Generic;
using UnityEngine;

public class CupBallDetector : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Balls in Cup: {ballsInCup.Count}\n";
        return info;
    }

    private CupController cupController;

    List<(GameObject ball, Rigidbody rb)> ballsInCup = new();

    private void Awake()
    {
        cupController = GetComponentInParent<CupController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballsInCup.Add((other.gameObject, other.attachedRigidbody));
            GameManager.ResetGame.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballsInCup.RemoveAll(x => x.ball == other.gameObject);
        }
    }

    private void Update()
    {
        if (ballsInCup.Count > 0)
        {
            foreach (var (ball, rb) in ballsInCup)
            {
                StopBall(rb);
            }
        }
    }

    void StopBall(Rigidbody rb)
    {
        rb.linearVelocity *= .98f;
    }

}
