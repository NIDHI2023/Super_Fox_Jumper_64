using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardsPlayer : MonoBehaviour
{
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "KillPlane" && myRB.gravityScale > 0)
        {
            myRB.gravityScale = -myRB.gravityScale;
        }
    }
}
