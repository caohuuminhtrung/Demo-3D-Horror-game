using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUi;
	public GameObject overlay;

	public static bool IsPaused = false;
	
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {	
		  	if (!IsPaused  && pauseMenuUi != null){
				overlay.SetActive(false);
				Time.timeScale = 0.0f;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				IsPaused = true;
				pauseMenuUi.SetActive(true);	
			}
        	else if(IsPaused && pauseMenuUi != null){
            	ResumeGame();
          	}
        }
	}

	public void ResumeGame(){
		overlay.SetActive(true);
		Time.timeScale = 1.0f;
		Cursor.visible = false;
		IsPaused = false;
		pauseMenuUi.SetActive(false);
	}

	public void ExitGame(){
		Application.Quit();
	}
	
	public void MainMenu(){
		SceneManager.LoadScene(0);
	}

	public void PlayGame(){
		SceneManager.LoadScene(1);
	}
}
