using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{
    public PlayerController player;
    public GameObject snowflake;
    public float windForce;
    public float snowfallTime;
    public Transform snowSpawnPoint;

    private float ogtime;
    private void Start()
    {
        ogtime = snowfallTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.localScale == new Vector3(-1f,1f,1f))
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
       
        //s.AddForce(new Vector2(-windForce * 10, 0f));



        if (snowfallTime > 0f)
        {
            snowfallTime -= Time.deltaTime;
        } else
        {

                Vector3 snowPos = new Vector3(snowSpawnPoint.position.x, snowSpawnPoint.position.y + Random.Range(-5,5), snowSpawnPoint.position.z);
                GameObject newSnow = Instantiate(snowflake, snowPos, snowSpawnPoint.rotation);
                Rigidbody2D s = newSnow.GetComponent<Rigidbody2D>();
                s.velocity = new Vector3(-windForce, 0f, 0f);


            snowfallTime = ogtime;
        }


    }
}
