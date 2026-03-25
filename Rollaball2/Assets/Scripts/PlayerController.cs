using Rewired;
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
        return info;
    }

    Vector2 movementInput = Vector2.zero;
    bool jump = false;
    bool interact = false;

    public CameraController cameraController;

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

    void GetInput()
    {
        float horizontal = player.GetAxis("MoveHorizontal");
        float vertical = player.GetAxis("MoveVertical");
        movementInput = new Vector2(horizontal, vertical);
    }




}
