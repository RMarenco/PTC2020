using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarController : MonoBehaviour
{   
    [Header("SpecialAbility")]
    public Image specialboxCooldown;
    public Image specialCooldown;
    public float specialCooldownTime = 15;
    public bool specialIsCooldown;

    [Header("Gem")]    
    public Image GemboxCooldown; 
    public Image IceGemboxCooldown; 

    public float BASE_FIRE_GEM_COOLDOWN_TIME = 2;
    public float BASE_ICE_GEM_COOLDOWN_TIME = 3;
    public float GemCooldownTime;
    public bool GemIsCooldown = false;
    public string ActiveGem;
    public PlayerController playerController;

    void Update()
    {
        ActiveGem = PlayerController.gem;
        SpecialAbility();   
        Gem();        
    }

    void SpecialAbility(){
        //#if UNITY_STANDALONE_WIN
            if(Input.GetButtonUp("Fire2")){
                specialIsCooldown = true;
            }
        //#endif

        //#if UNITY_WEBGL
            /*if(playerController.SpecialAttackButton.Pressed){
                specialIsCooldown = true;
            }*/
        //#endif
        if(specialIsCooldown){
            specialboxCooldown.fillAmount += 1 / specialCooldownTime * Time.deltaTime;
            specialCooldown.fillAmount += 1 / specialCooldownTime * Time.deltaTime;

            if(specialboxCooldown.fillAmount >= 1){
                specialboxCooldown.fillAmount = 0;
                specialCooldown.fillAmount = 0;
                specialIsCooldown = false;
            }
        }
    }

    void Gem(){        
        if(ActiveGem == "fire"){
            GemCooldownTime = BASE_FIRE_GEM_COOLDOWN_TIME;
        }else if(ActiveGem == "ice"){
            GemCooldownTime = BASE_ICE_GEM_COOLDOWN_TIME;
        }
        if(GemIsCooldown){
            GemboxCooldown.fillAmount += 1 / GemCooldownTime * Time.deltaTime;
            IceGemboxCooldown.fillAmount += 1 / GemCooldownTime * Time.deltaTime;
            if(GemboxCooldown.fillAmount >= 1){
                GemboxCooldown.fillAmount = 0;
                IceGemboxCooldown.fillAmount = 0;               
                GemIsCooldown = false;
                Debug.Log("Able to fire");
            }
        }
    }

    IEnumerator SetCoolDown(){
        if(!GemIsCooldown){
            yield return new WaitForSecondsRealtime(0);
            GemIsCooldown = true;           
        }        
    }
}
