using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject laserPrefab;
    public float shootInterval = 5;
    public float currentTime = 5;
    private float recordTime = 0.4f;

    public Vector3 recordedPosition;
    public HealthAttachment life;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<HealthAttachment>().modifier.spriteRenderer;
        life = GetComponent<HealthAttachment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life.health > 0)
        {
            Vector3 playerPos = GameManager.Instance.player.transform.position;
        
            currentTime -= Time.deltaTime;
            
            FaceFromPosition(playerPos);

            if (currentTime <= recordTime)
            {


                StartCoroutine(PrepareToShoot());


            }
        }
        
        
    }
    public void FaceFromPosition(Vector3 playerPos)
    {
        Vector3 distance = playerPos - transform.position;
        if (Mathf.Abs(distance.x) >= Mathf.Abs(distance.y))
        {
            if(distance.x > 0)
            {
                animator.SetInteger("direction", 1);
            }
            else
            {
                animator.SetInteger("direction", 3);
            }
        }
        else
        {
            if (distance.y > 0)
            {
                animator.SetInteger("direction", 0);
            }
            else
            {
                animator.SetInteger("direction", 2);
            }
        }
    }

    public IEnumerator PrepareToShoot()
    {
        

        currentTime = shootInterval + recordTime;
        recordedPosition = GameManager.Instance.player.transform.position;
        //spriteRenderer.color = Color.red;
        animator.SetBool("shooting", true);
        yield return new WaitForSeconds(recordTime/2);
        HealthAttachment soundSource = GetComponent<HealthAttachment>();
        soundSource.audioSource.PlayOneShot(soundSource.soundEffects[2]);

        animator.SetBool("shooting", false);
        yield return new WaitForSeconds(recordTime/2);
        ShootLaser();
        currentTime = shootInterval;
        //spriteRenderer.color = Color.white;

    }

    public void ShootLaser()
    {
        
        Vector3 playerPos = recordedPosition;
        Vector3 towardsPlayer = (playerPos - gameObject.transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, towardsPlayer, Mathf.Infinity, 9);

        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            float midX = (hit.point.x - transform.position.x) / 2 + transform.position.x;
            float midY = (hit.point.y - transform.position.y) / 2 + transform.position.y;
            Vector3 midPoint = new Vector3(midX, midY, 0) + (new Vector3(hit.point.x, hit.point.y) - transform.position).normalized *0.65f;


            float size =  Mathf.Sqrt( Mathf.Pow(hit.point.x - transform.position.x, 2) + Mathf.Pow(hit.point.y - transform.position.y, 2));

            GameObject projectile = Instantiate(laserPrefab, midPoint, transform.rotation);
            projectile.transform.localScale = new Vector3(size, 0.5f);
            projectile.transform.right = towardsPlayer;

        }

    }
}
