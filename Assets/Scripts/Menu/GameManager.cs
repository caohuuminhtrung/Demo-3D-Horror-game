using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
	[SerializeField] LosingCutscene losing;
	[SerializeField] EnemyController enemyController;
	[SerializeField] GameObject player;
	private bool isEndGame = false;
	private float gameTimeInSeconds = 0.0f;
	private float timeScale = 30f;

	public static bool IsPaused = false;

	public void Update()
	{
		if(pauseMenuUi != null){
			if (Input.GetKeyDown(KeyCode.Escape) && !isEndGame)
			{
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

			if (enemyController.isLosingGame){
				LoseGame();
			}

			if(isEndGame){
				if(Input.GetKeyDown(KeyCode.Return)){
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

	private void LoseGame(){
		if(!isEndGame){
			isEndGame = true;
			ArrayList hints = new ArrayList();
			hints.Add("Remember to check the door.");
			hints.Add("Hold your door tight or it will get in.");
			hints.Add("It's outside the widow, look out.");
			hints.Add("Don't let it see you through the window. Hide!");
			hints.Add("Your closet is not always safe.");
			hints.Add("Hold you closet doors tight.");
			hints.Add("Watch out for its laugh.");

			
			pauseMenuUi.SetActive(false);
			overlay.SetActive(false);
			enemy.SetActive(false);
			enemyPosition.SetActive(false);
			audioSource.SetActive(false);
			player.SetActive(false);
			losing.GameObject().SetActive(true);
			
			if(enemyController.jumpscareAtDoor){
				losing.SetHintText(hints[Random.Range(0, 2)].ToString());
			}
			else if(enemyController.jumpscareAtWindow){
				losing.SetHintText(hints[Random.Range(2, 4)].ToString());
			}
			else{
				losing.SetHintText(hints[Random.Range(4, 7)].ToString());
			}

			losing.PlayEndingSong();
			losing.PlayFlickerLight();
		}
		
		
	}
}
