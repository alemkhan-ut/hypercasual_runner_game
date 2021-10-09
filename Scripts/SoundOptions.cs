using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    public static SoundOptions instane;

    [SerializeField] private bool _isSoundActive;
    [SerializeField] private bool _isMusicActive;

    [SerializeField] private Slider _soundButtonSlider;
    [SerializeField] private Slider _musicButtonSlider;

    public bool IsSoundActive { get => _isSoundActive; }
    public bool IsMusicActive { get => _isMusicActive; }

    private void Awake()
    {
        instane = this;
    }

    public void RefreshSoundOptions()
    {
        _soundButtonSlider.value = GetSettings(GameData.SettingsType.SoundState);
        _musicButtonSlider.value = GetSettings(GameData.SettingsType.MusicState);

        if (GetSettings(GameData.SettingsType.MusicState) > 0)
        {
            _isMusicActive = true;
        }
        else
        {
            _isMusicActive = false;
        }

        if (GetSettings(GameData.SettingsType.SoundState) > 0)
        {
            _isSoundActive = true;
        }
        else
        {
            _isSoundActive = false;
        }
    }

    public void SetSettings(GameData.SettingsType settingsType, float value)
    {
        GameData.SetSettingsPrefs(settingsType, value);
        RefreshSoundOptions();
    }
    public float GetSettings(GameData.SettingsType settingsType)
    {
        return GameData.GetSettingsPrefs(settingsType);
    }

    public void DeleteSetttings(GameData.SettingsType settingsType)
    {
        GameData.DeleteSettingsPrefs(settingsType);
    }
}
