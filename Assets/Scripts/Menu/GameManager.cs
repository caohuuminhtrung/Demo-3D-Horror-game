using System.Collections;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenuUi;
	public GameObject overlay;
	public GameObject time;
	public GameObject winning;
	[SerializeField] GameObject alertBox;
	[SerializeField] GameObject endingCamera;
	[SerializeField] GameObject enemy;
	[SerializeField] GameObject enemyPosition;
	[SerializeField] GameObject audioSource;
	[SerializeField] LosingCutscene losing;
	[SerializeField] EnemyController enemyController;
	[SerializeField] GameObject player;
	[SerializeField] GameController gameController;
	[SerializeField] VideoPlayer videoPlayer;
	[SerializeField] LoadManager loadManager;
	private bool isEndGame = false;
	private float gameTimeInSeconds = 0.0f;
	private float timeScale = 30;

	public static bool IsPaused = false;

	void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        if(scene.name == "SampleScene"){
			if(PlayerPrefs.GetInt("LoadGame") == 1){
				Debug.Log("Load Data");
				Data data = LoadManager.load.LoadData();
				if(data != null){
					Vector3 playerPos = new Vector3(
						data.PlayerPosX, 
						data.PlayerPosY, 
						data.PlayerPosZ);
					Vector3 playerRot = new Vector3(
						data.PlayerRotX, 
						data.PlayerRotY, 
						data.PlayerRotZ);
					player.transform.position = playerPos;
					player.transform.eulerAngles = playerRot;

					gameTimeInSeconds = data.Hour * 3600;
					gameController.stage = data.Stage;
					gameController.isLoadScene = true;
				}
			}
		}
    }

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
		if(pauseMenuUi != null){
			if(gameController.isLoadScene == true){
			SaveGame();
			}
		}

		Application.Quit();
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void PlayGame()
	{
		if(File.Exists(loadManager.Path())){
			File.Delete(loadManager.Path());
		}
		SceneManager.LoadScene(1);
	}

	public void NewGame(){
		if(File.Exists(loadManager.Path())){
			alertBox.SetActive(true);
		}
		else{
			PlayGame();
		}
	}

	public void LoadGame(){
		PlayerPrefs.SetInt("LoadGame", 1);
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

		Debug.Log("GameTimeInSecond: " + gameTimeInSeconds);
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

	private void SaveGame(){
		Data data = new Data();
		data.PlayerPosX = player.transform.position.x;
		data.PlayerPosY = player.transform.position.y;
		data.PlayerPosZ = player.transform.position.z;
		data.PlayerRotX = player.transform.rotation.eulerAngles.x;
		data.PlayerRotY = player.transform.rotation.eulerAngles.y;
		data.PlayerRotZ = player.transform.rotation.eulerAngles.z;
		data.Hour = Mathf.FloorToInt(gameTimeInSeconds / 3600);
		data.Stage = gameController.stage;

		LoadManager.load.SaveData(data);
		PlayerPrefs.DeleteKey("LoadGame");
	}

}
