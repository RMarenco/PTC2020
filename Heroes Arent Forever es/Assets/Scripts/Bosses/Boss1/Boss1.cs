using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
{
	public GameObject BossHealth;
    public GameObject StageClear;
    public Vida vida;
    public string RoomName;
    public Animator animator;
    public Animator healthBarAnimator;
    public bool playerInRoom;    

    void Update()
    {
    	if(playerInRoom){
    		BossHealth.SetActive(true);
    		animator.SetBool("playerInRoom", true);
    	}
        if(vida.currentHealth <=0){
        	healthBarAnimator.SetTrigger("dead");
        	GameObject.Find(RoomName).GetComponent<Room>().enemiesInRoom.Remove(gameObject);
            animator.SetTrigger("Dead");            
            StartCoroutine(GoingNext());                                                 
        }
    }

    IEnumerator GoingNext(){       
        Destroy(gameObject,4); 
        StageClear.SetActive(true);
        Time.timeScale = 0;
        if(RoomController.instance.currentWorldName != "Castle"){
            yield return new WaitForSecondsRealtime(2);            
            if(RoomController.instance.currentWorldName == "Cave"){
                SceneManager.LoadScene("DeepCaveMain");                
            }if(RoomController.instance.currentWorldName == "DeepCave"){
                SceneManager.LoadScene("CastleMain");
            }
        }else{
            yield return new WaitForSecondsRealtime(3);            
            SceneManager.LoadScene("Menu");
        }
        
    }
}
