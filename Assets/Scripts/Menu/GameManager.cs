using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUi;

	public static bool IsPaused = false;
	
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {	
			Debug.Log("123");
		  	if (!IsPaused && pauseMenuUi != null){
				Debug.Log("abc");
				Time.timeScale = 0.0f;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				IsPaused = false;
				pauseMenuUi.SetActive(true);	
			}
        	else if(IsPaused && pauseMenuUi != null){
            	ResumeGame();
          	}
        }
	}

	public void ResumeGame(){
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
