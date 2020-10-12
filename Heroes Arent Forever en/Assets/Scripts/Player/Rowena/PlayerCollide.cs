using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
	public PlayerOnTrigger playerOnTrigger;
	public PlayerController player = null;
	
    void OnTriggerEnter2D(Collider2D other){
    	if(!player.hit){
    		playerOnTrigger.PlayerTriggerEvent(other);	
    		Debug.Log("this is activated");
    	}
        
    }
}
