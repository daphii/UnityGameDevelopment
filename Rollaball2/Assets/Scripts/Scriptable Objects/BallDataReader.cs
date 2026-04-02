using Sirenix.OdinInspector;
using UnityEngine;

public class BallDataReader : MonoBehaviour
{
    [Title("Ball Data")]
    [SerializeField, InlineEditor, Required]
    BallSO ballData;

    public string Name => ballData.Name;
    public int Power => ballData.ActualMaxPower;
    public float Influence => ballData.ActualInfluence;
    public float Bounciness => ballData.ActualBounciness;

    public int PowerRating => ballData.PowerRating;
    public int InfluenceRating => ballData.InfluenceRating;
    public int BouncinessRating => ballData.BouncinessRating;

    private void Awake()
    {
        SetPhysicsMaterial();
    }

    void SetPhysicsMaterial()
    {
        if (ballData.PhysicsMaterial != null)
        {
            if (TryGetComponent<Collider>(out var collider))
            {
                collider.material = ballData.PhysicsMaterial;
            }
            else
            {
                Debug.LogWarning($"No collider found on {gameObject.name} to set the physics material.");
            }
        }
    }
}
