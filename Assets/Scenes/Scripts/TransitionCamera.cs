using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    public Transform switchPoint;
    public Transform newCenter;
    public GameObject oldBG;
    public GameObject newBG;
    public PlayerController player;
    private LevelManager level;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 0f, -10f);
        newBG.SetActive(false);

        level = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x >= switchPoint.position.x)
        {
            oldBG.SetActive(false);
            newBG.SetActive(true);
            transform.position = new Vector3(newCenter.position.x, newCenter.position.y, -10f);

        }
        
    }
}
