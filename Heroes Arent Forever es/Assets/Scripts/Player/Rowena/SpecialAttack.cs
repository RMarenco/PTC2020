using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public GameObject player;	    
	public Vector2 velocity = new Vector2(0.0f, 0.0f);
	public float damage = 1.0f;
	public GameObject explosionPrefab;
	Vida enemyScript;
	Enemies enemyOther;
	Boss1 Boss;

    void Update()
    {   
    	Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
   		Vector2 newPosition = currentPosition  + velocity * Time.deltaTime;             

     	Debug.DrawLine(currentPosition, newPosition, Color.green, 30000000000);
   		RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, newPosition);

   		if(hits.Length == 0){
   			Invoke("DestroyFireRing", 2);
   		}else{
   			CancelInvoke("DestoyProjectile");
	        foreach(RaycastHit2D hit in hits){
	        	GameObject other = hit.collider.gameObject;
	        	if(other != player){
	        		if(other.gameObject.name == "EnemyColliders"){						
                     	enemyScript = other.transform.parent.gameObject.GetComponent<Vida>();
                     	if(other.transform.parent.gameObject.GetComponent<Enemies>()!=null){
	                     	enemyOther = other.transform.parent.gameObject.GetComponent<Enemies>();
	                  	}else{
	                    	Boss = other.transform.parent.gameObject.GetComponent<Boss1>();
	                  	}
                     	if((other.transform.parent.gameObject.GetComponent<Enemies>()!=null && !enemyOther.dead)){
	                     	enemyScript.TakeDamage(damage);
							Debug.Log(enemyScript.currentHealth);    
							GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			               	explosion.GetComponent<AudioSource>().volume = 0.2f;
			               	Destroy(explosion, 2.3f);
			               	Destroy(gameObject);
			            	Debug.Log(other.name);
                     	}                  							
                     	if(other.transform.parent.gameObject.GetComponent<Boss1>() != null){
                     		enemyScript.TakeDamage(damage);
							Debug.Log(enemyScript.currentHealth);    
							GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			               	explosion.GetComponent<AudioSource>().volume = 0.2f;
			               	Destroy(explosion, 2.3f);
			               	Destroy(gameObject);
			            	Debug.Log(other.name);
                     	}
						break;
					}
	        		if(other.CompareTag("TopWalls") || other.CompareTag("BottomWalls") || other.CompareTag("Obstacle")){
                    	Destroy(gameObject);                    	
                    	GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		               	explosion.GetComponent<AudioSource>().volume = 0.2f;
		               	Destroy(explosion, 2.3f);	
		            	Debug.Log(other.name); 
		            	break;
	               	}	              
	        	}
	        }
   		}

   		transform.position = newPosition; 
    }    

    void DestroyFireRing(){
      Destroy(gameObject);
   } 
}