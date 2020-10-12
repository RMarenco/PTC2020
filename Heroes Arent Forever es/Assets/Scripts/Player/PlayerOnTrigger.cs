using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnTrigger : MonoBehaviour
{
    public PlayerController player;
    public GameObject OtherPowerUp;
    //public DamageEnemy attackBox;
    public power powerCharge;
    public GameObject EscudoBox = null;
    public escudo Escudo = null;
    
    public void PlayerTriggerEvent(Collider2D other){
        if(other.gameObject.name == "EnemyColliders"){
            if(!player.hit && PlayerController.playerHealth > 0){
                player.StartCoroutine(player.HitBoxOff());
                if(!EscudoBox || !EscudoBox.activeSelf){
                    PlayerController.currentHealth--;
                    player.healthBar.SetHealth(PlayerController.currentHealth);       
                }else{
                    Escudo.TakeDamage();
                }
                
                Debug.Log("player health after enemy: " + PlayerController.currentHealth); 
            }
        }

        if(other.gameObject.name == "EnemyBullet"){
            if(!player.hit && PlayerController.playerHealth > 0){
                player.StartCoroutine(player.HitBoxOff());
                PlayerController.currentHealth--;
                player.healthBar.SetHealth(PlayerController.currentHealth);   
                Debug.Log("player health after enemy: " + PlayerController.currentHealth); 
            }
        }        

        if(other.tag == "IceGem" || other.tag == "FireGem"){
            PlayerController.projectilePrefab = other.gameObject.GetComponent<Gems>().GemPower;
            if(GameObject.FindGameObjectWithTag("Boots")){
                OtherPowerUp = GameObject.FindGameObjectWithTag("Boots");
                OtherPowerUp.SetActive(false);
            }
            if(other.tag == "IceGem"){
                player.IceGemBox.SetActive(true);        
                player.FireGemBox.SetActive(false);
                if(PlayerController.gem != "ice"){
                    PlayerController.gem = "ice";    
                }else{
                    Projectile.iceDamage += 0.3f;                
                }                
            }
            if(other.tag == "FireGem"){
                player.IceGemBox.SetActive(false);
                player.FireGemBox.SetActive(true);
                if(PlayerController.gem != "fire"){
                    PlayerController.gem = "fire";
                }else{
                    Projectile.fireDamage += 0.4f;
                }                
            }                    
            player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
            Destroy(other.gameObject);
            Debug.Log(PlayerController.projectilePrefab);
        }  

        if(other.tag == "Heart"){
            if(PlayerController.currentHealth < PlayerController.playerHealth){
                player.skillBar.GetComponent<AudioEffectsController>().RestoreHealth();
                PlayerController.currentHealth++;
                player.healthBar.SetHealth(PlayerController.currentHealth);
                Destroy(other.gameObject);
            }            
        }

        if(other.tag == "Boots"){
            PlayerController.MOVEMENT_BASE_SPEED_STATIC += 0.5f;
            if(GameObject.FindGameObjectWithTag("FireGem")){
                OtherPowerUp = GameObject.FindGameObjectWithTag("FireGem");
                OtherPowerUp.SetActive(false);
            }else if(GameObject.FindGameObjectWithTag("IceGem")){
                OtherPowerUp = GameObject.FindGameObjectWithTag("IceGem");
                OtherPowerUp.SetActive(false);
            }
            player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
            Destroy(other.gameObject);
        }
        
        //Esto
        if(other.tag =="sword"){
            player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
            OtherPowerUp = GameObject.FindGameObjectWithTag("greenApple");
            OtherPowerUp.SetActive(false);
            DamageEnemy.damage += 0.5f;
            Destroy(other.gameObject);
        }
        if(other.tag == "greenApple"){
            OtherPowerUp = GameObject.FindGameObjectWithTag("sword");
            OtherPowerUp.SetActive(false);
            player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
            powerCharge.IncressPower(100);
            Destroy(other.gameObject);
        }
        if(other.tag == "lifeOrb"){            
            player.skillBar.GetComponent<AudioEffectsController>().RestoreHealth();
            PlayerController.currentHealth++;
            player.healthBar.SetHealth(PlayerController.currentHealth);
            Destroy(other.gameObject);
            if(PlayerController.currentHealth > PlayerController.playerHealth){
                PlayerController.currentHealth = PlayerController.playerHealth;
            }
        }
        if(other.tag == "powerOrb"){
            if(gameObject.name != "Asus"){
                player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
                StartCoroutine("boost");
                Destroy(other.gameObject);
            }else if(gameObject.name == "Asus"){
                player.skillBar.GetComponent<AudioEffectsController>().PowerUp();
                powerCharge.IncressPower(5);
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator boost(){
        PlayerController.MOVEMENT_BASE_SPEED_STATIC += 0.5f;
        yield return new WaitForSeconds(10f);
        PlayerController.MOVEMENT_BASE_SPEED_STATIC -= 0.5f;
    }
}
