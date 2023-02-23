using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMovement : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        transform.right = transform.position - direction * 1000;
    }
}
