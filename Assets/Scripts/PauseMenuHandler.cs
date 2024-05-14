using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseButton;
    public GameObject LevelSelection;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Resume();
            }
            else if (Time.timeScale == 1)
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;       
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        LevelSelection.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
