using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
	[Header("Canvas")]
	public GameObject WorldName;	
    public Text Damage;
    public Text Speed;
    private GameObject player;    
    public GameObject Controls;
    public GameObject[] characters;
    public static string SelectedCharacter = null;
    public GameObject AsusSliders, AsusControls;
    public GameObject RowenaAbilities, RowenaControls;    

    void Awake(){
        if(SelectedCharacter == "Rowena"){
            RowenaControls.SetActive(true);
            characters[0].SetActive(true);
        }else if(SelectedCharacter == "Asus"){
            characters[1].SetActive(true);
            AsusControls.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {   
        if(characters[0].activeSelf){
            AsusSliders.SetActive(false);            
        }else{
            RowenaAbilities.SetActive(false);
        }
        /*#if UNITY_WEBGL
            Controls.SetActive(true);    
        #endif*/

        player = GameObject.FindGameObjectWithTag("Player");

        if(RoomController.instance.currentWorldName == "Cave"){
            WorldName.GetComponentsInChildren<Text>()[0].text = "Cave";    
        }else if(RoomController.instance.currentWorldName == "DeepCave"){
            WorldName.GetComponentsInChildren<Text>()[0].text = "Deep Caves";    
        }else if(RoomController.instance.currentWorldName == "Castle"){
            WorldName.GetComponentsInChildren<Text>()[0].text = "Castle";    
        }
        
        StartCoroutine(WorldNameOff());

        if(SelectedCharacter == "Rowena"){
            if(PlayerController.gem == "fire"){
                Damage.text = Projectile.fireDamage.ToString();
            }
            if(PlayerController.gem == "ice"){
                Damage.text = Projectile.iceDamage.ToString();
            }
        }else{
            Damage.text = DamageEnemy.damage.ToString();
        }
        
        Speed.text = PlayerController.MOVEMENT_BASE_SPEED_STATIC.ToString();        
    }

    // Update is called once per frame
    void Update()
    {
        if(player){
            if(SelectedCharacter == "Rowena"){
                if(PlayerController.gem == "fire"){
                    Damage.text = Projectile.fireDamage.ToString();
                }
                if(PlayerController.gem == "ice"){
                    Damage.text = Projectile.iceDamage.ToString();
                }
            }else{
                Damage.text = DamageEnemy.damage.ToString();
            }
        }
        Speed.text = PlayerController.MOVEMENT_BASE_SPEED_STATIC.ToString();
    }

    IEnumerator WorldNameOff(){        
        yield return new WaitForSeconds(3);
        WorldName.SetActive(false);
    }
}
