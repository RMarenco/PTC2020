using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType{
    	left, right, top, bottom
    }

    public DoorType doorType;

    private GameObject player;

    private float widthOffset = 12f;
    private float TopheightOffset = 10f;
    private float BottomheightOffset = 9.1f;

    public bool CanOpenDoor;

    public Animator anim;

    private void Start(){
    	player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update(){
        if(CanOpenDoor){
            anim.SetBool("EnemiesInRoom",CanOpenDoor);
        }else{
            anim.SetBool("EnemiesInRoom",CanOpenDoor);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
    	if(other.gameObject.name == "DoorColliders"){    		
    		switch(doorType){
    			case DoorType.bottom:
                    if(other.transform.parent.parent.gameObject.name == "Asus"){
                        player.transform.position = new Vector2(transform.position.x, transform.position.y - (BottomheightOffset + 0.2f));    
                    }else{
                        player.transform.position = new Vector2(transform.position.x, transform.position.y - BottomheightOffset);    
                    }    				                    
    				break;
    			case DoorType.left:
    				player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
    				break;
    			case DoorType.right:
    				player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
    				break;
    			case DoorType.top:
    				player.transform.position = new Vector2(transform.position.x, transform.position.y + TopheightOffset);
    				break;
    		}
    	}    	
    }
}
