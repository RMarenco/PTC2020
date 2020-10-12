using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escudo : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxShield = 50;
    public int currentShield;

    public shieldBar ShieldBar;

    void Start()
    {
        currentShield = maxShield;
        ShieldBar.SetMaxShield(maxShield);    
    }

    void Update()
    {

    }

    public void TakeDamage(){
        currentShield--;
        ShieldBar.setShield(currentShield);
    }

    public void recoverShield(int damage){
        
        currentShield += damage;
        ShieldBar.setShield(currentShield);
    }
}
