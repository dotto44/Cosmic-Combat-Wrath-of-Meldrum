using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
	#region Defaults
	public const int VERSION_NUMBER = 1;
	public const int DEFAULT_BEANS = 0;
    private const int DEFAULT_WORLD = 0;
	private const int DEFAULT_TELEPORTER = -1;
	#endregion
	public int beans = DEFAULT_BEANS;
	public int world = DEFAULT_WORLD;
	public int teleporter = DEFAULT_TELEPORTER;
	public int[] plutoBeanPilesStatus = { 4, 4, 4, 4 };
	public bool[] plutoTeleportersUnlocked = new bool[3];
	public bool[] plutoItemsUnlocked = new bool[1];
	public bool[] plutoRockPillarBroken = new bool[2];
	public bool beatSpirox = false;

	public bool isDirty { get; private set; }

	const bool DEBUG_ON = true;


	public void setBeans(int beans)
	{
		isDirty = true;
		this.beans = beans;
	}

	public int getBeans()
	{
		return beans;
	}

	public void setRockPillarBroken(int index)
    {
		isDirty = true;
		plutoRockPillarBroken[index] = true;
	}

	public bool[] getRockPillarBroken()
	{
		return plutoRockPillarBroken;
	}

	public bool getRockPillarBroken(int index)
	{
		return plutoRockPillarBroken[index];
	}

	public void setPlutoBeanPileStatus(int index, int value)
	{
		isDirty = true;
		plutoBeanPilesStatus[index] = value;
	}

	public int[] getPlutoBeanPileStatus()
	{
		return plutoBeanPilesStatus;
	}

	public int getPlutoBeanPileStatus(int index)
	{
		return plutoBeanPilesStatus[index];
	}

	public bool getPlutoTeleporterUnlocked(int index)
	{
		return plutoTeleportersUnlocked[index];
	}

	public void setPlutoTeleporterUnlocked(int index)
	{
		isDirty = true;
		plutoTeleportersUnlocked[index] = true;
	}

	public void setBeatSpirox()
	{
		isDirty = true;
		beatSpirox = true;
	}

	public bool getBeatSpirox()
	{
		return beatSpirox;
	}


	/// <summary>
	/// Writes the instance of this class to the specified file in JSON format.
	/// </summary>
	/// <param name="filePath">The file name and full path to write to.</param>
	public void WriteToFile(string filePath)
	{
		// Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
		string json = JsonUtility.ToJson(this, true);

		// Write that JSON string to the specified file.
		File.WriteAllText(filePath, json);

		// Tell us what we just wrote if DEBUG_ON is on.
		if (DEBUG_ON)
			Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
	}

	/// <summary>
	/// Returns a new SaveData object read from the data in the specified file.
	/// </summary>
	/// <param name="filePath">The file to attempt to read from.</param>
	public static SaveData ReadFromFile(string filePath)
	{
		// If the file doesn't exist then just return the default object.
		if (!File.Exists(filePath))
		{
			Debug.LogErrorFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
			return new SaveData();
		}
		else
		{
			// If the file does exist then read the entire file to a string.
			string contents = File.ReadAllText(filePath);

			// If debug is on then tell us the file we read and its contents.
			if (DEBUG_ON)
				Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);

			// If it happens that the file is somehow empty then tell us and return a new SaveData object.
			if (string.IsNullOrEmpty(contents))
			{
				Debug.LogErrorFormat("File: '{0}' is empty. Returning default SaveData");
				return new SaveData();
			}

			// Otherwise we can just use JsonUtility to convert the string to a new SaveData object.
			return JsonUtility.FromJson<SaveData>(contents);
		}
	}
}
