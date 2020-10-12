using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage;

    void Start() {
           
    }
    
    void OnTriggerEnter2D(Collider2D other) {

         if(other.CompareTag("Player")){

            other.GetComponent<Vida>().TakeDamage(damage); 

         }

    }
}
