using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUi;
	public GameObject overlay;
	public GameObject time;
	private float gameTimeInSeconds = 0.0f;
	private float timeScale = 30f;

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

		if(!IsPaused){
			gameTimeInSeconds += Time.deltaTime * timeScale;

			int hour = Mathf.FloorToInt(gameTimeInSeconds / 3600);
			if(hour == 0){
				time.GetComponent<TextMeshProUGUI>().text = "12 AM";
			}
			else{
				time.GetComponent<TextMeshProUGUI>().text = hour + " AM";
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
