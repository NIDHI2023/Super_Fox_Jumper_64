using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Sprite crankUp;
    public Sprite crankDown;
    public bool isCheckpointActivated;

    private SpriteRenderer mySR;

    // Start is called before the first frame update
    void Start()
    {
        mySR = GetComponent<SpriteRenderer>();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") { }
        mySR.sprite = crankUp;
        isCheckpointActivated = true;
    }


}
