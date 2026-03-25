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

    Rigidbody targetRB;
    Transform targetTransform;

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
        targetRB = followTarget.GetComponent<Rigidbody>();
        targetTransform = followTarget.transform;

    }

    private void FixedUpdate()
    {
        transform.position = targetTransform.position;
        // update forward to match the targets movment direction
        Vector3 targetVelocity = targetRB.linearVelocity;
        if (targetVelocity.magnitude > 0.1f)
        {
            transform.forward = targetVelocity.normalized;
        }
    }

}
