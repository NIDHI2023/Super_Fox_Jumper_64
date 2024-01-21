using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowflake : MonoBehaviour
{
    public PlayerController player;
    public float windForce;
    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            Rigidbody2D p = player.GetComponent<Rigidbody2D>();

            p.AddForce(new Vector2(-windForce, 0f));
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //destroy method will remove object from game completely until game is restarted
        if (other.tag == "KillPlane" && gameObject.tag != "Player")
        {
            Destroy(gameObject);
            
        }

        if (other.tag == "Finish" && gameObject.tag == "Player")
        {

            Rigidbody2D p = player.GetComponent<Rigidbody2D>();

            windForce = 0;
        }
    }
}
