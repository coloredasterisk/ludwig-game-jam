using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 5;
    public float currentTime = 5;
    public int shootDistance = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            Vector3 distance = GameManager.Instance.player.transform.position - transform.position;
            if (Mathf.Abs(distance.x) < shootDistance && Mathf.Abs(distance.y) < shootDistance)
            {
                ShootProjectile();
                currentTime = shootInterval;
            }
                
        }
    }

    public void ShootProjectile()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 towardsPlayer = (playerPos - gameObject.transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, towardsPlayer+gameObject.transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = towardsPlayer * 15;
        
        

    }
}
