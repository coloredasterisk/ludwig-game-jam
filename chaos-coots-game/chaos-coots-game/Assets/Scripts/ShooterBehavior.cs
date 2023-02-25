using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    [Header("Assign for animation")]
    public Animator animator;

    public GameObject projectilePrefab;
    public float shootInterval = 5;
    public float currentTime = 5;
    public float shootSpeed = 15;

    public HealthAttachment life;
    // Start is called before the first frame update
    void Start()
    {
        life = GetComponent<HealthAttachment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life.health > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime > (2 * shootInterval / 3) && currentTime <= shootInterval)
            {
                animator.SetInteger("state", 0);
            }
            else if (currentTime > (shootInterval / 3))
            {
                animator.SetInteger("state", 1);
            }
            else if (currentTime > 0)
            {
                animator.SetInteger("state", 2);
            }
            else if (currentTime <= 0)
            {

                ShootProjectile();
                StartCoroutine(AnimateShooting());

            }
        }
            
    }

    public IEnumerator AnimateShooting()
    {
        HealthAttachment soundSource = GetComponent<HealthAttachment>();
        soundSource.audioSource.PlayOneShot(soundSource.soundEffects[2]);

        currentTime = shootInterval + 0.3f;
        animator.SetInteger("state", 3);
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("state", 0);
        yield return new WaitForSeconds(0.2f);
        
        currentTime = shootInterval;
    }

    public void ShootProjectile()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 towardsPlayer = (playerPos - gameObject.transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, towardsPlayer+gameObject.transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = towardsPlayer * shootSpeed;
    }
}
