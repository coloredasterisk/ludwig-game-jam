using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int furnitureDestroyed = 0;
    public static int enemiesDestroyed = 0;
    public static int totalCurrency = 0;
    public static int fatalErrors = 0;
    public static int currency = 0;
    public static float time = 0;

    public static float musicVolume = 0.5f;
    public static float soundEffectVolume = 0.5f;
    public static List<AudioSource> soundEffects = new List<AudioSource>();

    public static void AddSoundEffect(AudioSource sound)
    {
        sound.volume = soundEffectVolume;
        soundEffects.Add(sound);
    }

    public static void ChangeSoundVolume(float value)
    {
        soundEffectVolume = value;
        soundEffects.ForEach(x => { if (x != null) x.volume = value; });
    }
}
