using Sirenix.OdinInspector;
using UnityEngine;

public class BallController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"<b>{ballData.Name}</b>\n";
        info += $"Power: {ballData.Power}\n";
        info += $"State: {CurrentState}\n";
        info += $"Velocity: {rb.linearVelocity.magnitude:F2}\n";
        info += $"AVelocity Magnitude: {rb.angularVelocity.magnitude:F2}\n";
        return info;
    }


    [Title("Ball Identifier")]
    public int PlayerID = -1;

    [Title("Breaking Settings")]
    public bool VelocityBrakesEnabled = true;
    float BrakingThreshold;
    public float BrakingRate = 0.99f;
    [Space]
    public bool TorqueBrakesEnabled = true;
    float TorqueBrakingVelocityThreshold;
    float TorqueBrakingAngularThreshold;
    public float TorqueBrakingRate = 0.99f;


    public bool BallActive;

    public Transform BallOverlay;

    Rigidbody rb;
    BallDataReader ballData;

    float stateCooldown = 0f;
    const float stateCooldownDuration = 0.1f;
    bool StateCooldownActive => stateCooldown > 0f;


    public bool IsMoving => rb.linearVelocity.magnitude > 0.1f;
    public Vector3 MovementDirection => rb.linearVelocity.normalized;
    public float Speed => rb.linearVelocity.magnitude;

    public enum BallState
    {
        Tee,
        Rolling,
        Stopped,
        Cup
    }

    private BallState currentState = BallState.Tee;

    public BallState CurrentState
    {
        get => currentState;
        set
        {
            if (!StateCooldownActive && currentState != value)
            {
                switch (currentState)
                {
                    case BallState.Tee:
                        // Handle exiting Tee state
                        Debug.Log($"{ballData.Name}: Exiting Tee state");
                        break;
                    case BallState.Rolling:
                        // Handle exiting Rolling state
                        Debug.Log($"{ballData.Name}: Exiting Rolling state");
                        break;
                    case BallState.Stopped:
                        // Handle exiting Stopped state
                        Debug.Log($"{ballData.Name}: Exiting Stopped state");
                        break;
                    case BallState.Cup:
                        // Handle exiting Cup state
                        Debug.Log($"{ballData.Name}: Exiting Cup state");
                        break;
                }

                currentState = value;
                stateCooldown = stateCooldownDuration;

                switch (currentState)
                {
                    case BallState.Tee:
                        // Handle entering Tee state
                        Debug.Log($"{ballData.Name}: Entering Tee state");
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        break;
                    case BallState.Rolling:
                        // Handle entering Rolling state
                        Debug.Log($"{ballData.Name}: Entering Rolling state");
                        break;
                    case BallState.Stopped:
                        // Handle entering Stopped state
                        Debug.Log($"{ballData.Name}: Entering Stopped state");
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        break;
                    case BallState.Cup:
                        // Handle entering Cup state
                        Debug.Log($"{ballData.Name}: Entering Cup state");
                        break;
                }
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballData = GetComponent<BallDataReader>();
        BrakingThreshold = ballData.Influence * 2;
        TorqueBrakingAngularThreshold = ballData.Influence * 2;
        TorqueBrakingVelocityThreshold = ballData.Influence * 2;
    }

    private void FixedUpdate()
    {
        if (stateCooldown > 0f)
        {
            stateCooldown -= Time.fixedDeltaTime;
            if (stateCooldown < 0f)
            {
                stateCooldown = 0f;
            }
        }

        switch (CurrentState)
        {
            case BallState.Rolling:
                if (VelocityBrakesEnabled && rb.linearVelocity.magnitude < BrakingThreshold && rb.linearVelocity.magnitude > 0)
                {
                    Brakes();
                }

                if (TorqueBrakesEnabled && (rb.linearVelocity.magnitude < TorqueBrakingVelocityThreshold || rb.angularVelocity.magnitude > TorqueBrakingAngularThreshold))
                {
                    TorqueBreaks();
                }

                if (rb.linearVelocity.magnitude < 0.1f)
                {
                    CurrentState = BallState.Stopped;
                }
                break;

            case BallState.Stopped:
                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    CurrentState = BallState.Rolling;
                }
                break;
        }
    }

    void Brakes()
    {
        rb.linearVelocity *= BrakingRate;
        if (rb.linearVelocity.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void TorqueBreaks()
    {
        rb.angularVelocity *= TorqueBrakingRate;
        if (rb.angularVelocity.magnitude < 0.05f)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cup"))
        {
            CurrentState = BallState.Cup;
        }
    }
}
