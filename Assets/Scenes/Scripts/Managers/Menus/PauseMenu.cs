using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;
    public GameObject pauseScreen;
    public GameObject pauseScreenButton;

    private PlayerController player;
    private LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if (Time.timeScale == 0)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;

        //if player is in air they fall straight down no momentum
        player.canMove = true;
        levelManager.levelMusic.UnPause();
    }

    public void LevelSelectLoad()
    {
        PlayerPrefs.SetInt("GemAmount", levelManager.gemAmount);
        PlayerPrefs.SetInt("PlayerLives", levelManager.currentLivesCount);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
    }

    public void MainMenuLoad()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }

    public void PauseGame()
    {
        //get current event system and make current object the pause screen button
        EventSystem.current.SetSelectedGameObject(pauseScreenButton);
        //easiest way to freeze when puased is change Time scale of game to 0
        Time.timeScale = 0;

        pauseScreen.SetActive(true);
        player.canMove = false;
        levelManager.levelMusic.Pause();

    }
}
