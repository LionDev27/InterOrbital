using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private Slider _mainSlider, _musicSlider, _sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.GetInt("FirstPlay") == 0)
        {
            _mainSlider.value = 0.7f;
            SetMainVolume(0.7f);
        }
        else
            SetSlider(_mainSlider, "MasterVolume");
        SetSlider(_musicSlider, "MusicVolume");
        SetSlider(_sfxSlider, "FxVolume");
    }

    #region SliderActions

    public void SetMainVolume(float value)
    {
        SetVolume("MasterVol", value);
        SaveSettings();
    }

    public void SetMusicVolume(float value)
    {
        SetVolume("musicVol", value);
        SaveSettings();
    }

    public void SetSfxVolume(float value)
    {
        SetVolume("sfxVol", value);
        SaveSettings();
    }

    #endregion
    
    private void SetVolume(string mixerParameter, float value)
    {
        _masterMixer.SetFloat(mixerParameter, Mathf.Log10(value) * 20);
    }

    private void SetSlider(Slider slider, string prefsKey)
    {
        var value = PlayerPrefs.GetFloat(prefsKey);
        slider.value = Mathf.Pow(10, value / 20);
    }
    
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("FirstPlay", 1);
        _masterMixer.GetFloat("MasterVol", out var master);
        PlayerPrefs.SetFloat("MasterVolume", master);
        _masterMixer.GetFloat("musicVol", out var music);
        PlayerPrefs.SetFloat("MusicVolume", music);
        _masterMixer.GetFloat("sfxVol", out var sfx);
        PlayerPrefs.SetFloat("FxVolume", sfx);
        PlayerPrefs.Save();
    }
}