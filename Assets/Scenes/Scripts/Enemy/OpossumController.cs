using System.Collections;
using UnityEngine;

public class OpossumController : MonoBehaviour
{

    //we want var to control speed this enemy moves at
    public float moveSpeed;
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
        sign = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //want to see if we're allowed to start moving. if so we should move to player
        if (canBeginMoving)
        {
            //we always will be moving to left side of screen so moveSpeed should be negative
            //to move we change velocity through rigidbody
            //velocity is vector3 (x,y,z) -> (negative moveSpeed to move left, current velocity y, z not touched)
            myRB.velocity = new Vector3(-moveSpeed, myRB.velocity.y, 0f);
            transform.localScale = new Vector3(sign, 1f, 1f);
        }

     
    }

    //there is a built in method that we can call to know if we're visible in the scene or not
    void OnBecameVisible()
    {
        //we should be able to start moving
        canBeginMoving = true;
    }

    //should destroy ourselves if we fall of the edge to save memory
    //we will check to see if we've entered the killplane trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        //destroy method will remove object from game completely until game is restarted
        if (other.tag == "KillPlane")
        {
            gameObject.SetActive(false);
        }
        if (other.tag == "Wall")
        {
            moveSpeed = -moveSpeed;
            sign = -sign;
        }
    }
}
