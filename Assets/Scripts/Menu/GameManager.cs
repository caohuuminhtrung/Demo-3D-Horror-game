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
	public GameObject winning;
	[SerializeField] GameObject enemy;
	[SerializeField] GameObject enemyPosition;
	[SerializeField] GameObject audioSource;
	private bool isEndGame = false;
	private float gameTimeInSeconds = 0.0f;
	private float timeScale = 30f;

	public static bool IsPaused = false;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && isEndGame)
		{
			if (!IsPaused && pauseMenuUi != null)
			{
				PauseGame();
			}
			else if (IsPaused && pauseMenuUi != null)
			{
				ResumeGame();
			}
		}

		if (!IsPaused)
		{
			DisplayTime();
		}

	}

	public void ResumeGame()
	{
		overlay.SetActive(true);
		Time.timeScale = 1.0f;
		Cursor.visible = false;
		IsPaused = false;
		pauseMenuUi.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}
	private void PauseGame()
	{
		overlay.SetActive(false);
		Time.timeScale = 0.0f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		IsPaused = true;
		pauseMenuUi.SetActive(true);
	}

	private void DisplayTime()
	{
		gameTimeInSeconds += Time.deltaTime * timeScale;

		int hour = Mathf.FloorToInt(gameTimeInSeconds / 3600);
		if (hour == 6)
		{
			WinGame();
		}
		if (hour == 0)
		{
			time.GetComponent<TextMeshProUGUI>().text = "12 AM";
		}
		else
		{
			time.GetComponent<TextMeshProUGUI>().text = hour + " AM";
		}
	}

	private void WinGame()
	{
		if (!isEndGame)
		{
			isEndGame = true;
			pauseMenuUi.SetActive(false);
			overlay.SetActive(false);
			enemy.SetActive(false);
			enemyPosition.SetActive(false);
			audioSource.SetActive(false);
			winning.SetActive(true);
			winning.GetComponent<WinningCutscene>().StartCutScene();
		}

	}
}
