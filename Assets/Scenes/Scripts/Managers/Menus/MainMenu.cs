using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //need to know the varoius levels we could load into
    public string firstLevel;
    public string levelSelect;
    //want to find name of all potential levels and lock them back up when new game is started
    public string[] levelNames;

    //with buttons we can attach them to certain actions fairly easily
    //methods don't need to be that same name as the button because we make connections manually

    public void NewGame()
    {
        //take us to the first level of the game
        SceneManager.LoadScene(firstLevel);

        //locking all levels
        for (int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i], 0);
        }
        //gems n lives back to default
        PlayerPrefs.SetInt("GemAmount", 0);
        PlayerPrefs.SetInt("PlayerLives", 3);
    }

    public void Continue ()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        //we want to quit everything and go back to desktop, only works in built version of game
        Application.Quit();
    }
}
