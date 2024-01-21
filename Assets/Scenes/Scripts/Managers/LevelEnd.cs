using System.Collections;
//specific library to change scenes
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    //tells what Scene to move to next
    public string levelNameToLoad;
    public string levelNameToUnlock;
    public int neededGems;
    public float whenToMove;
    public float whenToLoad;
    public bool movePlayer;
    public GameObject houseEnd;

    private PlayerController player;
    private CameraController theCamera;
    private LevelManager level;
    private bool alreadyPlayed;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
        level = FindObjectOfType<LevelManager>();
        alreadyPlayed = false;
    }

    public void UpdateGems(int current)
    {
        //want to see if we're allowed to start moving. if so we should move to player
        if (current >= neededGems)
        {
                gameObject.SetActive(true);
            
        }
        else
        {
                gameObject.SetActive(false);
        }
    }


    //want to know when trigger has been entered
    void OnTriggerEnter2D(Collider2D other)
    {
        //want to test it was player who entered
        if (other.tag=="Player")
        {
            StartCoroutine("LevelEndCoroutine");
        }
    }

    public IEnumerator LevelEndCoroutine ()
    {
        Debug.Log("1");
        player.canMove = false;
        if (theCamera != null)
        {
            theCamera.following = false;
            theCamera.transform.position = new Vector3(houseEnd.transform.position.x, houseEnd.transform.position.y, theCamera.transform.position.z);
        }
        level.invincibilityFrames = true;

        player.myRB.velocity = Vector3.zero;

        level.levelMusic.Stop();
        level.victoryMusic.Play();
        if (!alreadyPlayed)
        {
            level.victoryMusic.Play();
            alreadyPlayed = true;
        }

        //player prefs are a way to store info in the game that's permantently available, even when we stop game and run again
        //below values are stored locally on the device
        PlayerPrefs.SetInt("GemAmount", level.gemAmount);
        PlayerPrefs.SetInt("PlayerLives", level.currentLivesCount);

        //unlock next level in next select world
        PlayerPrefs.SetInt(levelNameToUnlock, 1);
        Debug.Log("2");


        yield return new WaitForSeconds(whenToMove);
        movePlayer = true;
        Debug.Log("3");

        player.transform.SetParent(player.transform); 
        player.myRB.velocity = new Vector3(player.moveSpeed, 0f, 0f);
        Debug.Log("4");


        //Debug.Log("stop music");
        //level.victoryMusic.Stop();
        //if(!level.endLevelMusic.isPlaying)
        //{
        //    level.endLevelMusic.Play();
        //}

        yield return new WaitForSeconds(level.victoryMusic.clip.length - 0.5f); //we will modify this

        houseEnd.GetComponent<SpriteRenderer>().sortingOrder = 1;
        yield return new WaitForSeconds(whenToLoad);


        //load up next level with sceneMangaer
        SceneManager.LoadScene(levelNameToLoad);

    }
}
