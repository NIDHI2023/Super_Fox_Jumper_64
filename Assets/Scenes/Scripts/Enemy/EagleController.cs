using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : MonoBehaviour
{
    public Transform leftPatrolPoint;
    public Transform rightPatrolPoint;
    public float moveSpeed;
    public bool movingRight;

    private Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if moving to the right. if moving right want to check our x pos against right patrol point x pos
        if (movingRight && transform.position.x > rightPatrolPoint.position.x)
        {
            movingRight = false;
        }

        if (!movingRight && transform.position.x < leftPatrolPoint.position.x)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            myRB.velocity = new Vector3(moveSpeed, myRB.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else
        {
            myRB.velocity = new Vector3(-moveSpeed, myRB.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
