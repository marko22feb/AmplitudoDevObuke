using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Settings : MonoBehaviour
{
    private Canvas SettingsCanvas;
    private Canvas MainMenuCanvas;
    [SerializeField]
    private GameObject KeybindPanel;
    [SerializeField]
    private GameObject AudioPanel;
    [SerializeField]
    private GameObject GraphicsPanel;

    private GameObject currentActivePanel;
    private bool UnsavedSettings;

    [Header ("Audio")]
    [SerializeField]
    private AudioMixer mainMixer;
    [SerializeField]
    Slider MasterSlider;
    [SerializeField]
    Slider SFXSLider;
    [SerializeField]
    Slider MusicSlider;
    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    Dropdown presetDropdown;
    [SerializeField]
    Dropdown AADropdown;
    [SerializeField]
    Dropdown TextureDropdown;

    private void Start()
    {
        SettingsCanvas = GetComponent<Canvas>();
        if (GameController.control.IsMainMenu)
        MainMenuCanvas = GameObject.Find("MainMenuCanvas").GetComponent<Canvas>();
        RefreshAudioSliders();
        RefreshDropdownBoxes();

        LoadSettings();
        PopulateResolutions();
    }

    public void RefreshDropdownBoxes()
    {
        presetDropdown.value = QualitySettings.GetQualityLevel();
        AADropdown.value = QualitySettings.antiAliasing;
        TextureDropdown.value = 3 - QualitySettings.masterTextureLimit;
    }

    public void RefreshAudioSliders()
    {
        float masterVolume;
        mainMixer.GetFloat("MasterVolume", out masterVolume);
        MasterSlider.value = masterVolume;

        mainMixer.GetFloat("SFXVolume", out masterVolume);
        SFXSLider.value = masterVolume;

        mainMixer.GetFloat("MusicVolume", out masterVolume);
        MusicSlider.value = masterVolume;
    }

    public void OnMasterVolumeChanged(float volume)
    {
        mainMixer.SetFloat("MasterVolume", volume);
        UnsavedSettings = true;
    }
    public void OnSFXVolumeChanged(float volume)
    {
        mainMixer.SetFloat("SFXVolume", volume);
        UnsavedSettings = true;
    }
    public void OnMusicVolumeChanged(float volume)
    {
        mainMixer.SetFloat("MusicVolume", volume);
        UnsavedSettings = true;
    }

    public void OnPresetQualityChanged(int value)
    {
        if (value == 6) return;
        QualitySettings.SetQualityLevel(value);

        RefreshDropdownBoxes();
    }

    public void OnTextureQualityChanged(int value)
    {
        QualitySettings.masterTextureLimit = (3 - value);
        presetDropdown.value = 6;
    }

    public void OnAntiAliasingChanged(int value)
    {
        QualitySettings.antiAliasing = value;
        presetDropdown.value = 6;
    }

    public void OnResolutionChanged(int value)
    {
        Screen.SetResolution(Screen.resolutions[value].width, Screen.resolutions[value].height, Screen.fullScreen);
    }

    public void PopulateResolutions()
    {
        resolutionDropdown.options.Clear();

        List<string> ResolutionString = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            ResolutionString.Add(Screen.resolutions[i].width + " x " + Screen.resolutions[i].height);
            resolutionDropdown.options.Add(new Dropdown.OptionData() { text = ResolutionString[i] });

            if (Screen.currentResolution.height == Screen.resolutions[i].height && Screen.currentResolution.width == Screen.resolutions[i].width)
                resolutionDropdown.value = i;
        }
    }

    public void IfUnsavedChangesSave()
    {
        if (UnsavedSettings)
        {
            SaveSettings();
            UnsavedSettings = false;
        }
    }

    public void OnMainMenuButtonClick(int type)
    {
        switch (type)
        {
            case 0:
                if (currentActivePanel != null) currentActivePanel.SetActive(false);
                AudioPanel.SetActive(true);
                currentActivePanel = AudioPanel;
                break;
            case 1:
                if (currentActivePanel != null) currentActivePanel.SetActive(false);
                GraphicsPanel.SetActive(true);
                currentActivePanel = GraphicsPanel;

                IfUnsavedChangesSave();
                break;
            case 2:
                IfUnsavedChangesSave();
                break;
            case 3:
                if (currentActivePanel != null) currentActivePanel.SetActive(false);
                KeybindPanel.SetActive(true);
                currentActivePanel = KeybindPanel;
                IfUnsavedChangesSave();
                break;
            case 4:
                if (GameController.control.IsMainMenu) {
                    SettingsCanvas.enabled = false;
                    MainMenuCanvas.enabled = true;
                } else 
                {
                    Time.timeScale = 1f;
                    GameController.control.IsInputEnabled = true;
                    SettingsCanvas.enabled = false;
                };
                IfUnsavedChangesSave();
                break;
            default:
                IfUnsavedChangesSave();
                break;
        }
    }

    void SaveSettings()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "settings.123");
        SettingsSave save = new SettingsSave();

        save.MasterVolume = MasterSlider.value;
        save.SFXVolume = SFXSLider.value;
        save.MusicVolume = MusicSlider.value;

        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "settings.123"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "settings.123", FileMode.Open);
            SettingsSave save = (SettingsSave)binaryFormatter.Deserialize(file);

            MasterSlider.value = save.MasterVolume;
            OnMasterVolumeChanged(save.MasterVolume);
            SFXSLider.value = save.SFXVolume;
            OnSFXVolumeChanged(save.SFXVolume);
            MusicSlider.value = save.MusicVolume;
            OnMusicVolumeChanged(save.MusicVolume);

            UnsavedSettings = false;

            file.Close();

        }
    }
}

[System.Serializable]

public class SettingsSave
{
    public float MasterVolume;
    public float SFXVolume;
    public float MusicVolume;
}
