using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool battleActive;
    public AudioSource bossMusic;
    public float timeBetweenSaws;
    public float timeUntilPlatforms;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform sawSpawnPoint;

    public GameObject spinSaw; //to know what we're dropping

    public GameObject boss; //to know the boss
    //want to kow what point the boss should be at. if at the right point do everything on the right side else do it on the left side
    public bool bossRight;
    public GameObject rightPlatforms;
    public GameObject leftPlatforms;

    public bool waitingForRespawn;

    public bool takeDamage;
    public int bossHealth;
    private int currentbossHealth;

    public GameObject victoryPlatform;

    //keeping track of time between as the fight goes on. when it reaches 0 new things happen
    private float sawDropTimer;
    private float platformAppearTimer;

    private CameraController theCamera;
    private LevelManager levelManager;
    private float storedSawDropTime; //keeping this so we can set it back to normal if we die
    private AudioSource ogMusic;

    // Start is called before the first frame update
    void Start()
    {
        sawDropTimer = timeBetweenSaws;
        platformAppearTimer = timeUntilPlatforms;

        //start boss on right side
        boss.transform.position = rightPoint.position;
        bossRight = true;
        currentbossHealth = bossHealth; //maxed
        theCamera = FindObjectOfType<CameraController>();
        levelManager = FindObjectOfType<LevelManager>();
        storedSawDropTime = sawDropTimer;
        ogMusic = levelManager.levelMusic;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.respawnActive)
        {
            battleActive = false;
            waitingForRespawn = true;
        }
        //setting everything to normal while we waitin to come back from the dead
        if (waitingForRespawn && !levelManager.respawnActive)
        {
            boss.SetActive(false);
            rightPlatforms.SetActive(false);
            leftPlatforms.SetActive(false);
            platformAppearTimer = timeUntilPlatforms;
            sawDropTimer = storedSawDropTime;
            boss.transform.position = rightPoint.position;
            bossRight = true;
            currentbossHealth = bossHealth;
            theCamera.following = true;
            waitingForRespawn = false;
            levelManager.levelMusic = ogMusic;
        }
        if (battleActive)
        {
            if (levelManager.levelMusic == ogMusic)
            {
                levelManager.levelMusic.Stop();
                levelManager.levelMusic = bossMusic;
                levelManager.levelMusic.Play();
            }
            
            //locking camera
            theCamera.following = false;
            theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, new Vector3(transform.position.x + 7, theCamera.transform.position.y, theCamera.transform.position.z), theCamera.slideInTime * Time.deltaTime);

            //boss should come to life and drop into game
            boss.SetActive(true);
            //timers start
            if (sawDropTimer > 0f)
            {
                sawDropTimer -= Time.deltaTime;
            } else
            {
                //want spawn point to move around but only change x position between left and right points
                //this needs a min and max to choose between
                sawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), sawSpawnPoint.position.y, sawSpawnPoint.position.z);

                //now that we have position we must spawn saw
                Instantiate(spinSaw, sawSpawnPoint.position, sawSpawnPoint.rotation);
                //now that saw is made we reset timer
                sawDropTimer = timeBetweenSaws;
            }

            //if boss is to the right we need the right platforms to appear (so player can jump right so platforms are actually on left)
            if (bossRight)
            {
                //start coutning down for platforms
                if(platformAppearTimer > 0f)
                {
                    platformAppearTimer -= Time.deltaTime;
                } else
                {
                    rightPlatforms.SetActive(true);
                }
            } else
            {
                //we're on the left side
                if (platformAppearTimer > 0f)
                {
                    platformAppearTimer -= Time.deltaTime;
                }
                else
                {
                    leftPlatforms.SetActive(true);
                }

            }

            if(takeDamage)
            {
                currentbossHealth -= 1;
                //want to know if we beat boss
                if (currentbossHealth <= 0)
                {
                    theCamera.following = true;
                    victoryPlatform.SetActive(true);
                    gameObject.SetActive(false);
                }

                //want to know if boss needs to move
                if(bossRight)
                {
                    boss.transform.position = leftPoint.position;
                } else
                {
                    boss.transform.position = rightPoint.position;
                }

                bossRight = !bossRight;
                rightPlatforms.SetActive(false);
                leftPlatforms.SetActive(false);
                platformAppearTimer = timeUntilPlatforms; //reset timer
                //saw drop rate increased
                timeBetweenSaws /= 2f;
                takeDamage = false;
            }
        } //end of if battle
    }

    //want to know when to start battle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            battleActive = true;
        }
    }
}
