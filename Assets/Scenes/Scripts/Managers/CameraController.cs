using UnityEngine;

public class CameraController : MonoBehaviour
{
    //want to tell the camera what it should be following
    //game object can be anything in the scene, so you could follow player or an enemy
    public GameObject target;


    public float aheadDistance;

    public float aboveDistance;

    public float slideInTime;

    private Vector3 targetPos;

    public bool following;


    // Start is called before the first frame update
    void Start()
    {
        following = true;
    }

   
    // Update is called once per frame
    void FixedUpdate()
    {
        if (following)
        {
            //transform refers to camera transform specifically
            targetPos = new Vector3(target.transform.position.x, target.transform.position.y + aboveDistance, transform.position.z);

            if (target.transform.parent.CompareTag("MovingPlatformV"))
            {
                targetPos = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            }
            //the z value for camera shouldn't be 0, should be -10 cause it's looking back at the plane of the screen
            //you can change planes to have foregrounds and backgrounds and mess with the player

            if (target.transform.localScale.x > 0f)
            {
                targetPos = new Vector3(targetPos.x + aheadDistance, targetPos.y, targetPos.z);

                //if (target.transform.localScale.y > 0f)
                //{
                //    targetPos = new Vector3(targetPos.x + aheadDistance, targetPos.y + aheadYDistance, targetPos.z);
                //} else
                //{
                //    targetPos = new Vector3(targetPos.x + aheadDistance, targetPos.y - aheadYDistance, targetPos.z);
                //}

            }
            else if (target.transform.localScale.x < 0f)
            {
                //if (target.transform.localScale.y > 0f)
                //{
                //    targetPos = new Vector3(targetPos.x - aheadDistance, targetPos.y + aheadYDistance, targetPos.z);
                //}
                //else
                //{
                //    targetPos = new Vector3(targetPos.x - aheadDistance, targetPos.y - aheadYDistance, targetPos.z);
                //}

                targetPos = new Vector3(targetPos.x - aheadDistance, targetPos.y, targetPos.z);
            }

            transform.position = Vector3.Lerp(transform.position, targetPos, slideInTime * Time.deltaTime);
            //Time.deltaTime is how long it takes from one frame to go to another. This is a way to normalize movement on different computers
            //Lerp(current vector, new vector to move to, time it takes to move)
        }
    }
}
