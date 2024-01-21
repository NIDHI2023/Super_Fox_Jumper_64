using System.Collections;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public int gemValue;
    private LevelManager levelManager;
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            levelManager.AddGems(gemValue);
            gameObject.SetActive(false);
        }
    }
}
