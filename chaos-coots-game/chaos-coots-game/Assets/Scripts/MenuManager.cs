using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    private bool isSettingsOpen = false;
    public int difficulty = 1;
    public void PlayEasyMode()
    {
        difficulty = 15;
        OpenControlMenu();
    }
    public void PlayNormalMode()
    {
        difficulty = 9;
        OpenControlMenu();
    }
    public void PlayHardMode()
    {
        difficulty = 3;
        OpenControlMenu();
    }

    public void PlayGame()
    {
        StartCoroutine(FinishWakingUp(difficulty));
    }
    private void OpenControlMenu()
    {
        CanvasReference.Instance.titleScreen.SetBool("Playing", true);
        CanvasReference.Instance.controlMenu.SetBool("Paused", true);
    }
    
    private IEnumerator FinishWakingUp(int amount)
    {
        CanvasReference.Instance.settingsMenu.SetBool("Paused", false);
        CanvasReference.Instance.controlMenu.SetBool("Paused", false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.player.GetComponent<Animator>().SetBool("ready", true);
        CanvasReference.Instance.Initialize(difficulty);
        CloseSettings();
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.player.health = amount;
        GameManager.Instance.playing = true;

        CanvasReference.Instance.titleScreen.gameObject.SetActive(false);
        CanvasReference.Instance.settingsMenu.gameObject.SetActive(false);
        CanvasReference.Instance.controlMenu.gameObject.SetActive(false);

    }

    public void UnPauseGame()
    {
        GameManager.Instance.PlayGame();
    }
    public void ToggleSettings()
    {
        if (isSettingsOpen)
        {
            CloseSettings();
        }
        else
        {
            OpenSettings();
        }
    }

    public void OpenSettings()
    {
        isSettingsOpen = true;
        CanvasReference.Instance.settingsMenu.SetBool("Paused", true);
    }
    public void CloseSettings()
    {
        isSettingsOpen = false;
        CanvasReference.Instance.settingsMenu.SetBool("Paused", false);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    
}
