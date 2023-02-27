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
    public Camera mainCamera;

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

    public AudioSource catSource;
    public List<AudioClip> soundEffects; //0 crash, 1 attack, 2 hurt, 3 dash

    private static float ANIMATE_RUN = 0.5f;
    private static float ANIMATE_STOP = 0.025f;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        catSource = GetComponent<AudioSource>();
        DataManager.AddSoundEffect(catSource);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playing && !GameManager.Instance.isPaused)
        {
            if (health > 0)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                attackArea.transform.right = mouseWorldPos -transform.position;


                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 0);
                direction = Vector2.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    direction += new Vector2(0, 1);
                    animator.SetFloat("moveY", 1);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    direction += new Vector2(0, -1);
                    animator.SetFloat("moveY", animator.GetFloat("moveY")-1);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    direction += new Vector2(-1, 0);
                    animator.SetFloat("moveX", -1);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    direction += new Vector2(1, 0);
                    animator.SetFloat("moveX", animator.GetFloat("moveX") + 1);
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


            if (Mathf.Abs(playerRB.velocity.x) > 0.01 || Mathf.Abs(playerRB.velocity.y) > 0.01)
            {
                lastDirection = playerRB.velocity;
            }
        }
        
    }

    public IEnumerator DashCooldown()
    {
        isDashing = true;
        catSource.PlayOneShot(soundEffects[3]);
        StartCoroutine(InvincibleTime(0.3f));
        playerRB.velocity = direction * dashSpeed;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
    public IEnumerator AttackCooldown()
    {
        //attack sound
        catSource.PlayOneShot(soundEffects[1]);

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
                DataManager.time = GameManager.Instance.timer;
                DataManager.fatalErrors += 1;
                GameManager.Instance.music.Stop();
                StartCoroutine(CrashSprite());

            }
            else
            {
                catSource.PlayOneShot(soundEffects[2]);
            }
            

            GameManager.Instance.CameraDistortion();
            CanvasReference.Instance.RemoveLife();
            StartCoroutine(InvincibleTime(1.5f));
        }

    }

    public IEnumerator InvincibleTime(float time)
    {
        invincible = true;
        for(float i = 0; i < time; i += 0.25f)
        {
            spriteRenderer.color = Color.cyan;
            yield return new WaitForSeconds(0.125f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.125f);
        }
        
        invincible = false;
    }
    public IEnumerator CrashSprite()
    {
        catSource.clip = soundEffects[0]; //crash sound

        Time.timeScale = 0.001f; //almost paused
        for (int i = 1; i <= 30; i++)
        {
            catSource.Play();
            GameObject emptyClone = Instantiate(emptySprite);
            emptyClone.transform.position = transform.position + new Vector3(0.1f*i, -0.1f*i);
            emptyClone.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            emptyClone.GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            yield return new WaitForSeconds(0.00003f);
        }
        CanvasReference.Instance.ShowCrashScreen();
        
        catSource.Stop();
        health -= 100;
        
        

    }
}
