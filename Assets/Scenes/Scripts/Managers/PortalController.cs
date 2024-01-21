using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public PlayerController player;
    public GameObject killPlaneTop;
    public GameObject killPlaneBot;
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("we in");

        //gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground World Objs";
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("we out");
        myRB.gravityScale = -myRB.gravityScale;
        
        if (myRB.gravityScale > 0)
        {
            killPlaneBot.SetActive(true);
            killPlaneTop.SetActive(false);
            Debug.Log("before" + player.transform.localScale);

            //player.transform.localScale = new Vector3(10f, 1f, 1f);
            Debug.Log("after" + player.transform.localScale);
        } else
        {
            killPlaneTop.SetActive(true);
            killPlaneBot.SetActive(false);
            //player.transform.localScale = new Vector3(player.transform.localScale.x, 1f, player.transform.localScale.z);
        }
    }
}
