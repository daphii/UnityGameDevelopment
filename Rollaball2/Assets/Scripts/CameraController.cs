using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Follow Target: {(followTarget != null ? followTarget.name : "None")}\n";
        info += $"Camera Position: {cameraPosition}\n";
        info += $"Camera Rotation: {cameraRotation}\n";
        return info;
    }

    [Title("Follow Settings")]
    public GameObject followTarget;
    public GameObject followCamera;

    [Space]
    public float maxRotationSpeed = 5f;
    public float maxCameraZoom = 8f;
    public float currentZoom = 0f;

    Vector3 cameraPosition;
    Vector3 cameraRotation;

    Vector2 cameraHorizontal;
    Vector2 cameraVertical;

    Player player;

    public Vector3 CameraFacingDirection => transform.forward;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        GetInput();
        RotateAroundFocus();
    }

    void GetInput()
    {
        cameraHorizontal.x = player.GetAxis("CameraHorizontal");
    }

    private void FixedUpdate()
    {
        transform.position = followTarget.transform.position;
    }


    void RotateAroundFocus()
    {
        // Rotate Camera focus point based on x input
        Vector3 focusRotation = transform.localEulerAngles;
        focusRotation.y += cameraHorizontal.x * maxRotationSpeed * Time.deltaTime;
        transform.localEulerAngles = focusRotation;
    }

}
