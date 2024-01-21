using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public string levelToLoad;
    public bool isUnlocked;
    public Sprite doorLocked;
    public Sprite doorUnlocked;

    //need this to change sprite
    private SpriteRenderer mySR;

    void Start()
    {
        //level 0 (1 when you make your own) should always be unlock
        //through playerprefs a 1 is unlocked and 0 is locked. could do with bool but don't have option with player prefs or something??
        PlayerPrefs.SetInt("Stage01", 1);
        mySR = GetComponent<SpriteRenderer>();

        if(PlayerPrefs.GetInt(levelToLoad) == 1)
        {
            isUnlocked = true;
        } else
        {
            isUnlocked = false;
        }

        if(isUnlocked)
        {
            mySR.sprite = doorUnlocked;
        } else
        {
            mySR.sprite = doorLocked;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("1");
        if (other.tag == "Player")
            Debug.Log(Input.GetButtonDown("Jump"));

        {
            if (Input.GetButtonDown("Jump") && isUnlocked)
            {
                Debug.Log("3");

                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
