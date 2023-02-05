using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                CloseGame();
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                ReturnToMainMenu();
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameManager gm = GameObject.Find("gameManager").GetComponent<GameManager>();
                if (!gm.isGameRunning)
                {
                    ReturnToMainMenu();
                }
            }
        }
    }
}