using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttack : MonoBehaviour
{
    //public AudioSource audioSource;
    //public List<AudioClip> soundEffects; //0 clap

    public BoxCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //DataManager.AddSoundEffect(audioSource);
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage();

        }

    }
}
