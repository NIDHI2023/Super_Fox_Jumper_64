using System.Collections;
using UnityEngine;

public class BunnyController : MonoBehaviour
{
    //we want var to control speed this enemy moves at
    public float moveSpeed;
    public Transform leftPoint;
    public Transform rightPoint;
    //we want to know if we are allowed to start moving, as we only want to begin moving when we become visible on the screen
    private bool canBeginMoving;
    //in order to move we also need Rigidbody so we can get velocity
    private Rigidbody2D myRB;
    private float sign;
    
    // Start is called before the first frame update
    void Start()
    {
        //this is to grab our own RB so we can move ourselves later
        myRB = GetComponent<Rigidbody2D>();

        moveSpeed *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        //want to see if we're allowed to start moving. if so we should move to player
        if (canBeginMoving)
        {
            if (myRB.velocity == new Vector2(0f,0f))
            {
                myRB.velocity = new Vector3(moveSpeed, 0f, 0f);

            }
            Debug.Log("Our start speed" + myRB.velocity);

            
            if (transform.position.x <= leftPoint.transform.position.x)
            {
                Debug.Log("we can move right");
                myRB.velocity = new Vector3(-myRB.velocity.x, myRB.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
                Debug.Log("velocity" + myRB.velocity);

            }
            else if (transform.position.x >= rightPoint.transform.position.x)
            {
                Debug.Log("we can move");
                myRB.velocity = new Vector3(-myRB.velocity.x, myRB.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
                Debug.Log("velocity" + myRB.velocity);

            }
        }
        


    }

    //there is a built in method that we can call to know if we're visible in the scene or not
    void OnBecameVisible()
    {
        canBeginMoving = true;
    }

    //should destroy ourselves if we fall of the edge to save memory
    //we will check to see if we've entered the killplane trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        //destroy method will remove object from game completely until game is restarted
        if (other.tag == "KillPlane")
        {
            Destroy(gameObject);
        }
        if (other.tag == "Wall")
        {
            moveSpeed = -moveSpeed;
            sign = -sign;
        }
    }
}
