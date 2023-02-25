using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    float timer = 0;
    public float lifeTime = 1;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
