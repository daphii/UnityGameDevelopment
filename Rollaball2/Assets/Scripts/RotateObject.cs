using Sirenix.OdinInspector;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Title("Rotation Settings")]
    public Vector3 RotationSpeed = new(0f, 0f, 0f);

    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
