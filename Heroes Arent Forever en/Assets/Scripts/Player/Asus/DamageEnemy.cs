using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public static float damage = 1.0f;
    Vida enemyHealth;
	void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "EnemyColliders")
        {   
        	if(other.transform.parent.gameObject.GetComponent<Vida>()!=null){
        		enemyHealth = other.transform.parent.gameObject.GetComponent<Vida>();
	        	Debug.Log("Enemy name" + other.transform.parent.gameObject.name);     	
	            enemyHealth.TakeDamage(damage);
	            Debug.Log("Enemy Hit" + enemyHealth.currentHealth);	
        	}
        	
        }
    }
}
