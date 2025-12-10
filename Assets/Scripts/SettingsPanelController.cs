using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainButtonsPanel;
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        mainButtonsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
    }
}
