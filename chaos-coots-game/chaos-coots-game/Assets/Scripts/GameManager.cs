using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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

    private float distortValue = 0;
    private float distortHoldTime = 1;
    private float distortCurrentTime = 0;

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
            
            if (initialTrigger.activeSelf == false)
            {
                if (Input.anyKeyDown)
                {
                    initialTrigger.SetActive(true);
                    userInterface.SetActive(true);
                }
            }
            if (player.health <= -50)
            {
                if (Input.anyKeyDown)
                {
                    if (!Input.GetKeyDown(KeyCode.LeftAlt) && !Input.GetKeyDown(KeyCode.RightAlt) && !Input.GetKeyDown(KeyCode.F4))
                    {
                        DataManager.soundEffects.Clear();
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        Time.timeScale = 1;
                    }

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

}
