using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitchTrigger : MonoBehaviour
{
    public float cameraRadius;
    public float weight = 2;

    public List<GameObject> enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().ChangeTarget(transform, cameraRadius, weight);

            //activate enemies
            foreach(var enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                if (enemy.CompareTag("Boss"))
                {
                    CanvasReference.Instance.bossBar.gameObject.SetActive(true);
                }
            }
            enemies.Clear();
        }
    }
}
