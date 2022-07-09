using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1000)]
public class MenuUIManager : MonoBehaviour
{

    public GameObject menu1;
    public GameObject menu2;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void LoadMainMenu()
    {
        menu1.SetActive(true);
        menu2.SetActive(false);
    }

    public void LoadSubMenu()
    {
        menu1.SetActive(false);
        menu2.SetActive(true);
    }
}
