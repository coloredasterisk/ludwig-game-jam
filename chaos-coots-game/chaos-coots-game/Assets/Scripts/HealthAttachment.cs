using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttachment : MonoBehaviour
{
    public DoorBehavior progression;
    public SpriteModifier modifier;

    public int maxHealth = 1;
    public int health = 1;

    public List<SpriteValues> healthSprites;
    public Dictionary<int, Sprite> spriteDictionary = new Dictionary<int, Sprite>();

    [System.Serializable] public struct SpriteValues
    {
        public int health;
        public Sprite sprite;
    }

    private void Start()
    {
        
        maxHealth = health;

        foreach(SpriteValues spriteValues in healthSprites)
        {
            spriteDictionary.Add(spriteValues.health, spriteValues.sprite);
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        if(health <= 0)
        {
            DropItems drops = GetComponent<DropItems>();
            if(drops != null)
            {
                drops.SpawnDrops();
            }
            if(progression != null)
            {
                progression.RemoveSelf(this);
            }
            Destroy(gameObject);
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
        }
        
    }
}
