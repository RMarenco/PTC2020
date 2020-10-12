using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public GameObject BlackImage, mainMenu, optionsMenu, menuFirstButton, optionsFirstButton, optionsCloseButton, SelectCharacter, SelectCharacterFirstOption, title;

	public string[] roomNames;
	int myElements;

	void Awake(){
		Time.timeScale = 1f;
		PlayerController.currentHealth = 0;
		StartCoroutine(HideBlackImage());
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(menuFirstButton);		
	}

	void Update(){
		if(mainMenu.activeSelf && !EventSystem.current.currentSelectedGameObject == menuFirstButton){
			EventSystem.current.SetSelectedGameObject(menuFirstButton);
		}
		if(SelectCharacter.activeSelf && !EventSystem.current.currentSelectedGameObject == SelectCharacterFirstOption){
			EventSystem.current.SetSelectedGameObject(SelectCharacterFirstOption);		
		}
		if(optionsMenu.activeSelf && !EventSystem.current.currentSelectedGameObject == optionsFirstButton){
			EventSystem.current.SetSelectedGameObject(optionsFirstButton);	
		}
	}

	public void PlayGame(){		
		title.SetActive(false);
		SelectCharacter.SetActive(true);
		mainMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(SelectCharacterFirstOption);					
	}

	public void CloseCharacterSelect(){
		title.SetActive(true);
		SelectCharacter.SetActive(false);
		mainMenu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(menuFirstButton);					
	}

	public void Asus(){
		EventManager.SelectedCharacter = "Asus";
		myElements = Random.Range(0, roomNames.Length);
		SceneManager.LoadScene(roomNames[myElements]);
		Debug.Log("Play");
	}

	public void Rowena(){
		EventManager.SelectedCharacter = "Rowena";
		myElements = Random.Range(0, roomNames.Length);
		SceneManager.LoadScene(roomNames[myElements]);
		Debug.Log("Play");
	}

	public void QuitGame(){
		Debug.Log("Quit");
		Application.Quit();
	}

	public void OpenOptions(){
		title.SetActive(false);
		optionsMenu.SetActive(true);
		mainMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(optionsFirstButton);		
	}

	public void CloseOptions(){
		title.SetActive(true);
		optionsMenu.SetActive(false);
		mainMenu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(optionsCloseButton);	
	}

	IEnumerator HideBlackImage(){
		yield return new WaitForSeconds(1);
		BlackImage.SetActive(false);
	}
}