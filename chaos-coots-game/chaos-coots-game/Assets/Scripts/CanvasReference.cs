using System;
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
    public GameObject endWindow;
    public TextMeshProUGUI endWindowText;

    public GameObject pausedMenu;
    public Animator settingsMenu;
    public Animator controlMenu;
     
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
        yield return new WaitForSecondsRealtime(0.01f);
        for (int i = 0; i < amount; i++)
        {
            Instance.CreateLife();
        }
    }

    

    public void ShowCrashScreen()
    {

        string strTime = convertToTime(DataManager.time) + "s was simulated. \r\n";

        string display = string.Format("A fatal error has occured. \r\n\r\n" +
            "{0}" +
            "Your {1} goldfish will be saved \r\n" +
            "\r\n\r\nPress any key to continue...",
            strTime, DataManager.currency);
        crashText.text = display;
        Instance.crashWindow.SetActive(true);
    }

    public static string convertToTime(float time)
    {
        int mins = (int)(time / 60);
        double sec = Math.Round(time % 60, 2);

        string timeline = "";
        if (mins == 0)
        {
            timeline = "" + sec;
        }
        else
        {
            if (sec < 10)
            {
                timeline = mins + ":0" + sec;
            }
            else
            {
                timeline = mins + ":" + sec;
            }

        }
        return timeline;
    }
}
