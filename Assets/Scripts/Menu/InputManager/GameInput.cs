using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
    [SerializeField] private string fileName = "input.settings";
	[SerializeField] private InputComponent[] input;
    [SerializeField] private Slider slider;

	private static GameInput gameInput;
    private float mouseSens;

    private void Awake() {
        gameInput = this;
		GameInput.Key.LoadSettings();
    }
    public float Sensitivity{
        get { return this.mouseSens; }
    }

	public static GameInput Key
	{
		get
        {
            if (gameInput == null)
            {
                Debug.LogError("GameInput là null. Nhớ check thứ tự hay đã khởi tạo chưa nha. T ngồi sửa cái quỷ này 1 tiếng hơn.");
            }
            return gameInput;
        }
	}


	private KeyCode FindKey(string name){
		for(int i = 0; i < input.Length; i++)
		{
			if(name == input[i].DefaultKeyName) return input[i].keyCode;
		}

		return KeyCode.None;
	}

	private int GetInt(string text)
	{
		int value;
		if(int.TryParse(text, out value)) return value;
		return 0;
	}

	private string Path(){
		return Application.dataPath + "/" + fileName;
	}

	private void SetKey(string value){
		string[] result = value.Split(new char[]{'='});

		for(int i = 0; i < input.Length; i++)
		{
			if(result[0] == input[i].DefaultKeyName)
			{
				input[i].keyCode = (KeyCode) GetInt(result[1]);
				input[i].ButtonText.text = input[i].keyCode.ToString();
			}
		}
	}

    private void SetSens(string value){
        string[] result = value.Split(new char[]{'='});
        mouseSens = float.Parse(result[1]);
    }

	public void DefaultSettings(){
		for(int i = 0; i < input.Length; i++){
			input[i].keyCode = input[i].DefaultKeyCode;
			input[i].ButtonText.text = input[i].DefaultKeyCode.ToString();
		}

        mouseSens = 1.0f;
        slider.value = mouseSens;
	}

	public void LoadSettings(){
		if(!File.Exists(Path()))
		{
			DefaultSettings();
			return;
		}

		StreamReader reader = new StreamReader(Path());

		while(!reader.EndOfStream)
		{   
            string str = reader.ReadLine();

            if(str.Contains("MouseSens")){
                SetSens(str);
                slider.value = mouseSens;
                break;
            }

			SetKey(str);
		}

		reader.Close();
	}

	public void SaveSettings(){
		StreamWriter writer = new StreamWriter(Path());

		for(int i = 0; i < input.Length; i++)
		{
			writer.WriteLine(input[i].DefaultKeyName + "=" + (int)input[i].keyCode);
		}

        writer.WriteLine("MouseSens=" + mouseSens);

		writer.Close();
		Debug.Log(this + " Save ...: " + Path());
	}

	public bool GetKey(string name){
		return Input.GetKey(FindKey(name));
	}

	public bool GetKeyDown(string name){
		return Input.GetKeyDown(FindKey(name));
	}

	public bool GetKeyUp(string name){
		return Input.GetKeyUp(FindKey(name));
	}
	
	public void ButtonSave(){
		SaveSettings();
	}

	public void ButtonDefault(){
		DefaultSettings();
	}

    public void SetMouseSens(float value){
        mouseSens = value;
    }
}
