using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DasherBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public float dashSpeed = 500;
    private Rigidbody2D objectRB;
    private FollowPlayer movementController;

    public float dashInterval = 5;
    private float currentTime = 0;
    public float recordTime = 0.3f;
    public Vector3 recordedPosition;

    public HealthAttachment life;
    // Start is called before the first frame update
    void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();
        movementController = GetComponent<FollowPlayer>();
        currentTime = dashInterval;
        life = GetComponent<HealthAttachment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life.health > 0)
        {
            Vector3 distance = GameManager.Instance.player.transform.position - transform.position;
            FaceFromPosition(distance);
            currentTime -= Time.deltaTime;
            if (currentTime <= recordTime)
            {
                StartCoroutine(PrepareToDash());

            }
        }
    }

    public void FaceFromPosition(Vector3 distance)
    {
        if (Mathf.Abs(distance.x) >= Mathf.Abs(distance.y))
        {
            if (distance.x > 0)
            {
                animator.SetInteger("direction", 3);
            }
            else
            {
                animator.SetInteger("direction", 1);
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

    public IEnumerator PrepareToDash()
    {
        currentTime = dashInterval + recordTime;
        recordedPosition = GameManager.Instance.player.transform.position;
        //spriteRenderer.color = Color.red;
        movementController.enabled = false;
        animator.SetBool("dashing", true);
        yield return new WaitForSeconds(recordTime/2);

        HealthAttachment soundSource = GetComponent<HealthAttachment>();
        soundSource.audioSource.PlayOneShot(soundSource.soundEffects[2]);

        animator.SetBool("dashing", false);
        yield return new WaitForSeconds(recordTime/2);
        Dash();
        currentTime = dashInterval;
        //spriteRenderer.color = Color.white;
        movementController.enabled = true;

    }

    void Dash() 
    {
        Vector3 direction = (recordedPosition - transform.position).normalized;
        objectRB.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
    }
}
