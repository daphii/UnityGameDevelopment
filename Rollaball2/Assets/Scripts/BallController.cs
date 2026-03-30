using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

public class BallController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"<b>{ballData.Name}</b>\n";
        info += $"Max Shot Power: {ballData.Power}\n";
        info += $"Direction: {transform.forward}\n";
        info += $"Velocity: {rb.linearVelocity.magnitude:F2}\n";
        info += $"Velocity Vector: {rb.linearVelocity}\n";
        info += $"AVelocity Magnitude: {rb.angularVelocity.magnitude:F2}\n";
        info += $"AVelocity: {rb.angularVelocity}\n";
        return info;
    }

    [Title("Ball Settings")]
    public float OverloadPowerModifier = 1.25f;
    public float ChargeTime = 2f;
    float chargeAmmount = 0f;
    public AnimationCurve ShotPowerCurve;

    [Title("Breaking Settings")]
    public bool VelocityBrakesEnabled = true;
    public float BrakingThreshold = 1.5f;
    public float BrakingRate = 0.99f;
    [Space]
    public bool TorqueBrakesEnabled = true;
    public float TorqueBrakingVelocityThreshold = 5f;
    public float TorqueBrakingAngularThreshold = 20f;
    public float TorqueBrakingRate = 0.99f;

    public float ChargePercent => chargeAmmount / ChargeTime;

    bool isCharging = false;
    bool Overcharged = false;

    Vector2 inputDirection = Vector2.zero;


    public Transform BallOverlay;

    Rigidbody rb;
    Player player;
    BallDataReader ballData;

    bool shotPressed;

    public bool IsMoving => rb.linearVelocity.magnitude > 0.1f;
    public Vector3 MovementDirection => rb.linearVelocity.normalized;
    public float Speed => rb.linearVelocity.magnitude;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = ReInput.players.GetPlayer(0);
        ballData = GetComponent<BallDataReader>();
    }

    private void Update()
    {
        if (VelocityBrakesEnabled && rb.linearVelocity.magnitude < BrakingThreshold && rb.linearVelocity.magnitude > 0)
        {
            Brakes();
        }

        if (TorqueBrakesEnabled && (rb.linearVelocity.magnitude < TorqueBrakingVelocityThreshold || rb.angularVelocity.magnitude > TorqueBrakingAngularThreshold))
        {
            TorqueBreaks();
        }

        GetInput();
    }

    private void FixedUpdate()
    {
        ProcessInput();
    }

    void GetInput()
    {
        inputDirection.x = player.GetAxis("MoveHorizontal");
        inputDirection.y = player.GetAxis("MoveVertical");

        if (player.GetButtonDown("Interact"))
        {
            StartCharging();
        }
        if (player.GetButton("Interact"))
        {
            AddCharge();
        }
        if (player.GetButtonUp("Interact"))
        {
            ShootBall();
        }
    }

    void StartCharging()
    {
        chargeAmmount = 0f;
        Overcharged = false;
    }

    void AddCharge()
    {
        chargeAmmount += Time.deltaTime;
        if (chargeAmmount > ChargeTime)
        {
            chargeAmmount = ChargeTime;
            Overcharged = true;
        }
    }

    void ProcessInput()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float speedThreshold = currentSpeed > 10 ? 1 : currentSpeed / 10;

        float influenceAmmount = ballData.Influence * speedThreshold;

        if (currentSpeed > 0.1f && inputDirection.magnitude > 0.1f)
        {
            // Change this to make the movement relative to the ball indicator's forward direction
            Vector3 moveDirection = BallOverlay.forward * inputDirection.y + BallOverlay.right * inputDirection.x;
            //Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
            rb.AddForce(moveDirection * influenceAmmount);
        }
    }
    void ShootBall()
    {

        Vector3 shotDirection = BallOverlay.forward;
        float shotPower = ballData.Power * ShotPowerCurve.Evaluate(ChargePercent);
        rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        chargeAmmount = 0f;
        Overcharged = false;
        Debug.Log($"Shooting ball with power: {shotPower}");
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

}
