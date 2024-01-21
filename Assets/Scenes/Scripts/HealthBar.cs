using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //want the fill image to adjust its property to make bar go up and down
    public Image health;

    //for extra collers do publicGradient healthColors;
    

    //want a method that allows us to set the amount of health we have on bar
    //we could have this adjust color too
    public void SetCurrentHealth(float amount)
    {
        health.fillAmount = amount;
        //health.color = healthColors.Evaluate(healthColors);
    }

    public void SetMaxHealth(float amount)
    {
        health.fillAmount = amount;
    }
}
