using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataService : MonoBehaviour
{

	private static DataService _instance = null;
	public static DataService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<DataService>();

				if (_instance == null)
				{
					GameObject go = new GameObject(typeof(DataService).ToString());
					_instance = go.AddComponent<DataService>();
				}
			}

			return _instance;
		}
	}

	/// The currently loaded Save Data.
	public SaveData saveData { get; private set; }

	/// Use this to prevent reloading the data when a new scene loads.
	bool isDataLoaded = false;

	/// Store the currently loaded profile number here.
	public int currentlyLoadedProfileNumber { get; private set; }

	/// The maximum number of profiles we'll allow our users to have.
	public const int MAX_NUMBER_OF_PROFILES = 4;

	/// The base name of our save data files.
	private const string SAVE_DATA_FILE_NAME_BASE = "savedata";

	/// The base name of our save data backup files.
	private const string SAVE_DATA_BACKUP_FILE_NAME_BASE = "savedatabackup";

	/// The extension of our save data files.
	private const string SAVE_DATA_FILE_EXTENSION = ".txt";

	/// The directory our save data files will be stored in. 
	/// This is done through a getter because we're calling to a non-constant member (Application.dataPath)
	/// to construct this.
	private string SAVE_DATA_DIRECTORY { get { return Application.dataPath + "/saves/"; } }

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(this);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}

		saveData = new SaveData();
	}
}
