using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


// TODO fix when X to close windows, bee not appear again

public class DataController : MonoBehaviour {

	//private string jsonFolder = "JSONdata/";
	private string initDataFile = "initdata.json";
	private string savedDataFile = "saveddata.json";
	
	private string filePath;
	private GameData loadedData;
	private bool dataLoaded;
	private HoneyScore honeyScore;
	private HoneyCurrency honeyCurrency;

	// Use this for initialization
	void Start () {
		//print(Application.persistentDataPath);

		DontDestroyOnLoad(gameObject);
		// so multiple objects wont be created, delete newest
		if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
	}

	// TODO on scene load ?
	void Update() {
		// done here and not in LoadGameData since the scene name must first be loaded (.....)
		if (!dataLoaded && loadedData != null && SceneManager.GetActiveScene().name == "GameScene") {
			dataLoaded = true;  // so this wont be repeated
			// then load from file
			
			// get the objects that data will be loaded into
			//GameObject[] unitObjects = GameObject.FindGameObjectsWithTag("honeyUnit");
			honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
			honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

			for (int i = 0; i < loadedData.units.Length; i++) {
				GameObject temp = GameObject.Find(loadedData.units[i].name);
				HoneyHandler honeyHandler = temp.GetComponent<HoneyHandler>();
				honeyHandler.fillTime = (float) loadedData.units[i].time;
				honeyHandler.points = loadedData.units[i].points;
				honeyHandler.price = loadedData.units[i].price;
				honeyHandler.honeyEmpty.sprite = Resources.Load<Sprite>("Sprites/" + loadedData.units[i].honeyEmptyButton);
				honeyHandler.honeyFull.sprite = Resources.Load<Sprite>("Sprites/" + loadedData.units[i].honeyFullImage);
				
				honeyHandler.honeyEmpty.enabled = true;
				honeyHandler.honeyFull.enabled = false;

				//temp.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + loadedData.units[i].honeyEmptyButton);
				//temp.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + loadedData.units[i].honeyFullImage);
			}
			honeyScore.harvestedHoney = loadedData.honeyScore;
			honeyCurrency.currency = loadedData.honeyCurrency;
		}
	}
	
	// called when GameScene is loaded
	public void LoadGameData(bool loadFromSave) {
		// Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build

		if (loadFromSave) {
			filePath = Application.persistentDataPath + "/" + savedDataFile; // dataPath
		}
		else {
			filePath = Application.persistentDataPath + "/" + initDataFile;
		}

		//Debug.Log(filePath);

		if(File.Exists(filePath)) {

            ReadDataFromPath();

			if (!loadFromSave) {
				// overwrite initDataFile with content of savedDataFile
				//System.IO.File.WriteAllText(Application.persistentDataPath + "/" + savedDataFile, dataAsJson);
				//Debug.Log("overwrite saveddatafile");
			}
        }
        else {
			// then file does not exist at this path, so we must create it
			// TODO REMOVE in my case, at /Users/thordis/Library/Application Support/thordisCompany/Honeycomb/
			TextAsset file = Resources.Load("JSONdata/initdata") as TextAsset;  // note: don't use .json extension
			string contentsAsJson = file.ToString(); // The full text of the asset as a single string.
			// create both files
			File.WriteAllText(Application.persistentDataPath + "/" + initDataFile, contentsAsJson);
			File.WriteAllText(Application.persistentDataPath + "/" + savedDataFile, contentsAsJson);
			
			// then load the data
			ReadDataFromPath();
			Debug.Log("Files created at persitentPath.");
        }
	}

	private void ReadDataFromPath() {
		// reads data from filePath and puts it into loadedData

		// Read the json from the file into a string
		string dataAsJson = File.ReadAllText(filePath); 
		// Pass the json to JsonUtility, and tell it to create a GameData object from it
		loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
		// 
		dataLoaded = false;

		System.IO.File.WriteAllText(Application.persistentDataPath + "/" + savedDataFile, dataAsJson);
	}

	public void SaveData(string name) {
		// change loaded data
		loadedData.honeyScore = honeyScore.harvestedHoney;
		loadedData.honeyCurrency = honeyCurrency.currency;
		for (int i = 0; i < loadedData.units.Length; i++) {
			if (loadedData.units[i].name == name) {
				// called from HoneyHandler, so only change data for unit with name given as parameter
				GameObject temp = GameObject.Find(loadedData.units[i].name);
				HoneyHandler honeyHandler = temp.GetComponent<HoneyHandler>();
				loadedData.units[i].time = honeyHandler.fillTime;
				loadedData.units[i].points = honeyHandler.points;
				loadedData.units[i].price = honeyHandler.price;
				loadedData.units[i].honeyEmptyButton = honeyHandler.honeyEmpty.sprite.name;
				loadedData.units[i].honeyFullImage = honeyHandler.honeyFull.sprite.name;
			}
		}
		// write loaded data to file
		string dataAsJson = JsonUtility.ToJson(loadedData);
		System.IO.File.WriteAllText(Application.persistentDataPath + "/" + savedDataFile, dataAsJson);
		//Debug.Log("DATA SAVED");
	}
}
