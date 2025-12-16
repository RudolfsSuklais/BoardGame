using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    const string PREF_RESOLUTION = "RESOLUTION_INDEX";
    const string PREF_FULLSCREEN = "FULLSCREEN";

    void OnEnable()
    {
        SetupResolutionDropdown();
        SetupFullscreenToggle();
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var r = resolutions[i];
            options.Add($"{r.width} x {r.height} @{r.refreshRate}Hz");

            if (r.width == Screen.currentResolution.width &&
                r.height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt(PREF_RESOLUTION, currentIndex);
        resolutionDropdown.SetValueWithoutNotify(savedIndex);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetupFullscreenToggle()
    {
        bool isFullscreen = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;

        fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);

        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        Screen.fullScreen = isFullscreen;
    }

    void SetResolution(int index)
    {
        var r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen, r.refreshRate);

        PlayerPrefs.SetInt(PREF_RESOLUTION, index);
        PlayerPrefs.Save();
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt(PREF_FULLSCREEN, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
