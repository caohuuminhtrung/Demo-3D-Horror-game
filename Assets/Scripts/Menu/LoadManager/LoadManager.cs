using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField] private string fileName = "load.settings";
    [SerializeField] private Button loadButton;
    private static LoadManager loadManager;

    private void Awake() {
        loadManager = this;
		LoadManager.load.LoadData();
        if(loadButton != null && File.Exists(Path())){
            loadButton.interactable = true;
        }
    }

    public static LoadManager load
	{
		get
        {
            if (loadManager == null)
            {
                Debug.LogError("LoadManager là null.");
            }
            return loadManager;
        }
	}


    public Data LoadData(){
		if(!File.Exists(Path()))
		{
			return null;
		}

        Data data = new Data();
		StreamReader reader = new StreamReader(Path());

		while(!reader.EndOfStream)
		{   
            string str = reader.ReadLine();

            string[] result = str.Split(new char[]{'='});

            
            if(result[0] == "PlayerPosX"){
                data.PlayerPosX = float.Parse(result[1]);
            }
            
            if(result[0] == "PlayerPosY"){
                data.PlayerPosY = float.Parse(result[1]);
            }
            
            if(result[0] == "PlayerPosZ"){
                data.PlayerPosZ = float.Parse(result[1]);
            }

            if(result[0] == "PlayerRotX"){
                data.PlayerRotX = float.Parse(result[1]);
            }

            if(result[0] == "PlayerRotY"){
                data.PlayerRotY = float.Parse(result[1]);
            }

            if(result[0] == "PlayerRotZ"){
                data.PlayerRotZ = float.Parse(result[1]);
            }
            if(result[0] == "Hour"){
                data.Hour = int.Parse(result[1]);
            }
            if(result[0] == "Stage"){
                data.Stage = int.Parse(result[1]);
            }
		}

		reader.Close();
        return data;

	}

	public void SaveData(Data data){
		StreamWriter writer = new StreamWriter(Path());
		
		writer.WriteLine("PlayerPosX=" + data.PlayerPosX);
		writer.WriteLine("PlayerPosY=" + data.PlayerPosY);
		writer.WriteLine("PlayerPosZ=" + data.PlayerPosZ);
		writer.WriteLine("PlayerRotX=" + data.PlayerRotX);
		writer.WriteLine("PlayerRotY=" + data.PlayerRotY);
        writer.WriteLine("PlayerRotZ=" + data.PlayerRotZ);
        writer.WriteLine("GameTimeInSeconds=" + data.Hour);
        writer.WriteLine("Stage=" + data.Stage);

		writer.Close();
		Debug.Log(this + " Save ...: " + Path());
	}

    public string Path(){
		return Application.dataPath + "/" + fileName;
	}

}
