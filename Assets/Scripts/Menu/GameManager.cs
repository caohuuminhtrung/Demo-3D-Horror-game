using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenuUi;
	public GameObject overlay;
	public GameObject time;
	public GameObject winning;
	[SerializeField] GameObject endingCamera;
	[SerializeField] GameObject enemy;
	[SerializeField] GameObject enemyPosition;
	[SerializeField] GameObject audioSource;
	[SerializeField] LosingCutscene losing;
	[SerializeField] EnemyController enemyController;
	[SerializeField] GameObject player;
	[SerializeField] GameController gameController;
	[SerializeField] VideoPlayer videoPlayer;
	private bool isEndGame = false;
	private float gameTimeInSeconds = 0.0f;
	private float timeScale = 30;

	public static bool IsPaused = false;

	public void Update()
	{
		if (pauseMenuUi != null)
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !isEndGame)
			{
				Debug.Log("ESC Pressed");
				if (!IsPaused)
				{
					PauseGame();
				}
				else if (IsPaused)
				{
					ResumeGame();
				}
			}

			if (!IsPaused)
			{
				DisplayTime();
			}

			if (enemyController.isLosingGame)
			{
				LoseGame();
			}

			if (isEndGame)
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					MainMenu();
				}
			}
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
		if (gameController.stage < 5)
		{
			time.GetComponent<TextMeshProUGUI>().text = "10 PM";
		}
		else
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
	}

	private void WinGame()
	{
		if (!isEndGame)
		{
			isEndGame = true;

			winning.SetActive(true);
			winning.GetComponent<WinningCutscene>().StartCutScene();
			pauseMenuUi.SetActive(false);
			overlay.SetActive(false);
			enemy.SetActive(false);
			enemyPosition.SetActive(false);
			audioSource.SetActive(false);

		}

	}

	private void LoseGame()
	{
		if (!isEndGame)
		{
			isEndGame = true;

			videoPlayer.GameObject().SetActive(true);
			videoPlayer.Play();
			pauseMenuUi.SetActive(false);
			overlay.SetActive(false);
			enemy.SetActive(false);
			enemyPosition.SetActive(false);
			audioSource.SetActive(false);
			player.SetActive(false);

			StartCoroutine(StartLosingScene());
		}
	}

	IEnumerator StartLosingScene()
	{
		yield return new WaitForSeconds(3f);
		videoPlayer.GameObject().SetActive(false);
		ArrayList hints = new ArrayList
		{
			"Remember to check the door.",
			"Hold your door tight or it will get in.",
			"It's outside the widow, look out.",
			"Don't let it see you through the window. Hide!",
			"Your closet is not always safe.",
			"Hold you closet doors tight.",
			"Watch out for its laugh."
		};

		if (enemyController.jumpscareAtDoor)
		{
			losing.SetHintText(hints[Random.Range(0, 2)].ToString());
		}
		else if (enemyController.jumpscareAtWindow)
		{
			losing.SetHintText(hints[Random.Range(2, 4)].ToString());
		}
		else if (enemyController.jumpscareAtCloset)
		{
			losing.SetHintText(hints[Random.Range(4, 7)].ToString());
		}

		losing.GameObject().SetActive(true);
		losing.PlayEndingSong();
		losing.PlayEndingScene();

	}
}
