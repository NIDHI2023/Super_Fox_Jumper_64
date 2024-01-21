using System.Collections;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject newPoint;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //tells us when something is triggered, so it can fall through
    void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 targetPos = new Vector3(newPoint.transform.position.x + 2f, newPoint.transform.position.y, newPoint.transform.position.z);
        if (other.tag == "Player")
        {
            player.transform.position = targetPos;
        }
    }
}
