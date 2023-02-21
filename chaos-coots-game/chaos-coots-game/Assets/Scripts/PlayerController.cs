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

    public GameObject attackArea;
    public GameObject attackHitBox;
    public Vector2 direction = Vector2.zero;
    public Vector2 lastDirection = new Vector2(0.1f,0);
    public float movementSpeed = 100;
    public float dashSpeed = 50;
    public bool isDashing = false;
    public bool isAttacking = false;
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
        playerRB.velocity = direction * dashSpeed;
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }
    public IEnumerator AttackCooldown()
    {
        isAttacking = true;
        GameObject hitBox = Instantiate(attackHitBox, attackHitBox.transform.position, attackArea.transform.rotation);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(0.20f);
        hitBox.SetActive(false);
        Destroy(hitBox);
        yield return new WaitForSeconds(0.30f);
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if(invincible == false)
        {
            health -= 1;
            if (health == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            CanvasReference.Instance.RemoveLife();
            StartCoroutine(InvincibleTime());
        }

    }

    public IEnumerator InvincibleTime()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
