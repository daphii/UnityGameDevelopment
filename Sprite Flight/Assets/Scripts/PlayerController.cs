using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float thrustPower = 0.2f;
    public float maxSpeed = 5f;

    public GameObject thrustVisual;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            thrustVisual.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            thrustVisual.SetActive(false);
        }


        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            Vector2 direction = (worldPosition - transform.position);
            transform.up = direction;

            rb.AddForce(direction.normalized * thrustPower);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.LoseCondition.Invoke();
        }
    }
}
