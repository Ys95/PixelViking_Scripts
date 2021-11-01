using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [Space]
    [SerializeField] OptionsSlider musicVolume;
    [SerializeField] OptionsSlider soundEffectsVolume;
    [SerializeField] OptionsSlider uiScale;
    [SerializeField] OptionsSlider brighntessLevel;

    [SerializeField] Toggle vSync;

    [System.Serializable]
    struct OptionsSlider
    {
        public Slider slider;
        public TextMeshProUGUI value;

        public void UpdateText()
        {
            if (value == null) return;
            value.text = slider.value.ToString();
        }
    }

    void OnEnable()
    {
        LoadOptions();
    }

    void LoadOptions()
    {
        float musicVol = PlayerSettings.MusicVolume.Load() * 100f;
        float soundEffectsVol = PlayerSettings.SoundEffectsVolume.Load() * 100f;
        float ui = PlayerSettings.UiScale.Load() * 100f;
        float brightness = PlayerSettings.LightsLevel.Load();

        Mathf.Round(musicVol);
        Mathf.Round(soundEffectsVol);
        Mathf.Round(ui);
        Mathf.Round(brightness);

        musicVolume.slider.value = musicVol;
        soundEffectsVolume.slider.value = soundEffectsVol;
        uiScale.slider.value = ui;
        brighntessLevel.slider.value = brightness;
        vSync.isOn = PlayerSettings.VSync.Load();
    }

    public void SaveOptions()
    {
        float musicVol = musicVolume.slider.value / 100;
        float soundEffectsVol = soundEffectsVolume.slider.value / 100;
        float ui = uiScale.slider.value / 100;
        float brightnessLevel = brighntessLevel.slider.value;

        PlayerSettings.MusicVolume.Save(musicVol);
        PlayerSettings.SoundEffectsVolume.Save(soundEffectsVol);
        PlayerSettings.UiScale.Save(ui);
        PlayerSettings.VSync.Save(vSync.isOn);
        PlayerSettings.LightsLevel.Save(brightnessLevel);

        PlayerSettings.SetVSyncOption();

        uiManager.LoadScaleSettings();

        if (SoundManager.Instance != null) SoundManager.Instance.LoadVolumeSettings();
        if (GlobalLightsSettings.Instance != null) GlobalLightsSettings.Instance.LoadSettings();
    }

    public void OnSliderValueChange()
    {
        musicVolume.UpdateText();
        soundEffectsVolume.UpdateText();
        uiScale.UpdateText();
        brighntessLevel.UpdateText();
    }
}