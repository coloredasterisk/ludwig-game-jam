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

            if (CompareTag("Silence"))
            {
                GameManager.Instance.music.clip = GameManager.Instance.soundEffects[2];
            }

            //activate enemies
            foreach(var enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                if (enemy.CompareTag("Boss"))
                {
                    GameManager.Instance.music.clip = GameManager.Instance.soundEffects[2];
                    GameManager.Instance.music.mute = false;
                    GameManager.Instance.music.volume = DataManager.musicVolume;
                    GameManager.Instance.music.Play();
                    CanvasReference.Instance.bossBar.gameObject.SetActive(true);
                }
            }
            enemies.Clear();
        }
    }

    private static IEnumerator Silence()
    {
        float volume = GameManager.Instance.music.volume;
        while (volume > 0)
        {
            volume -= Time.unscaledDeltaTime;
            GameManager.Instance.music.volume = volume;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        GameManager.Instance.music.mute = true;
        
    }
}
