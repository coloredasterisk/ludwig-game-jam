using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject initialTrigger;
    public GameObject userInterface;

    public static GameManager Instance;
    public Transform currentCameraTarget;
    public CinemachineTargetGroup targetGroup;
    public PlayerController player;
    public PostProcessVolume volume;

    public bool isPaused = false;

    private float distortValue = 0;
    private float distortHoldTime = 1;
    private float distortCurrentTime = 0;

    //All of this for moving the pause menu??
    public bool isMoving = false;
    public float requestedPausePosition = -400;
    public float pausedTimer = 1;
    //end

    public AudioSource music;
    public AudioSource audioSource;
    public List<AudioClip> soundEffects;


    public float timer = 0;

    public bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                ToggleGame();
                
            }

            if (initialTrigger.activeSelf == false)
            {
                if (Input.anyKeyDown)
                {
                    initialTrigger.SetActive(true);
                    userInterface.SetActive(true);
                    music.Play();
                }
            }
            if (player.health <= -50)
            {
                if (Input.anyKeyDown)
                {
                    DataManager.soundEffects.Clear();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    Time.timeScale = 1;

                }
            }
            else
            {
                timer += Time.deltaTime;
            }
            if (distortValue > 0)
            {
                if (distortCurrentTime > 0)
                {
                    distortCurrentTime -= Time.deltaTime;
                }
                else
                {
                    distortValue -= Time.deltaTime * 10;
                    if (distortValue < 0)
                    {
                        distortValue = 0;
                    }
                    volume.weight = distortValue;
                }
            }
        }
        
    }

    public void ChangeTarget(Transform position, float radius, float weight)
    {
        targetGroup.RemoveMember(currentCameraTarget);
        targetGroup.AddMember(position, weight, radius);
        currentCameraTarget = position;
    }
    public void CameraDistortion()
    {
        distortValue = 1;
        volume.weight = distortValue;
        distortCurrentTime = distortHoldTime;
    }

    public void ConcludeGame()
    {
        playing = false;
        audioSource.PlayOneShot(soundEffects[0]);

        string timeDisplay = CanvasReference.convertToTime(DataManager.time);

        CanvasReference.Instance.endWindowText.text = "Virus Terminated in " + timeDisplay +
            "\r\nRogue AI Destroyed: " + DataManager.enemiesDestroyed +
            "\r\nFurniture Destroyed:" + DataManager.furnitureDestroyed +
            "\r\nTotal Goldfish Collected:" + DataManager.totalCurrency +
            "\r\nBlue Screens of Death: " + DataManager.fatalErrors +
            "\r\n\r\nThanks for Playing!";
        CanvasReference.Instance.endWindow.SetActive(true);
    }

    public void ToggleGame()
    {
        if (isPaused)
        {
            PlayGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {

        if(!isMoving)
        {
            Time.timeScale = 0f;
            isPaused = true;
            requestedPausePosition = 0;
            StartCoroutine(MovePauseMenu());
        }
    }
    public void PlayGame()
    {

        if (!isMoving)
        {
            Time.timeScale = 1;
            isPaused = false;
            requestedPausePosition = -400;
            StartCoroutine(MovePauseMenu());
        }
    }

    public IEnumerator MovePauseMenu()
    {
        CanvasReference.Instance.pausedMenu.SetActive(true);
        pausedTimer = 0;
        isMoving = true;
        Vector3 currentPos = CanvasReference.Instance.pausedMenu.transform.localPosition;
        while (pausedTimer < 1)
        {
            pausedTimer += Time.unscaledDeltaTime * 5;
            CanvasReference.Instance.pausedMenu.transform.localPosition = 
                new Vector3(0, Mathf.Lerp(currentPos.y, requestedPausePosition, pausedTimer));
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        if (requestedPausePosition < -300)
        {
            CanvasReference.Instance.pausedMenu.SetActive(false);
        }
        isMoving = false;
    }
}
