using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttack : MonoBehaviour
{

    public BoxCollider2D hitbox;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage();

        }
        if (collision.GetComponent<HealthAttachment>() != null)
        {
            collision.GetComponent<HealthAttachment>().TakeDamage(1);

        }

    }
}
