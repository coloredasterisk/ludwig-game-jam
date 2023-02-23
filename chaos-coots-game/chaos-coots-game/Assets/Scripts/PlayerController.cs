using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [System.NonSerialized] public Rigidbody2D playerRB;
    public int health = 3;
    public bool invincible = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject emptySprite;

    public int attackDamage = 1;
    public GameObject attackArea;
    public GameObject attackHitBox;
    public float attackCooldown = 0.3f;
    public Vector2 direction = Vector2.zero;
    public Vector2 lastDirection = new Vector2(0.1f,0);
    public float movementSpeed = 100;
    public float dashCooldown = 1f;
    public float dashSpeed = 50;
    public bool isDashing = false;
    public bool isAttacking = false;

    private static float ANIMATE_RUN = 0.25f;
    private static float ANIMATE_STOP = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        for(int i = 0; i < health; i++)
        {
            CanvasReference.Instance.CreateLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0)
        {
            direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                direction += new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += new Vector2(0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += new Vector2(-1, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += new Vector2(1, 0);
            }
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift)) && !isDashing)
            {
                StartCoroutine(DashCooldown());
            }
            else
            {
                playerRB.AddForce(direction * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                StartCoroutine(AttackCooldown());
            }
        }
        
        

        if(Mathf.Abs(playerRB.velocity.x) > 0.01 || Mathf.Abs(playerRB.velocity.y) > 0.01)
        {
            lastDirection = playerRB.velocity;
        }
    }

    private void FixedUpdate()
    {

        animator.SetFloat("moveX", lastDirection.x);
        animator.SetFloat("moveY", lastDirection.y);

        attackArea.transform.right = new Vector3(lastDirection.x, lastDirection.y, 0).normalized*100 +transform.localPosition - attackHitBox.transform.localPosition;
    }

    public IEnumerator DashCooldown()
    {
        isDashing = true;
        StartCoroutine(InvincibleTime(0.3f));
        playerRB.velocity = direction * dashSpeed;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
    public IEnumerator AttackCooldown()
    {
        isAttacking = true;
        GameObject hitBox = Instantiate(attackHitBox, attackHitBox.transform.position, attackArea.transform.rotation);
        hitBox.GetComponent<AttackHitBox>().damage = attackDamage;
        hitBox.SetActive(true);
        
        yield return new WaitForSeconds(0.20f);
        hitBox.SetActive(false);
        Destroy(hitBox);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
        else
        {
            if(lastDirection.x > ANIMATE_RUN)
            {
                lastDirection.x = ANIMATE_STOP;
            }
            else if (lastDirection.x < -ANIMATE_RUN)
            {
                lastDirection.x = -ANIMATE_STOP;
            }

            if(lastDirection.y > ANIMATE_RUN)
            {
                lastDirection.y = ANIMATE_STOP;
            }
            else if(lastDirection.y < -ANIMATE_RUN)
            {
                lastDirection.y = -ANIMATE_STOP;
            }
        
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            if (lastDirection.x > ANIMATE_RUN)
            {
                lastDirection.x = ANIMATE_STOP;
            }
            else if (lastDirection.x < -ANIMATE_RUN)
            {
                lastDirection.x = -ANIMATE_STOP;
            }

            if (lastDirection.y > ANIMATE_RUN)
            {
                lastDirection.y = ANIMATE_STOP;
            }
            else if (lastDirection.y < -ANIMATE_RUN)
            {
                lastDirection.y = -ANIMATE_STOP;
            }
        }
    }

    public void TakeDamage()
    {
        if(invincible == false)
        {
            health -= 1;
            if (health == 0)
            {
                StartCoroutine(CrashSprite());
                
            }
            GameManager.Instance.CameraDistortion();
            CanvasReference.Instance.RemoveLife();
            StartCoroutine(InvincibleTime(1f));
        }

    }

    public IEnumerator InvincibleTime(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }
    public IEnumerator CrashSprite()
    {
        Time.timeScale = 0.001f; //almost paused
        for (int i = 1; i <= 30; i++)
        {
            GameObject emptyClone = Instantiate(emptySprite);
            emptyClone.transform.position = transform.position + new Vector3(0.1f*i, -0.1f*i);
            emptyClone.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            emptyClone.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            yield return new WaitForSeconds(0.00003f);
        }
        CanvasReference.Instance.crashWindow.SetActive(true);
        health -= 100;
        
        

    }
}
