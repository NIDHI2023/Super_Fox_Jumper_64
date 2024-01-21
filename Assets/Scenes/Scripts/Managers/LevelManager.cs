using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//for List
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    //want a timer to know when to respawn and object that we will respawn
    public float respawnCountdown;
    public GameObject deathExplosion;
    public int gemAmount;
    public TextMeshProUGUI gemText;

    public bool respawnActive;

    public float maxHealth;
    public float currentHealth;

    public int startLivesCount;
    public int currentLivesCount;
    public TextMeshProUGUI livesText;
    //Images might be helpful later

    public GameObject gameOver;
    public Sprite upIcon;
    public Sprite crouchIcon;
    public Image livesIcon;
    public Image healthIcon;

    //want a threshold for gems to give us an extra life
    public int bonusLifeThreshold;
    //want ahidden counter to keep track if threshold is reached
    private int gemExtraLifeCounter;

    public AudioSource gemSound;
    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public AudioSource endLevelMusic;
    public AudioSource victoryMusic;

    //want to keep track if we're invincible. don't keep taking dmaage after being knocked back
    public bool invincibilityFrames;

    //want the object that we will respawn
    private PlayerController player;
    private GameObject killPlane;

    //want to know if we should actually respawn or not
    private bool canRespawn;
    private HealthBar healthBar;

    //public LevelEnd end;

    //want list of objects that have reset script
    private List<ResetPlayerDeath> objectsResetting;

    private PauseMenu pauseMenu;
    

    // Start is called before the first frame update
    void Start()
    {
        //want to see if Playerprefs has any vals. first time we start game PlayerPrefs isn't called
        //once we beat one level it has been called and from then on it will have been called
        //HasKey lets us know if it has been called or not
        if (PlayerPrefs.HasKey("GemAmount"))
        {
            gemAmount = PlayerPrefs.GetInt("GemAmount");
        } else
        {
            gemAmount = 0;
        }
        if (PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLivesCount = PlayerPrefs.GetInt("PlayerLives");
        }
        else
        {
            currentLivesCount = startLivesCount;
        }


        pauseMenu = FindObjectOfType<PauseMenu>();
        pauseMenu.gameObject.SetActive(true);
        //can't use normal GetComponent cause that only grabs Components off the object our script is attached to
        //we're actually trying to interact with a different object here
        //FindObjectOfType will find us object with whatever we place between the <> attached to that object
        player = FindObjectOfType<PlayerController>();
        killPlane = GameObject.FindGameObjectWithTag("KillPlane");
        gemText.text = "Gems: " + gemAmount;

        currentHealth = maxHealth;
        //get health bar
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        canRespawn = true;

        //want to fill the List at the start of the world
        //the FindObjects returns an array so we need that and then convert that to a list
        objectsResetting = FindObjectsOfType<ResetPlayerDeath>().ToList();

        livesText.text = "x: " + currentLivesCount;
    }

    //want to always check health once per frame
    void Update()
    {
        if (currentHealth < 50)
        {
            healthIcon.sprite = crouchIcon;
        } else
        {
            healthIcon.sprite = upIcon;
        }
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (gemExtraLifeCounter >= bonusLifeThreshold)
        {
            //give extra life
            currentLivesCount++;
            livesText.text = "x: " + currentLivesCount;

            //subtract from hidden counter, don't reset to 0 in case there's overflow from picking up a x3 gem or something
            gemExtraLifeCounter -= bonusLifeThreshold;
        }
        if (canRespawn)
        {
            currentLivesCount -= 1;
            livesText.text = "x: " + currentLivesCount;

            if (currentLivesCount > 0)
            {
                canRespawn = false;
                if (currentLivesCount <= 2) {
                livesIcon.sprite = crouchIcon;
                } else
                {
                    livesIcon.sprite = upIcon;
                }

                //must call coroutines to start
                StartCoroutine("RespawnCoroutine");
            } else
            {
                player.gameObject.SetActive(false);
                livesText.text = "x: 0";
                gameOver.SetActive(true);

                levelMusic.Stop();
                Debug.Log("music hould stop");
                gameOverMusic.Play();
            } 
        }
    }

    //Coroutines are ways to do thiings in a separate timeline from the game world
    //we have an IEnumerator return type which gives us support for simple interation over non generic collections. we can read data in a collection but no modifying it
    //we have a "yield return" statement which means a pause happens and resumes on next frame
    public IEnumerator RespawnCoroutine ()
    {
        respawnActive = true;
        //we're specifcally working with PlayerController component so if we want to impact entire player object we need to say gameObject so unity knows what we mean
        //SetActive means is the object curently active or not in the scene
        player.gameObject.SetActive(false);
        killPlane.SetActive(false);

        //we can force an object to get created in the scene. what obj, where to copy it, and what rotation
        Instantiate(deathExplosion, player.transform.position, player.transform.rotation);


        //before doing movement/reactication we have two options
        //move player and then wait before reactivating OR
        // wait and then move and reactivate
        //either way is fiine, here is yield return
        //hits this line of code, pauses for how much we say, then runs on next frame after timer goes off
        yield return new WaitForSeconds(respawnCountdown);

        respawnActive = false;

        //before becoming active must set health back to full
        currentHealth = maxHealth;
        canRespawn = true;
        healthBar.SetCurrentHealth(currentHealth / maxHealth);

        //we want to move the playe to the respawn pos
        player.transform.position = player.respawnPos;

        //walking through the list one by one and tell everything to become activea gain at the right spot
        foreach (ResetPlayerDeath obj in objectsResetting)
        {
            //make obj active and the move them to position we need to make active first before moving them. when they're not active can't use script methods
            obj.gameObject.SetActive(true);
            obj.ResetAfterDeath();
        }

        //gemAmount = 0;
        gemExtraLifeCounter = 0;
        gemText.text = "Gems: " + gemAmount;

        //we now can reactivate ourselves since we have been moved to the correct position
        player.gameObject.SetActive(true);

        Camera.main.transform.position = new Vector3(player.transform.position.x, 0f, Camera.main.transform.position.z);

        yield return new WaitForSeconds(1);
        killPlane.SetActive(true);

    }


    public void AddGems(int amountOfGems)
    {
        gemAmount += amountOfGems;
        if (gemAmount % 10 == 0)
        {
            currentLivesCount++;
        }
        gemExtraLifeCounter += gemAmount;
        //end.UpdateGems(gemAmount);
        gemText.text = "Gems: " + gemAmount;

        gemSound.Play();
    }

    //want to know what happens to our health when we take some damage, need to know how much damage to dish out
    public void DamagePlayer(float dmg)
    {
        if (!invincibilityFrames) {
            currentHealth -= dmg;
            healthBar.SetCurrentHealth(currentHealth / maxHealth);
            player.Knockback();
            StartCoroutine("IFramesFlashCoroutine");

            player.hurtSound.Play();
        }

        
    }
    public IEnumerator IFramesFlashCoroutine()
    {
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        Color ogColor = playerSprite.color;

        for (int i = 0; i < 5; i++)
        {
            playerSprite.color = Color.Lerp(playerSprite.color,Color.red, 1f);
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = Color.Lerp(playerSprite.color, ogColor, 1f);
            yield return new WaitForSeconds(0.2f);
        }
        

    }

        public void RegenHealth(float amt)
    {
        currentHealth += amt;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth / maxHealth);
    }

    public void GiveHealth (int healthToGive)
    {
        currentHealth += healthToGive;
        //can't give too much health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth/maxHealth);

    }

    public void AddLife(int lives)
    {
        currentLivesCount += lives;
        livesText.text = "x: " + currentLivesCount;

        gemSound.Play();
    }
}
