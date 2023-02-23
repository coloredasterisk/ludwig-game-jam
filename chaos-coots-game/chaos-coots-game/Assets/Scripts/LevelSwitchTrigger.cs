using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitchTrigger : MonoBehaviour
{
    public float cameraRadius;

    public List<GameObject> enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().ChangeTarget(transform, cameraRadius);

            //activate enemies
            foreach(var enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
            }
            enemies.Clear();
        }
    }
}
