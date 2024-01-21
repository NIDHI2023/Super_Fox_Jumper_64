using UnityEngine;

public class BounceOnEnemy : MonoBehaviour
{
    public GameObject deathAnim;
    public float bounceForce;
    public AudioSource enemyDieSound;

    private Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = transform.parent.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
            //for explosion animation
            Instantiate(deathAnim, other.transform.position, other.transform.rotation);

            playerRB.velocity = new Vector3(playerRB.velocity.x, bounceForce, 0f);
            enemyDieSound.Play();
        }

        //getComponent is resource intnese so don't use too much
        if (other.tag == "Boss")
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, bounceForce, 0f);
            //grab the Boss Script and change the take damage to hurt the boss
            other.transform.parent.GetComponent<Boss>().takeDamage = true;
        }
    }
}
