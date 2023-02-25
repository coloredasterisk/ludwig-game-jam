using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public void PlayEasyMode()
    {
        PlayGame(15);
    }
    public void PlayNormalMode()
    {
        PlayGame(9);
    }
    public void PlayHardMode()
    {
        PlayGame(3);
    }

    private void PlayGame(int amount)
    {
        GameManager.Instance.player.health = amount;
        CanvasReference.Instance.Initialize(amount);
        CanvasReference.Instance.titleScreen.SetBool("Playing", true);
        GameManager.Instance.playing = true;
    }
    public void OpenSettings()
    {

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
