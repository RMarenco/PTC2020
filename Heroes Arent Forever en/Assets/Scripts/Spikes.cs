using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{    
    public PlayerController player = null;

    void Start(){
    	player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "FeetCollider"){
            if(!player.hit && PlayerController.playerHealth > 0){
                player.StartCoroutine(player.HitBoxOff());
                PlayerController.currentHealth--;
                player.healthBar.SetHealth(PlayerController.currentHealth);    
                Debug.Log("player health after spike: " + PlayerController.currentHealth);                
            }
        }
    }
}
