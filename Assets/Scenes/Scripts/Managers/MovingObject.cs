using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    //the obj we are moving
    public GameObject movingObject;
    //position of start and end points
    public Transform startPoint;
    public Transform endPoint;
    //how fast to move between thse two points
    public float moveSpeed;

    //want to move from start to end and then end to start w/o makign new points
    //get a vector that holds the position of current target pos we are moving to
    //when we get to this target we say new target is other point
    public Vector3 currentTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        //in begin we always move to end pos
        currentTargetPos = endPoint.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //always should bemoving towards currentTargetPos
        //MoveTowards allows us to consistently move in the same direction at a constant speed
        //args:(current pos, target we go towards, how long it should take for us to get there)
        movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, currentTargetPos, moveSpeed * Time.deltaTime);
        //telling platform to change current target when reaches
        if (movingObject.transform.position == endPoint.position)
        {
            currentTargetPos = startPoint.position;
        }
        if (movingObject.transform.position == startPoint.position)
        {
            currentTargetPos = endPoint.position;
        }
    }
}
