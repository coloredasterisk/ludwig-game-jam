using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAttachment : MonoBehaviour
{
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
            Destroy(gameObject);
        }
        else if (spriteDictionary.ContainsKey(health))
        {
            modifier.spriteRenderer.sprite = spriteDictionary[health];
        }
        modifier.GlitchSprite();
    }
}
