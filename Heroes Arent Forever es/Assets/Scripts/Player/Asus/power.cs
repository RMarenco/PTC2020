using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class power : MonoBehaviour
{
    public int maxPower = 100;
    public static int currentPower;
    public powerBar PowerBar;

    void Start()
    {
        currentPower = 0;
        PowerBar.SetMaxPower(maxPower); 
        IncressPower(1); 
    }

    void Update()
    {
        if(currentPower > 100){
            currentPower = 99;
            IncressPower(1);
        }  
    }


    public void IncressPower(int power){
        currentPower += power;
        PowerBar.setPower(currentPower); 
    }

    public void DecressPower(){
        currentPower = 0;
        PowerBar.setPower(currentPower);
    }

}
