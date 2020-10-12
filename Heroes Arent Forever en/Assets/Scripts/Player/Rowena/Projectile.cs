using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   public Vector2 velocity = new Vector2(0.0f, 0.0f);
   public GameObject player;   
   public GameObject explosionPrefab;	
   public static float iceDamage = 1.0f;
   public static float fireDamage = 1.0f;   
   public float realDamage = 1.0f;
   public Vector2 offset = new Vector2(0.0f, 0.0f);
   public float freeze = 1.0f;
   public bool triggerWall;
   public bool iceCanDamage = true;
   public bool canHitWall = true;
   float enemySpeed;   
   Vida enemyHealth;
   Enemies enemyOther;
   Boss1 Boss;

   void Start(){
      triggerWall = player.GetComponent<PlayerController>().triggerTopWall;
   }

   void Update(){
   	Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
   	Vector2 newPosition = currentPosition  + velocity * Time.deltaTime;

   	Debug.DrawLine(currentPosition + offset, newPosition + offset, Color.green, 30000000000);
      
   	RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);

      if(hits.Length == 0){
         Invoke("DestoyProjectile", 2);
      }else{
         CancelInvoke("DestoyProjectile");
         foreach(RaycastHit2D hit in hits){
            GameObject other = hit.collider.gameObject;
            if(other != player){               
               if(other.gameObject.name == "EnemyColliders"){
                  enemyHealth = other.transform.parent.gameObject.GetComponent<Vida>();
                  if(other.transform.parent.gameObject.GetComponent<Enemies>()!=null){
                     enemyOther = other.transform.parent.gameObject.GetComponent<Enemies>();
                  }else{
                     Boss = other.transform.parent.gameObject.GetComponent<Boss1>();
                  }
                  //realDamage = (iceDamage / enemyHealth.damageReduction);
                  realDamage = iceDamage;
                  if((other.transform.parent.gameObject.GetComponent<Enemies>()!=null && !enemyOther.dead) || (other.transform.parent.gameObject.GetComponent<Boss1>() != null && Boss.vida.currentHealth != 0)){
                     if(gameObject.CompareTag("Iceball")){
                        if(realDamage > 0 && iceCanDamage){
                           if(other.transform.parent.gameObject.GetComponent<Enemies>()!=null){
                              enemySpeed = enemyOther.speed;
                              enemyOther.speed = (enemyOther.speed / freeze);
                              Debug.Log("Enemy mv" + enemyOther.speed);
                           }
                           enemyHealth.TakeDamage(realDamage);    
                           canHitWall = false;
                           GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                           Destroy(explosion, 2.3f);
                           
                           Debug.Log("Damage Dealt: " + realDamage); 
                           Debug.Log(enemyHealth.currentHealth);
                           Debug.Log(other.name);                                
                           StartCoroutine(Freezing());  
                        }
                               
                          
                        IEnumerator Freezing() {
                           iceCanDamage = false;
                           gameObject.GetComponent<SpriteRenderer>().enabled = false;
                           yield return new WaitForSeconds(1);
                           iceCanDamage = true;
                           if(other.transform.parent.gameObject.GetComponent<Enemies>()!=null){
                              enemyOther.speed = enemySpeed;
                              Debug.Log("Enemy mv" + enemyOther.speed);
                           }
                           Destroy(gameObject);
                        }

                        }else{
                        realDamage = fireDamage;         
                        //realDamage = (fireDamage / enemyHealth.damageReduction);
                        enemyHealth.TakeDamage(realDamage);     
                        
                        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                        Destroy(gameObject); 
                        Destroy(explosion, 2.3f);
                        
                        Debug.Log("Damage Dealt: " + realDamage); 
                        Debug.Log(enemyHealth.currentHealth); 
                        Debug.Log(other.name);
                     }    
                  }                       
                  break;
               }
               if(other.CompareTag("TopWalls")){      
                  if(triggerWall && canHitWall){                           
                     GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                     Destroy(explosion, 2.3f);
                     Destroy(gameObject);
                     Debug.Log(other.name);  
                     break;                                   
                  }
               }
               if(canHitWall && (other.CompareTag("BottomWalls") || other.CompareTag("Obstacle"))){                       
                  GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                  Destroy(explosion, 2.3f);
                  Destroy(gameObject);
                  Debug.Log(other.name);  
                  break;                             
               }               
            }                 
         }
      }   	
   	transform.position = newPosition;      
   }
   void DestoyProjectile(){
      Destroy(gameObject);
   }
}
