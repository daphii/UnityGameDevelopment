using TMPro;
using UnityEngine;

public class DebugOverlayPanel : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InfoText;

    public void SetTitle(string title)
    {
        TitleText.text = title;
    }

    public void UpdateInfo(string info)
    {
        InfoText.text = info;
    }
}
