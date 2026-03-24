using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Target: {(Target != null ? Target.name : "None")}\n";
        return info;
    }

    [Title("Target Settings")]
    public Transform Target;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Target != null)
        {
            agent.SetDestination(Target.position);
        }
    }


}
