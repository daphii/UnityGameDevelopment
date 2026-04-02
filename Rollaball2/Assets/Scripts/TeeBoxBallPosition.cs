using Sirenix.OdinInspector;
using UnityEngine;

public class TeeBoxBallPosition : DebugMonoBehaviour
{
    public override string DebugOverlayInfo()
    {
        string info = "";
        info += $"Index: {index}\n";
        info += $"Occupied: {isOccupied}\n";
        info += $"Ball ID: {(isOccupied ? ballID.ToString() : "N/A")}\n";
        return info;
    }

    [Title("Settings")]
    public int index;
    public Vector3 Position => transform.position + Vector3.up * 0.51f;
    public Quaternion Rotation => transform.rotation;
    public bool isOccupied => ballID != -1;
    [Space, DisplayAsString(15)]
    public int ballID = -1;

    public void Occupy(int ballID)
    {
        this.ballID = ballID;
    }

    public void Vacate()
    {
        this.ballID = -1;
    }
}
