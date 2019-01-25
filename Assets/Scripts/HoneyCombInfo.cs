using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoneyCombInfo : System.Object {
	// structure of information on honey colors, prices, points, etc
	public HoneyProperties[] honeyProperties;
}

[System.Serializable]
public class HoneyProperties : System.Object {
	// structure of information on honey colors, prices, points, etc
	public string fullSprite;
	public string emptySprite;
	public float time;
	public int price;
	public int points;
	public string description;
}
