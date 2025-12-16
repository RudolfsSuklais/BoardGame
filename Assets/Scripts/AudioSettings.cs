using UnityEngine;
using TMPro;

public class AudioSettings : MonoBehaviour
{
    public AudioSource gameMusic;
    public TMP_Dropdown musicDropdown;

    const string PREF_MUSIC = "MUSIC_VOLUME_INDEX";

    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt(PREF_MUSIC, 4);

        musicDropdown.SetValueWithoutNotify(savedIndex);
        ApplyVolume(savedIndex);

        musicDropdown.onValueChanged.RemoveAllListeners();
        musicDropdown.onValueChanged.AddListener(OnMusicDropdownChanged);
    }

    void OnMusicDropdownChanged(int index)
    {
        ApplyVolume(index);
        PlayerPrefs.SetInt(PREF_MUSIC, index);
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

        gameMusic.volume = volume;
    }
}
