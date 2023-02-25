using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttachment : MonoBehaviour
{
    public GameObject emptySprite;

    public List<DoorBehavior> progression;
    public SpriteModifier modifier;

    public int maxHealth = 1;
    public int health = 1;

    public AudioSource audioSource;
    public List<AudioClip> soundEffects; //0 death sounds //1 take damage sounds



    public List<SpriteValues> healthSprites;
    public Dictionary<int, Sprite> spriteDictionary = new Dictionary<int, Sprite>();

    [System.Serializable] public struct SpriteValues
    {
        public int health;
        public Sprite sprite;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(audioSource);
        health = maxHealth;

        foreach(SpriteValues spriteValues in healthSprites)
        {
            spriteDictionary.Add(spriteValues.health, spriteValues.sprite);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            if(maxHealth > 0)
            {
                maxHealth = 0;
                DropItems drops = GetComponent<DropItems>();
                if (drops != null)
                {
                    drops.SpawnDrops();
                }
                if (progression != null && progression.Count > 0)
                {
                    foreach (DoorBehavior door in progression)
                    {
                        if (door != null)
                        {
                            door.RemoveSelf(this);
                        }

                    }
                }
                
                
                

                GetComponent<Rigidbody2D>().simulated = false;
                if (GetComponent<BoxCollider2D>()) { GetComponent<BoxCollider2D>().enabled = false;}
                else if (GetComponent<CircleCollider2D>()){ GetComponent<CircleCollider2D>().enabled = false;}
                else if (GetComponent<CapsuleCollider2D>()){ GetComponent<CapsuleCollider2D>().enabled = false; }
                StartCoroutine(CrashSprite());
                
            }
            
            
        }
        else if (spriteDictionary.ContainsKey(health))
        {
            if(modifier != null)
            {
                modifier.spriteRenderer.sprite = spriteDictionary[health];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = spriteDictionary[health];
            }
            
        }
        if(modifier != null)
        {
            modifier.GlitchSprite();
            if(health > 0)
            {
                audioSource.PlayOneShot(soundEffects[1]);
            }
        }
        
    }

    public IEnumerator CrashSprite()
    {
        audioSource.clip = soundEffects[0]; //crash sound
        
        for (int i = 1; i <= 10; i++)
        {
            audioSource.Play();
            GameObject emptyClone = Instantiate(emptySprite, transform);
            emptyClone.transform.position = transform.position + new Vector3(0.1f * i, -0.1f * i);
            if(modifier != null)
            {
                emptyClone.GetComponent<SpriteRenderer>().sprite = modifier.spriteRenderer.sprite;
            }
            else
            {
                emptyClone.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            }
            
            emptyClone.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }
}
