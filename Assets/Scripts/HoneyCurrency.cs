using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoneyCurrency : MonoBehaviour {
	// similar to HoneyScore, but is decreased when honey is bought

	public int currency;
	private string currencyText = "CURRENCY: ";
	private TextMeshProUGUI tmpu;

	// Use this for initialization
	void Start () {
		tmpu = GetComponent<TextMeshProUGUI>();
		currency = 0;
	}
	
	// Update is called once per frame
	void Update () {
		tmpu.text = currencyText + currency.ToString();
	}
}
