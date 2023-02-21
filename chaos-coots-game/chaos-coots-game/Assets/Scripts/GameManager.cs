using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform currentCameraTarget;
    public CinemachineTargetGroup targetGroup;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTarget(Transform position, float radius)
    {
        targetGroup.RemoveMember(currentCameraTarget);
        targetGroup.AddMember(position, 1, radius);
        currentCameraTarget = position;


    }
}
