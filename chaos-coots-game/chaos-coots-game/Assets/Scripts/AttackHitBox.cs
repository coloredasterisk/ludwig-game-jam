using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public int damage;
    public BoxCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            collision.GetComponent<Rigidbody2D>().velocity *= -1;
        }
    }
}
