using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugOverlay : MonoBehaviour
{
    [Tooltip("Interval in seconds between updates of the debug overlay.")]
    public float UpdateInterval = 0.25f;
    public int MaxOverlayItems = 12;

    private float timeSinceLastUpdate = 0f;

    public RectTransform OverlayContainer;
    public GameObject OverlayPanelPrefab;

    public static UnityEvent<string, IOverlayDebugInfo> AddToDebugOverlay = new();
    public static UnityEvent<string> RemoveFromDebugOverlay = new();

    [DisplayAsString(15)]
    public List<DebugOverlayInfo> OverlayItems = new();

    void OnAddToDebugOverlay(string title, IOverlayDebugInfo item)
    {
        if (OverlayItems.Count < MaxOverlayItems)
        {
            GameObject panel = Instantiate(OverlayPanelPrefab, OverlayContainer);
            DebugOverlayInfo infoItem = new(title, item, panel);
            OverlayItems.Add(infoItem);
        }
        else
        {
            Debug.LogWarning("DebugOverlay: Maximum number of overlay items reached. Cannot add more items.");
        }
    }

    void OnRemoveFromDebugOverlay(string title)
    {
        // Find the item to remove, and destroy its panel
        for (int i = 0; i < OverlayItems.Count; i++)
        {
            if (OverlayItems[i].Title == title)
            {
                Destroy(OverlayItems[i].PanelObject);
                OverlayItems.RemoveAt(i);
                break;
            }
        }
    }

    private void Awake()
    {
        AddToDebugOverlay.AddListener(OnAddToDebugOverlay);
        RemoveFromDebugOverlay.AddListener(OnRemoveFromDebugOverlay);
    }

    private void Update()
    {
        if (timeSinceLastUpdate >= UpdateInterval)
        {
            foreach (var item in OverlayItems)
            {
                item.Panel.UpdateInfo(item.Info.DebugOverlayInfo());
            }
            timeSinceLastUpdate = 0f;
        }

        timeSinceLastUpdate += Time.unscaledDeltaTime;
    }
}

public struct DebugOverlayInfo
{
    public string Title;
    public IOverlayDebugInfo Info;
    public GameObject PanelObject;
    public DebugOverlayPanel Panel;

    public DebugOverlayInfo(string title, IOverlayDebugInfo info, GameObject panel)
    {
        Title = title;
        Info = info;
        PanelObject = panel;
        Panel = panel.GetComponent<DebugOverlayPanel>();
        Panel.SetTitle(title);
        PanelObject.name = $"{title} - Debug Overlay";
    }
}
