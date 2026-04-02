using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShotController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Charge Percent: {ChargePercent:P0}\n";
        info += $"Input Direction: {inputDirection}\n";
        return info;
    }

    [Title("Shot Settings")]
    public float OverloadPowerModifier = 1.25f;
    public float ChargeTime = 2f;
    float chargeAmmount = 0f;
    public AnimationCurve ShotPowerCurve;

    bool isPressingCharge = false;
    bool Overcharged = false;
    public float ChargePercent => chargeAmmount / ChargeTime;

    Vector2 inputDirection = Vector2.zero;

    PlayerController playerController;
    Player player;

    GameObject Ball => playerController.Ball;
    BallController BallController => playerController.BallController;
    Rigidbody BallRB => playerController.BallRigidbody;
    BallDataReader BallData => Ball.GetComponent<BallDataReader>();
    Transform BallOverlay => playerController.PlayerBallOverlay.gameObject.transform;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        player = ReInput.players.GetPlayer(playerController.PlayerID);
    }

    private void Update()
    {
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
        if (player.GetButtonDown("SelectDown"))
        {
            UseNextBall();
        }
        if (player.GetButtonDown("SelectUp"))
        {
            UsePreviousBall();
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
        float currentSpeed = BallRB.linearVelocity.magnitude;
        float speedThreshold = currentSpeed > 10 ? 1 : currentSpeed / 10;

        float influenceAmmount = BallData.Influence * speedThreshold;

        if (currentSpeed > 0.1f && inputDirection.magnitude > 0.1f)
        {
            // Change this to make the movement relative to the ball indicator's forward direction
            Vector3 moveDirection = BallOverlay.forward * inputDirection.y + BallOverlay.right * inputDirection.x;
            //Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
            BallRB.AddForce(moveDirection * influenceAmmount);
        }
    }
    void ShootBall()
    {

        Vector3 shotDirection = BallOverlay.forward;
        float shotPower = BallData.Power * ShotPowerCurve.Evaluate(ChargePercent);
        BallRB.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        BallController.CurrentState = BallController.BallState.Rolling;
        chargeAmmount = 0f;
        Overcharged = false;
        Debug.Log($"Shooting ball with power: {shotPower}");
    }

    void UseNextBall()
    {
        if (BallController.CurrentState == BallController.BallState.Tee || BallController.CurrentState == BallController.BallState.Stopped)
        {
            Vector3 position = Ball.transform.position;
            Quaternion rotation = Ball.transform.rotation;
            Ball.SetActive(false);
            Ball.transform.localPosition = Vector3.zero;

            GameObject nextBall = playerController.GetNextBall();
            nextBall.transform.SetPositionAndRotation(position, rotation);
            nextBall.SetActive(true);
            UIBallSelectIndicator.NewBallSelected.Invoke(nextBall);
        }
    }

    void UsePreviousBall()
    {
        if (BallController.CurrentState == BallController.BallState.Tee || BallController.CurrentState == BallController.BallState.Stopped)
        {
            Vector3 position = Ball.transform.position;
            Quaternion rotation = Ball.transform.rotation;
            Ball.SetActive(false);
            Ball.transform.localPosition = Vector3.zero;

            GameObject previousBall = playerController.GetPreviousBall();
            previousBall.transform.SetPositionAndRotation(position, rotation);
            previousBall.SetActive(true);
            UIBallSelectIndicator.NewBallSelected.Invoke(previousBall);
        }
    }
}
