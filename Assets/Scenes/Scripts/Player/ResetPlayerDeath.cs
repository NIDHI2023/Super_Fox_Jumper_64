using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerDeath : MonoBehaviour
{
    private Vector3 startPosition;
    //rotations aren't stored in Vector3, they're in Quaternions
    private Quaternion startRotation;
    private Vector3 startScale;

    private Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        if (GetComponent<Rigidbody2D>() != null)
        {
            myRB = GetComponent<Rigidbody2D>();
        }
    }

    public void ResetAfterDeath()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;

        if (myRB!= null)
        {
            //quick way to set this with all 0s
            myRB.velocity = Vector3.zero;
        }
    }
}
