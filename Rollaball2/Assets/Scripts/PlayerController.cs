using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"PlayerController: {gameObject.name}";
        info += $"\nPosition: {transform.position}";
        info += $"\nMovement Input: {movementInput}";
        info += $"\nJump: {jump}";
        info += $"\nInteract: {interact}";
        info += $"\nLeft Spin: {leftSpin}";
        info += $"\nRight Spin: {rightSpin}";
        return info;
    }

    [Title("Movement Options")]
    float maxSpeed = 5f;

    [Space]
    float jumpForce = 5f;

    Vector2 movementInput = Vector2.zero;
    bool jump = false;
    bool interact = false;
    float leftSpin = 0f;
    float rightSpin = 0f;

    public CameraController cameraController;
    Vector3 CameraFacing => cameraController.CameraFacingDirection;

    Rigidbody rb;


    Player player;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        ProcessUpdate();
    }

    void GetInput()
    {
        float horizontal = player.GetAxis("MoveHorizontal");
        float vertical = player.GetAxis("MoveVertical");
        movementInput = new Vector2(horizontal, vertical);

        leftSpin = player.GetAxis("LeftSpin");
        rightSpin = player.GetAxis("RightSpin");
    }

    void ProcessUpdate()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        // Calculate movement direction based on camera facing and input
        Vector3 forward = Vector3.ProjectOnPlane(CameraFacing, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        Vector3 movementDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        rb.AddForce(movementDirection * maxSpeed);


        /*Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        rb.AddForce(movementDirection * maxSpeed);*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EndText.LoseCondition.Invoke();
        }
    }
}
