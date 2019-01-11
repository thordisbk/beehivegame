using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : System.Object {
	// the content of json data in files
	public HoneyUnitGameData[] units;
    public int honeyScore;
    public int honeyCurrency;
}

[System.Serializable]
public class HoneyUnitGameData : System.Object {
	// information on each honey unit
	public string name;					// name of game obejct
    public string honeyEmptyButton;		// name of sprite for gameobject's honeyEmptyButton image
    public string honeyFullImage;		// name of sprite for gameobject's honeyFullImage image
    public float time;
    public int points;
    public int price;
}

// on serializable
// https://docs.unity3d.com/ScriptReference/Serializable.html