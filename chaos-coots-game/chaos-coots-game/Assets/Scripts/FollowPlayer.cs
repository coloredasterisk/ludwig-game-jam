using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Rigidbody2D objectRB;
    public float movementSpeed = 100;
    public int followDistance = 15;

    // Start is called before the first frame update
    void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 distance = GameManager.Instance.player.transform.position - transform.position;
        if (Mathf.Abs(distance.x) < followDistance && Mathf.Abs(distance.y) < followDistance)
        {
            Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
            objectRB.AddForce(direction * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
            
        
    }

}
