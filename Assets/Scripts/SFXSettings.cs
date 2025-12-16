using UnityEngine;
using TMPro;

public class SFXSettings : MonoBehaviour
{
    [Header("SFX Audio Sources")]
    public AudioSource[] sfxSources;

    [Header("UI")]
    public TMP_Dropdown sfxDropdown;

    const string PREF_SFX = "SFX_VOLUME_INDEX";

    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt(PREF_SFX, 4);

        sfxDropdown.SetValueWithoutNotify(savedIndex);
        ApplyVolume(savedIndex);

        sfxDropdown.onValueChanged.RemoveAllListeners();
        sfxDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDropdownChanged(int index)
    {
        ApplyVolume(index);
        PlayerPrefs.SetInt(PREF_SFX, index);
        PlayerPrefs.Save();
    }

    void ApplyVolume(int index)
    {
        float volume = index switch
        {
            0 => 0f,
            1 => 0.25f,
            2 => 0.5f,
            3 => 0.75f,
            4 => 1f,
            _ => 1f
        };

        foreach (var src in sfxSources)
        {
            if (src != null)
                src.volume = volume;
        }
    }
}
