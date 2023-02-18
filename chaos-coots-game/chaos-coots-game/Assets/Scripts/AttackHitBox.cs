using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public BoxCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<HealthAttachment>() != null)
        {
            collision.GetComponent<HealthAttachment>().TakeDamage();
        }
    }
}
