using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_DiceNumberText : MonoBehaviour {

	private TextMeshProUGUI text;
	public static int dice1Number;
	public static int dice2Number;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<TMPro.TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Rolled: \n"+dice1Number.ToString ()+ " and "+dice2Number.ToString();
	}
}
