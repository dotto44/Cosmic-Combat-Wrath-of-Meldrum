using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameInputManager
{

    public static float localAudioValue;
    private static bool hasChangedKeysSinceSave;
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[10]
    {
            "Attack", //0
			"Jump", //1
			"Dash", //2
			"Reload", //3
			"Left", //4
			"Right",
            "Up",
            "Down",
            "Rocket",
            "Focus"
    };
    /* static KeyCode[] defaults = new KeyCode[10]
    {
            KeyCode.Joystick1Button3, //0
	     	KeyCode.Joystick1Button2, //1
			KeyCode.Joystick1Button15, //2
			KeyCode.Joystick1Button14, //3
			KeyCode.LeftArrow, //4
			KeyCode.RightArrow,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.DownArrow,
            KeyCode.F
    }; */
    static KeyCode[] defaults = new KeyCode[10]
    {
            KeyCode.C, //0
	     	KeyCode.Z, //1
			KeyCode.X, //2
			KeyCode.Joystick1Button14, //3
			KeyCode.LeftArrow, //4
			KeyCode.RightArrow,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.DownArrow,
            KeyCode.F
    };
    static GameInputManager()
    {
        InitializeDictionary();
    }

    public static void hasModifiedSettings()
    {
        hasChangedKeysSinceSave = true;
    }
    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }
    /*public static bool GetButtonDown(string keyMap)
    {
        return Input.GetButtonDown(keyMapping[keyMap]);
    }*/
    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }
    public static bool GetKeyUp(string keyMap)
    {
        return Input.GetKeyUp(keyMapping[keyMap]);
    }
    public static void swapKey(KeyCode kcode, string key)
    {
        hasChangedKeysSinceSave = true;
        keyMapping[key] = kcode;
    }
    public static void swapKey(KeyCode kcode, string key, bool isUser)
    {
        hasChangedKeysSinceSave = isUser;
        keyMapping[key] = kcode;
    }
    public static void showKeys()
    {
        foreach (KeyValuePair<string, KeyCode> s in keyMapping)
        {
            Debug.Log("Move: " + s.Key + "Key: " + s.Value);
        }
    }
    public static KeyCode checkKey(string key)
    {
        return keyMapping[key];
    }

}