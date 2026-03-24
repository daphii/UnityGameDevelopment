using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Vector2 SizeLimit = new(1, 3);
    float MinSize => SizeLimit.x;
    float MaxSize => SizeLimit.y;

    public Vector2 SpeedLimits = new(1, 2);
    float MinSpeed => SpeedLimits.x;
    float MaxSpeed => SpeedLimits.y;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetRandomSize();
        SetRandomMovement();
    }

    void SetRandomSize()
    {
        var randomSize = Random.Range(MinSize, MaxSize);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        rb.mass = randomSize;
    }

    void SetRandomMovement()
    {
        var randomDirection = Random.insideUnitCircle.normalized;
        var randomSpeed = Random.Range(MinSpeed, MaxSpeed);
        var movement = randomDirection * randomSpeed;
        rb.linearVelocity = movement;
        rb.AddTorque(randomSpeed * 0.1f);
    }
}
