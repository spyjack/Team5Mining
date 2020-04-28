using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject helpPanel;
    public GameObject creditsPanel;
    public GameObject loadingScreen;
    [Header("Scene Info")]
    public string nextSceneName;

    public void OnClickStart()
    {
        loadingScreen.gameObject.SetActive(true);
        SceneManager.LoadScene(nextSceneName);
    }
    public void OnClickQuit()
    {
        Application.Quit();
        print("Application is trying to close");
    }
    public void OnClickHelp()
    {
        helpPanel.gameObject.SetActive(true);
    }
    public void OnClickCredits()
    {
        creditsPanel.gameObject.SetActive(true);
    }
    public void OnClickHelpQuit()
    {
        helpPanel.gameObject.SetActive(false);
    }
    public void OnClickCreditsQuit()
    {
        creditsPanel.gameObject.SetActive(false);
    }
}
