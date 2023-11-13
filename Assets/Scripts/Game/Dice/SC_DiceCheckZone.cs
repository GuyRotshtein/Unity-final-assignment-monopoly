using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DiceCheckZone : MonoBehaviour {

	Vector3 dice1Velocity;
	Vector3 dice2Velocity;

	// Update is called once per frame
	void FixedUpdate () {
		dice1Velocity = SC_DiceScript.dice1Velocity;
		dice2Velocity = SC_DiceScript.dice2Velocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (dice1Velocity.x == 0f && dice1Velocity.y == 0f && dice1Velocity.z == 0f)
		{
			switch (col.gameObject.name) {
			case "D1_Side1":
				SC_DiceNumberText.dice1Number = 6;
				SC_Pawn.dice1Number = 6;
				break;
			case "D1_Side2":
				SC_DiceNumberText.dice1Number = 5;
				SC_Pawn.dice1Number = 5;
				break;
			case "D1_Side3":
				SC_DiceNumberText.dice1Number = 4;
				SC_Pawn.dice1Number = 4;
				break;
			case "D1_Side4":
				SC_DiceNumberText.dice1Number = 3;
				SC_Pawn.dice1Number = 3;
				break;
			case "D1_Side5":
				SC_DiceNumberText.dice1Number = 2;
				SC_Pawn.dice1Number = 2;
				break;
			case "D1_Side6":
				SC_DiceNumberText.dice1Number = 1;
				SC_Pawn.dice1Number = 1;
				break;
			}
		}
		if (dice2Velocity.x == 0f && dice2Velocity.y == 0f && dice2Velocity.z == 0f)
		{
			// disable dices
			
			switch (col.gameObject.name) {
				case "D2_Side1":
					SC_DiceNumberText.dice2Number = 6;
					SC_Pawn.dice2Number = 6;
					break;
				case "D2_Side2":
					SC_DiceNumberText.dice2Number = 5;
					SC_Pawn.dice2Number = 5;
					break;
				case "D2_Side3":
					SC_DiceNumberText.dice2Number = 4;
					SC_Pawn.dice2Number = 4;
					break;
				case "D2_Side4":
					SC_DiceNumberText.dice2Number = 3;
					SC_Pawn.dice2Number = 3;
					break;
				case "D2_Side5":
					SC_DiceNumberText.dice2Number = 2;
					SC_Pawn.dice2Number = 2;
					break;
				case "D2_Side6":
					SC_DiceNumberText.dice2Number = 1;
					SC_Pawn.dice2Number = 1;
					break;
			}
		}
	}
}
