using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float lifeTime = 0;
    public float maxLife = 10;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ranged"))
        {
            collision.gameObject.GetComponent<HealthAttachment>().TakeDamage(1);
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
    }

}
