using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SC_DiceScript : MonoBehaviour {
	private static Rigidbody Dice1;
	private static Rigidbody Dice2;
	public static Vector3 dice1Velocity;
	public static Vector3 dice2Velocity;
	public int canRoll;
    
	// Use this for initialization
	void Start ()
	{
		canRoll = 0;
		Dice1 = SC_MonopolyLogic.Instance.unityObjects["dice1"].GetComponent<Rigidbody>();
		Dice2 = SC_MonopolyLogic.Instance.unityObjects["dice2"].GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		dice1Velocity = Dice1.velocity;
		dice2Velocity = Dice2.velocity;
		
		if (Input.GetKeyDown (KeyCode.Space) && canRoll != 0) {
			SC_DiceNumberText.dice1Number = 0;
			SC_DiceNumberText.dice2Number = 0;
			float dirX = Random.Range (0, 500);
			float dirY = Random.Range (0, 500);
			float dirZ = Random.Range (0, 500);
			transform.position = new Vector3 (0, 0, -3);
			transform.rotation = Quaternion.identity;
			Dice1.AddForce (transform.forward * 500);
			Dice1.AddTorque (dirX, dirY, dirZ);
		}
	}

	private void ReRoll()
	{
		
		dice1Velocity = Dice1.velocity;
		dice2Velocity = Dice2.velocity;
	
		SC_DiceNumberText.dice1Number = 0;
		SC_DiceNumberText.dice2Number = 0;
		float dirX = Random.Range (0, 500);
		float dirY = Random.Range (0, 500);
		float dirZ = Random.Range (0, 500);
		Dice1.transform.position = new Vector3 (1, 1 ,-3);
		Dice1.transform.rotation = Quaternion.identity;
		Dice1.AddForce (transform.forward * 500);
		Dice1.AddTorque (dirX, dirY, dirZ);
		dirX = Random.Range (0, 500);
		dirY = Random.Range (0, 500);
		dirZ = Random.Range (0, 500);
		Dice2.transform.position = new Vector3 (-1, -1, -3);
		Dice2.transform.rotation = Quaternion.identity;
		Dice2.AddForce (transform.forward * 500);
		Dice2.AddTorque (dirX, dirY, dirZ);
	}

	public void Btn_Game_RollDices()
	{
		canRoll = 1;
		ReRoll();
	}
}
