using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shieldBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxShield(int shield){
        slider.maxValue = shield;
        slider.value = shield;
    }

    public void setShield(int shield){
        slider.value = shield;
    }
}
