using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject MenuPause;
    bool isPaused = false;

    public GameObject Button1;
    public GameObject Button2;

    private void OnPause(InputValue value)
    {
        if (isPaused == false)
        {
            MenuPause.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            MenuPause.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
    }

    public void RestartOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitOnClick()
    {
        Application.Quit();
    }
}
