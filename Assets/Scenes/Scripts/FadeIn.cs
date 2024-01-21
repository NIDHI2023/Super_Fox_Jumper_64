using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeInTime;
    private Image fadeInScreen;
    // Start is called before the first frame update
    void Start()
    {
        fadeInScreen = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        fadeInScreen.CrossFadeAlpha(0f, fadeInTime, false);
        if (fadeInScreen.color.a == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
