using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float lifeTime = 0;
    public float maxLife = 10;

    private AudioSource audioSource;
    public List<AudioClip> soundEffects; //0 pellet break
    public GameObject soundPrefab;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource != null)
        {
            DataManager.AddSoundEffect(audioSource);
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime > maxLife)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        soundPrefab.GetComponent<AudioSource>().volume = DataManager.soundEffectVolume;
        soundPrefab.GetComponent<AudioSource>().clip = soundEffects[0];
        GameObject soundObject = Instantiate(soundPrefab);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ranged"))
        {
            collision.gameObject.GetComponent<HealthAttachment>().TakeDamage(1);
        } 
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossBehavior>().TakeDamage(1);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ranged"))
        {
            collision.gameObject.GetComponent<HealthAttachment>().TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossBehavior>().TakeDamage(1);
        }
    }

}
