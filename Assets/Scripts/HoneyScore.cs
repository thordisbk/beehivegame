using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoneyScore : MonoBehaviour {

	private int MAX_HONEY = 9999999;	// to avoid overflow

	[HideInInspector] public int harvestedHoney;  // increased by HoneyHandler
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

	// outside classes use this to increment the harvestedHoney value
	public void ChangeHarvestedHoneyValue(int value) {
		// value can be positive or negative (not yet used)
		if (harvestedHoney < MAX_HONEY) {
			harvestedHoney += value;
		}
	}
}
