using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasReference : MonoBehaviour
{
    public static CanvasReference Instance;
    public TextMeshProUGUI currencyText;
    public GameObject listOfLives;
    public GameObject lifePrefab;
    public GameObject crashWindow;

    private void Start()
    {
        Instance = this;
    }

    public void CreateLife()
    {
        Vector3 position = new Vector3((listOfLives.transform.childCount)*37.5f +25f,-25);
        GameObject lifeClone = Instantiate(lifePrefab, listOfLives.transform);
        lifeClone.transform.localPosition = position;
    }
    public void RemoveLife()
    {
        GameObject image = listOfLives.transform.GetChild(listOfLives.transform.childCount - 1).gameObject;
        image.GetComponent<ImageModifier>().GlitchDestroy();
    }
}
