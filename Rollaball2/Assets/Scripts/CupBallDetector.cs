using UnityEngine;

public class CupBallDetector : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Ball in Cup: {BallInCup}\n";
        return info;
    }

    bool BallInCup = false;

    private CupController cupController;


    private void Awake()
    {
        cupController = GetComponentInParent<CupController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BallInCup = true;
            Debug.Log("Ball Detected in Cup, Invoking Hole Completed Event");
            BallController ballController = other.GetComponent<BallController>();
            CourseManager.HoleCompleted.Invoke(ballController.PlayerID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BallInCup = false;
        }
    }

}
