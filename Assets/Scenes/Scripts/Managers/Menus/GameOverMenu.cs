using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;
    private LevelManager levelManager;

    private PauseMenu pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        pauseMenu.gameObject.SetActive(false);
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void RestartLevel()
    {
        //reset vals
        PlayerPrefs.SetInt("GemAmount", 0);
        PlayerPrefs.SetInt("PlayerLives", levelManager.startLivesCount);
        //don't want to load player to checkpoint, must go to beginning
        //we can do this by loading scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelSelectLoad()
    {
        PlayerPrefs.SetInt("GemAmount", 0);
        PlayerPrefs.SetInt("PlayerLives", levelManager.startLivesCount);
        SceneManager.LoadScene(levelSelect);
    }

    public void MainMenuLoad()
    {
        SceneManager.LoadScene(mainMenu);
        PlayerPrefs.SetInt("GemAmount", 0);
        PlayerPrefs.SetInt("PlayerLives", levelManager.startLivesCount);
    }
}
