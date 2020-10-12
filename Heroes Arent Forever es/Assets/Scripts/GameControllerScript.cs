using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameControllerScript : MonoBehaviour
{    
    public GameObject pauseMenu, continueBtn, gameOver, gameOverMenuBtn;
    public static bool isPaused;
    public string SceneName;
    public Image Blades;
    
    // Start is called before the first frame update    
    void Start(){
        EventSystem.current.SetSelectedGameObject(null);
    }
    // Update is called once per frame
    void Update()
    {        
        if(!gameOver.activeSelf && Blades.GetComponent<Animator>().enabled == false && Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                EventSystem.current.SetSelectedGameObject(continueBtn);
                resumeGame();
            }else{                
                PauseGame();
            }
        }

        if(gameOver.activeSelf){
            EventSystem.current.SetSelectedGameObject(gameOverMenuBtn);
        }
    }

    public void PauseGame(){

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;        
        EventSystem.current.SetSelectedGameObject(continueBtn);
        
    }

    public void resumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;      
    }

    public void GoToMainMenu(){        
        SceneManager.LoadScene(SceneName);
    }
}