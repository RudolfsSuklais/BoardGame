using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI")]
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Sources")]


    private Resolution[] resolutions;

    void Start()
    {
       

        // =======================
        // RESOLUTIONS
        // =======================
        resolutions = Screen.resolutions
            .Where(r => r.refreshRate == Screen.currentResolution.refreshRate)
            .ToArray();

        resolutionDropdown.ClearOptions();
        var options = resolutions.Select(r => r.width + " x " + r.height).ToList();
        resolutionDropdown.AddOptions(options);

        int currentIndex = System.Array.FindIndex(resolutions, r =>
            r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height);

        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        // =======================
        // FULLSCREEN
        // =======================
        fullscreenToggle.isOn = Screen.fullScreen;

        // =======================
        // LOAD AUDIO SETTINGS
        // =======================
        float musVol = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.7f);

        // Make sure sliders are correctly set up
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        musicSlider.wholeNumbers = false;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.wholeNumbers = false;

        // Apply saved values
        musicSlider.value = musVol;
        sfxSlider.value = sfxVol;

       
    }

    // ==========================
    // RESOLUTION
    // ==========================
    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // ==========================
    // FULLSCREEN
    // ==========================
    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    // ==========================
    // MUSIC VOLUME
    // ==========================
public void SetMusicVolume(float value)
{
    AudioManager.Instance.SetMusicVolume(value);
}

public void SetSFXVolume(float value)
{
    AudioManager.Instance.SetSFXVolume(value);
}



    
}
