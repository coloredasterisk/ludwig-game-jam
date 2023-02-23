using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public List<HealthAttachment> defeatEnemies;
    // Start is called before the first frame update
    void Start()
    {
        foreach(HealthAttachment defeat in defeatEnemies)
        {
            defeat.progression.Add(this);
        }
    }

    public void RemoveSelf(HealthAttachment objectToRemove)
    {
        if (defeatEnemies.Contains(objectToRemove))
        {
            defeatEnemies.Remove(objectToRemove);
        }
        if(defeatEnemies.Count == 0)
        {
            gameObject.SetActive(false);
        }
    }


}
