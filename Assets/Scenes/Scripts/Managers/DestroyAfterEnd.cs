using UnityEngine;

public class DestroyAfterEnd : MonoBehaviour
{
    public float lifetime;
    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
