using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectType
{
    Fish,
    Life,
    AttackSpeed,
    DashCooldown,
    Damage,
};

public class CollectableItem : MonoBehaviour
{
    private bool collected = false;
    private float currentDegree = 0;
    private float spinMultipler = 150;
    public int value;
    public float magnetSpeed = 1;
    public CollectType type;
    public AudioSource audioSource;
    public List<AudioClip> soundEffects;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(audioSource);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            if (GameManager.Instance.playing)
            {
                Collect(collision);
            }
            
        }
    }
    private void Update()
    {
        currentDegree += Time.deltaTime * spinMultipler;
        transform.rotation = Quaternion.Euler(0, currentDegree, 0);

        transform.position += (GameManager.Instance.player.transform.position - transform.position).normalized * Time.deltaTime * magnetSpeed;
    }

    public void Collect(Collider2D collision)
    {
        collected = true;
        audioSource.PlayOneShot(soundEffects[0]);
        GetComponent<SpriteRenderer>().color = Color.clear;

        if(type == CollectType.Fish)
        {
            DataManager.totalCurrency += value;
            DataManager.currency += value;
            CanvasReference.Instance.currencyText.text = "" + DataManager.currency;
            
        }
        else if(type == CollectType.Life)
        {
            collision.GetComponent<PlayerController>().health += value;
            CanvasReference.Instance.CreateLife();
        }
        else if (type == CollectType.AttackSpeed)
        {
            collision.GetComponent<PlayerController>().attackCooldown /= 2;
        }
        else if (type == CollectType.DashCooldown)
        {
            collision.GetComponent<PlayerController>().dashCooldown /= 2;
        }
        else if(type == CollectType.Damage)
        {
            collision.GetComponent<PlayerController>().attackDamage += 1;
        }

        StartCoroutine(DelayCollect());
    }

    public IEnumerator DelayCollect()
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }

}
