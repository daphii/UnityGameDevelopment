using Sirenix.OdinInspector;
using UnityEngine;

public class CourseHole : MonoBehaviour
{
    [Tooltip("The par for this hole.")]
    public int Par = 3;

    [Space]
    [SerializeField, Required]
    public TeeBoxController TeeBox;
    [SerializeField, Required]
    public CupController cupController;

}
