using Rewired;
using UnityEngine;

public class BallOverlayController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Current Ball: {CurrentBallObject.name}\n";
        info += $"Ball Position: {ballTransform.position}\n";
        info += $"Cup Target: {CurrentCupTarget.name}\n";
        info += $"Cup Position: {cupTransform.position}\n";
        info += $"Overlay Active: {OverlayActive}\n";
        return info;
    }

    public GameObject CurrentBallObject;
    public GameObject CurrentCupTarget;

    [Space]
    public float LookSpeed = 5f;

    [Space]
    public GameObject AimIndicator;

    Transform ballTransform;
    BallController ballController;

    Transform cupTransform;

    Player player;

    public bool OverlayActive = false;
    public Vector3 AimDirection => transform.forward;

    Vector2 inputDirection = Vector2.zero;


    private void Awake()
    {
        SetBall(CurrentBallObject);
        SetCupTarget(CurrentCupTarget);
        player = ReInput.players.GetPlayer(0);
    }

    private void Start()
    {
        ActivateOverlay();
    }

    private void Update()
    {
        GetInput();

        if (ballController.IsMoving)
        {
            if (OverlayActive)
            {
                HideOverlay();
            }

        }
        else
        {
            if (!OverlayActive)
            {
                ActivateOverlay();
            }
        }


    }

    private void FixedUpdate()
    {
        transform.position = ballTransform.position;
        ProcessInput();
    }

    void GetInput()
    {
        inputDirection.x = player.GetAxis("MoveHorizontal");
        //inputDirection.y = player.GetAxis("MoveVertical");
    }

    void ProcessInput()
    {
        if (OverlayActive && inputDirection.x != 0)
        {
            transform.Rotate(Vector3.up, inputDirection.x * LookSpeed * Time.deltaTime);
        }
    }

    public void SetBall(GameObject newBall)
    {
        CurrentBallObject = newBall;
        if (CurrentBallObject != null)
        {
            ballTransform = CurrentBallObject.transform;
            ballController = CurrentBallObject.GetComponent<BallController>();
        }
        else
        {
            Debug.LogError("CurrentBallObject reference is not set in BallOverlayController.");
        }
    }

    public void SetCupTarget(GameObject newCupTarget)
    {
        CurrentCupTarget = newCupTarget;
        if (CurrentCupTarget != null)
        {
            cupTransform = CurrentCupTarget.transform;
        }
        else
        {
            Debug.LogError("CurrentCupTarget reference is not set in BallOverlayController.");
        }
    }


    public void ActivateOverlay()
    {
        // Set forward to match the cup target's direction
        if (cupTransform != null)
        {
            // look at the cup target, but ignore the y axis to keep the overlay flat
            Vector3 direction = cupTransform.position - ballTransform.position;
            direction.y = 0;
            transform.forward = direction.normalized;
            AimIndicator.SetActive(true);
            OverlayActive = true;
        }
        else
        {
            Debug.LogError("Cup Transform is not set.");
        }
    }

    public void HideOverlay()
    {
        OverlayActive = false;
        AimIndicator.SetActive(false);
    }

}