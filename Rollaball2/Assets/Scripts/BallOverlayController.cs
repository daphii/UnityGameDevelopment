using Rewired;
using Sirenix.OdinInspector;
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
    public float LookSpeed = 50f;
    public float RotateFollowThreshold = 5f;

    [Space]
    public GameObject AimIndicator;
    public bool AimAssistEnabled = true;
    public bool AimIndicatorEnabled = true;

    Transform ballTransform;
    BallController ballController;

    Transform cupTransform;

    Player player;

    [DisplayAsString(15)]
    public bool OverlayActive = false;
    public Vector3 AimDirection => transform.forward;

    Vector2 inputDirection = Vector2.zero;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void Start()
    {
        ActivateOverlay();
    }

    private void Update()
    {
        GetInput();

        if (ballController.CurrentState == BallController.BallState.Rolling)
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
        if (ballController.CurrentState != BallController.BallState.Cup)
        {
            FollowBall();
        }
        ProcessInput();
    }

    void GetInput()
    {
        if (!player.GetButton("Interact"))
        {
            inputDirection.x = player.GetAxis("MoveHorizontal");
        }
        //inputDirection.y = player.GetAxis("MoveVertical");
    }

    void ProcessInput()
    {
        if (OverlayActive && inputDirection.x != 0)
        {
            transform.Rotate(Vector3.up, inputDirection.x * LookSpeed * Time.deltaTime);
        }
    }

    void FollowBall()
    {
        transform.position = ballTransform.position;
        if (ballController.Speed > RotateFollowThreshold)
        {
            Vector3 direction = ballController.MovementDirection;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.forward = direction;
            }
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
            if (AimAssistEnabled)
            {
                Vector3 direction = cupTransform.position - ballTransform.position;
                direction.y = 0;
                transform.forward = direction.normalized;
            }
            if (AimIndicatorEnabled)
            {
                AimIndicator.SetActive(true);
            }
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