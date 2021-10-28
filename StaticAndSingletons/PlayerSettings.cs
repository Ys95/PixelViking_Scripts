using UnityEngine;

public static class PlayerSettings
{
    public const string MUSIC_VOLUME = "MusicVolume";
    public const string SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public const string UI_SCALE = "UiScale";
    public const string LIGHTS_LEVEL = "LightsLevel";
    public const string VSYNC = "VSync";

    public const float DEFAULT_MUSIC_VOLUME = 0.2f;
    public const float DEFAULT_SOUND_EFFECTS_VOLUME = 1f;
    public const float DEFAULT_UI_SCALE = 1f;
    public const float DEFAULT_LIGHTS_LEVEL = 0f;

    public const bool DEFAULT_VSYNC_STATE = false;

    static SliderSetting musicVolume = new SliderSetting(MUSIC_VOLUME, DEFAULT_MUSIC_VOLUME, DEFAULT_MUSIC_VOLUME);
    static SliderSetting soundEffectsVolume = new SliderSetting(SOUND_EFFECTS_VOLUME, DEFAULT_SOUND_EFFECTS_VOLUME, DEFAULT_SOUND_EFFECTS_VOLUME);
    static SliderSetting uiScale = new SliderSetting(UI_SCALE, DEFAULT_UI_SCALE, DEFAULT_UI_SCALE);
    static SliderSetting lightsLevel = new SliderSetting(LIGHTS_LEVEL, DEFAULT_LIGHTS_LEVEL, DEFAULT_LIGHTS_LEVEL);

    static CheckmarkSetting vSync = new CheckmarkSetting(VSYNC, DEFAULT_VSYNC_STATE, DEFAULT_VSYNC_STATE);

    #region Getters

    public static SliderSetting MusicVolume { get => musicVolume; }
    public static SliderSetting SoundEffectsVolume { get => soundEffectsVolume; }
    public static SliderSetting UiScale { get => uiScale; }
    public static CheckmarkSetting VSync { get => vSync; }
    public static SliderSetting LightsLevel { get => lightsLevel; }

    #endregion Getters

    [System.Serializable]
    public abstract class Setting
    {
        [SerializeField] protected string name;
    }

    public class SliderSetting : Setting
    {
        [SerializeField] float defaultValue;
        [SerializeField] float value;

        #region Get/Set

        public string Name { get => name; set => name = value; }
        public float DefaultValue { get => defaultValue; }
        public float Value { get => value; set => this.value = value; }

        #endregion Get/Set

        public SliderSetting(string settingName, float defaultVal, float val)
        {
            name = settingName;
            defaultValue = defaultVal;
            value = val;
        }

        public void Save(float val)
        {
            float value = val;
            PlayerPrefs.SetFloat(name, value);
            PlayerPrefs.Save();
        }

        public float Load()
        {
            if (PlayerPrefs.HasKey(name)) return PlayerPrefs.GetFloat(name);
            return defaultValue;
        }
    }

    public class CheckmarkSetting : Setting
    {
        [SerializeField] bool defaultValue;
        [SerializeField] bool value;

        #region Get/Set

        public string Name { get => name; set => name = value; }
        public bool DefaultValue { get => defaultValue; }
        public bool Value { get => value; set => this.value = value; }

        #endregion Get/Set

        public CheckmarkSetting(string settingName, bool defaultVal, bool val)
        {
            name = settingName;
            defaultValue = defaultVal;
            value = val;
        }

        public void Save(bool val)
        {
            value = val;
            int intVal;

            if (value == true) intVal = 1;
            else intVal = 0;

            PlayerPrefs.SetInt(name, intVal);
            PlayerPrefs.Save();
        }

        public bool Load()
        {
            if (PlayerPrefs.HasKey(name))
            {
                int intVal = PlayerPrefs.GetInt(name);

                if (intVal == 1) return true;
                return false;
            }
            return defaultValue;
        }
    }

    public static void SetVSyncOption()
    {
        bool isOn = vSync.Load();

        if (isOn) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }
}