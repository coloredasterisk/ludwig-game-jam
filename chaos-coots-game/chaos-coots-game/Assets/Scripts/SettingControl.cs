using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    [Header("Paused")]
    public Slider pausedsoundSlider;
    public TextMeshProUGUI pausedsoundSliderValue;

    public Slider pausedmusicSlider;
    public TextMeshProUGUI pausedmusicSliderValue;
    [Header("Settings")]
    

    public Slider settingsoundSlider;
    public TextMeshProUGUI settingsoundSliderValue;

    public Slider settingmusicSlider;
    public TextMeshProUGUI settingsmusicSliderValue;

    // Start is called before the first frame update
    void Start()
    {

        pausedsoundSlider.value = DataManager.soundEffectVolume;
        pausedsoundSliderValue.text = "" + (DataManager.soundEffectVolume * 100) + "%";

        pausedmusicSlider.value = DataManager.musicVolume;
        pausedmusicSliderValue.text = "" + (DataManager.musicVolume * 100) + "%";

        settingsoundSlider.value = DataManager.soundEffectVolume;
        settingsoundSliderValue.text = "" + (DataManager.soundEffectVolume * 100) + "%";

        settingmusicSlider.value = DataManager.musicVolume;
        settingsmusicSliderValue.text = "" + (DataManager.musicVolume * 100) + "%";

        DelayMusic();
    }

    public void ChangeSoundVolume(float value)
    {

        pausedsoundSliderValue.text = "" + Math.Round(value * 100, 1) + "%";
        settingsoundSliderValue.text = "" + Math.Round(value * 100, 1) + "%";

        DataManager.ChangeSoundVolume(value);

        pausedsoundSlider.value = DataManager.soundEffectVolume;
        settingsoundSlider.value = DataManager.soundEffectVolume;
    }
    public void ChangeMusicVolume(float value)
    {
        pausedmusicSliderValue.text = "" + Math.Round(value * 100, 1) + "%";
        settingsmusicSliderValue.text = "" + Math.Round(value * 100, 1) + "%";
        DataManager.musicVolume = value;
        if(GameManager.Instance != null)
        {
            GameManager.Instance.music.volume = value;
        }

        pausedmusicSlider.value = DataManager.musicVolume;
        settingmusicSlider.value = DataManager.musicVolume;
    }

    public IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.music.volume = DataManager.musicVolume;
    }
}
