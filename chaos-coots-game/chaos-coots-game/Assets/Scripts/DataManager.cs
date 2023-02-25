using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int currency = 0;
    public static float time = 0;

    public static float soundEffectVolume = 0.5f;
    public static List<AudioSource> soundEffects = new List<AudioSource>();

    public static void AddSoundEffect(AudioSource sound)
    {
        sound.volume = soundEffectVolume;
        soundEffects.Add(sound);
    }
}
