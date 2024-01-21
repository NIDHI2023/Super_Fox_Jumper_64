using System.Collections;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private bool canSee;
    private Vector3 respawnPos;
    public float distance;
    public float yDist;
    public GameObject player;
    private Rigidbody2D myRB;


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        respawnPos = transform.position;
        myRB.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        canSee = (transform.position.x - player.transform.position.x) <= distance && (transform.position.y - player.transform.position.y) <= yDist;

        //want to see if we're allowed to start moving. if so we should move to player
        if (canSee) {
            myRB.WakeUp(); 
        } else {
            myRB.Sleep();
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.position = respawnPos;
            myRB.Sleep();
        }

        if (other.tag == "KillPlane")
        {
            Destroy(gameObject);
        }
    }
}
