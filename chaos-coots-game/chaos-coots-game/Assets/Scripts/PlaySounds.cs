using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>(); //pop, swish
    float timer = 0;
    int index = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 4.5f)
        {
            if(index != 2)
            {
                index = 2;
                audioSource.PlayOneShot(audioClips[1]);
            }
        }
        else if(timer > 2f)
        {
            if(index != 1)
            {
                index = 1;
                audioSource.PlayOneShot(audioClips[0]);
            } 
        }
    }
}
