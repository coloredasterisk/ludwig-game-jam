using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasReference : MonoBehaviour
{
    public static CanvasReference Instance;
    public TextMeshProUGUI currencyText;
    public GameObject listOfLives;
    public GameObject lifePrefab;
    public GameObject crashWindow;
    public Animator titleScreen;
    public TextMeshProUGUI crashText;
    public Slider bossBar;
    public Image bossBarColor;

    private void Start()
    {
        Instance = this;

        currencyText.text = "" + DataManager.currency;
    }

    public void CreateLife()
    {
        Vector3 position = new Vector3((listOfLives.transform.childCount)*37.5f +25f,0);
        GameObject lifeClone = Instantiate(lifePrefab, listOfLives.transform);
        lifeClone.transform.localPosition = position;
    }
    public void RemoveLife()
    {
        GameObject image = listOfLives.transform.GetChild(listOfLives.transform.childCount - 1).gameObject;
        image.GetComponent<ImageModifier>().GlitchDestroy();
    }
    public void Initialize(int amount)
    {
        Image[] children = listOfLives.transform.GetComponentsInChildren<Image>();
        foreach(Image child in children)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Wait(amount));
    }
    private IEnumerator Wait(int amount)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < amount; i++)
        {
            Instance.CreateLife();
        }
    }

    public void ShowCrashScreen()
    {
        int mins = (int) (DataManager.time / 60);
        float sec = ((int)((DataManager.time % 60) * 100))/100f;
        
        string timeline = "";
        if(mins == 0)
        {
            timeline = sec +"s was simulated. \r\n";
        }
        else
        {
            timeline = mins + ":" + sec + "s was simulated. \r\n";
        }

        string display = string.Format("A fatal error has occured. \r\n\r\n" +
            "{0}" +
            "Your {1} goldfish will be saved \r\n" +
            "\r\n\r\nPress any key to continue...",
            timeline, DataManager.currency);
        crashText.text = display;
        Instance.crashWindow.SetActive(true);
    }
}
