using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    //want to have a a value that tellus us how much damage an object does to player
    public int damageToGive;

    //grab levelManager since that allows player to respawn
    private LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
     if (other.tag == "Player")
        {
            levelManager.DamagePlayer(damageToGive);
        }   
    }

}
