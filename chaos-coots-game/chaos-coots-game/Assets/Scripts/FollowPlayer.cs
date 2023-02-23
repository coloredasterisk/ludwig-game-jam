using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Rigidbody2D objectRB;
    public float movementSpeed = 50;
    public HealthAttachment life;
    // Start is called before the first frame update
    void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();
        life = GetComponent<HealthAttachment>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(life.health > 0)
        {
            Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
            objectRB.AddForce(direction * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
            
        }
    }

}
