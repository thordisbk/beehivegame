using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoneyScore : MonoBehaviour {

	public int harvestedHoney;  // increased by HoneyHandler
	private string honeyText = "HONEY: ";
	private TextMeshProUGUI tmpu;

	// Use this for initialization
	void Start () {
		tmpu = GetComponent<TextMeshProUGUI>();
		harvestedHoney = 0;
	}
	
	// Update is called once per frame
	void Update () {
		tmpu.text = honeyText + harvestedHoney.ToString();
	}
}
