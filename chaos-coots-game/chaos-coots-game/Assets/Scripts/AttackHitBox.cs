using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> soundEffects; //0 clap
    public int damage;
    public BoxCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(audioSource);
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision);
        if (collision.GetComponent<HealthAttachment>() != null)
        {
            if (collision.GetComponent<Rigidbody2D>())
            {

                Vector3 lookVector = collision.transform.position - transform.position;
                collision.GetComponent<Rigidbody2D>().AddForce(lookVector * 25, ForceMode2D.Impulse);
            }
            collision.GetComponent<HealthAttachment>().TakeDamage(damage);

        } else if(collision.GetComponent<ShopAttachment>() != null)
        {
            
            collision.GetComponent<ShopAttachment>().SellItem();
        } else if(collision.GetComponent<ProjectileBehavior>() != null)
        {
            audioSource.PlayOneShot(soundEffects[0]);
            collision.gameObject.layer = 14;
            collision.GetComponent<Rigidbody2D>().velocity *= -1;
        } else if(collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossBehavior>().TakeDamage(damage);

        }
    }
}
