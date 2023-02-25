using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private float currentDegree = 0;
    public float spinMultipler = 180;
    public float startRotation = 0;

    public void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 0, currentDegree + startRotation);
    }

    private void Update()
    {
        currentDegree += Time.deltaTime * spinMultipler;
        transform.rotation = Quaternion.Euler(0, 0, currentDegree + startRotation);
    }
}
