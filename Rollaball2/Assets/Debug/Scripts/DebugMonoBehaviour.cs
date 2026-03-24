using Sirenix.OdinInspector;
using UnityEngine;

public abstract class DebugMonoBehaviour : MonoBehaviour, IOverlayDebugInfo
{
    [Title("Debug")]
    [ShowIf("@!UnityEngine.Application.isPlaying")]
    public bool ShowDebugOverlay = false;
    private bool debugInfoRegistered = false;

    public string Title => $"{gameObject.name}/{GetType().Name}";

    [Title("Debug")]
    [ShowIf("@UnityEngine.Application.isPlaying")]
    [Button("Add to Debug Overlay")]
    public void AddToDebugOverlay()
    {
        if (!debugInfoRegistered)
        {
            DebugOverlay.AddToDebugOverlay.Invoke(Title, this);
            debugInfoRegistered = true;
            Debug.Log($"Added {Title} to Debug Overlay.");
        }
    }

    [ShowIf("@UnityEngine.Application.isPlaying")]
    [Button("Remove from Debug Overlay")]
    public void RemoveFromDebugOverlay()
    {
        if (debugInfoRegistered)
        {
            DebugOverlay.RemoveFromDebugOverlay.Invoke(Title);
            debugInfoRegistered = false;
            Debug.Log($"Removed {Title} from Debug Overlay.");
        }
    }

    public abstract string DebugOverlayInfo();

    private void Start()
    {
        //Debug.Log($"Start called for DebugMonoBehaviour. ShowDebugOverlay: {ShowDebugOverlay}");
        if (ShowDebugOverlay)
        {
            //Debug.Log($"ShowDebugOverlay is enabled for {Title}. Adding to Debug Overlay.");
            AddToDebugOverlay();
        }
        else
        {
            //Debug.Log($"ShowDebugOverlay is disabled for {Title}. Not adding to Debug Overlay.");
        }
    }
}