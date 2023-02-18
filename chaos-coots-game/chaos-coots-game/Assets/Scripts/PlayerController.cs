using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.NonSerialized] public Rigidbody2D playerRB;
    public GameObject attackArea;
    public GameObject attackHitBox;
    public Vector2 direction = Vector2.zero;
    public Vector2 lastDirection = Vector2.zero;
    public float movementSpeed = 100;
    public float dashSpeed = 50;
    public bool isDashing = false;
    public bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
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
        if (Input.GetMouseButtonDown(1) && !isDashing)
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
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        attackHitBox.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }
}
