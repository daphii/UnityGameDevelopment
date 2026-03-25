using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class FollowCamVelocityTilt : MonoBehaviour
{
    [InfoBox("This will overide the Cinemachine FollowCam Follow Offset Z with a velocity-based tilt.")]
    public bool Activate = true;

    [Tooltip("The velocity at which the camera starts tilting down.")]
    public float StartVelocity = 10f;
    float StartTiltAngle;

    [Tooltip("The velocity at which the camera stops tilting down.")]
    public float EndVelocity = 0.5f;
    [Tooltip("The maximum tilt angle in degrees when the ball is reached the End Velocity.")]
    public float EndTiltAngle;

    [Space]
    [Tooltip("The time it takes to return to the original position after the velocity drops below the End Velocity.")]
    public float ReturnTime = 0.5f;

    CinemachineFollow followCam;

    Rigidbody followRB;

    private void Awake()
    {
        followCam = GetComponent<CinemachineFollow>();
        followRB = GetComponent<CinemachineCamera>().Target.TrackingTarget.gameObject.GetComponent<BallOverlayController>().CurrentBallObject.GetComponent<Rigidbody>();
        StartTiltAngle = followCam.FollowOffset.z;
    }

    private void Update()
    {
        if (!Activate) return;
        float velocity = followRB.linearVelocity.magnitude;
        if (velocity < StartVelocity && velocity > EndVelocity)
        {
            float t = Mathf.InverseLerp(StartVelocity, EndVelocity, velocity);
            float newOffset = Mathf.Lerp(StartTiltAngle, EndTiltAngle, t);
            followCam.FollowOffset.z = newOffset;
            Debug.Log($"Setting offset to {newOffset}");
        }
        else
        {
            followCam.FollowOffset.z = Mathf.Lerp(followCam.FollowOffset.z, StartTiltAngle, Time.deltaTime / ReturnTime);
        }
    }
}
