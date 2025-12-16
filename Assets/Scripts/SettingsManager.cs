using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource gameMusic;
    public AudioSource sfxSource;

    [Header("UI")]
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxSlider;
    public TMP_Dropdown resolutionDropdown;
    public UnityEngine.UI.Toggle cameraShakeToggle;

    Resolution[] resolutions;

    void Start()
    {
        // === AUDIO ===
        musicSlider.value = gameMusic.volume;
        sfxSlider.value = sfxSource.volume;

        // === RESOLUTIONS ===
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string label = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(label);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // === AUDIO ===
    public void SetMusicVolume(float value)
    {
        gameMusic.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    // === RESOLUTION ===
    public void SetResolution(int index)
    {
        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    // === RANDOM SETTING ===
    public void ToggleCameraShake(bool enabled)
    {
        PlayerPrefs.SetInt("CameraShake", enabled ? 1 : 0);
    }
}
