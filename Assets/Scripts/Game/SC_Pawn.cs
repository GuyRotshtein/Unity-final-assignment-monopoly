using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SC_Pawn : MonoBehaviour
{
    private string pawnColor;
    public MonopolyBoard currentBoard;
    private int boardPosition;
    public int steps;
    private bool isMoving;
    private bool diceResultsGotten;
    public static int dice1Number;
    public static int dice2Number;
    private GameObject curPawn;
    public bool turnStartad;

    public SC_Pawn()
    {
        turnStartad = false;
        diceResultsGotten = true;
        
    }
    
    private void Start()
    {
        turnStartad = false;
        diceResultsGotten = true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    
    public IEnumerator MovePawn(GameObject targetPawn, MonopolyBoard _Board)
    {
        // Debug.Log("Waiting");
        
        if (diceResultsGotten)
        {
            diceResultsGotten = false;
            dice1Number = 0;
            dice2Number = 0;
                
        }
        yield return new WaitUntil(() => (dice1Number != 0 && dice2Number != 0));
        SC_MonopolyLogic.Instance.unityObjects["Btn_Game_ReRoll"].SetActive(false);
        steps = dice1Number + dice2Number;
        Debug.Log("steps: "+steps);
        SC_MonopolyLogic.Instance.unityObjects["Txt_DiceResults"].SetActive(true);
        
        if (!targetPawn.gameObject.name.Contains(pawnColor))
        {
            Debug.LogError("Oops!!");
            yield break;
        }
        
        if (isMoving)
        {
            // Debug.LogError("Was moving!!");
            yield break;
        }
        isMoving = true;
        
        while (steps > 0)
        {
            
            boardPosition++;
            // Debug.Log("Board position is: "+boardPosition);
            if (boardPosition > 39)
            {
                boardPosition = boardPosition - 40;
                SC_MonopolyLogic.Instance.OnPassGo();
            }

            Vector3 nextTile = MonopolyBoard.Instance.childTileList[boardPosition].position;
            while (MoveToNextTile(targetPawn, nextTile)){yield return null;}

            yield return new WaitForSeconds(0.1f);
            steps--;
        }
        turnStartad = false;
        isMoving = false;
        diceResultsGotten = true;
        //Debug.Log("A");
        // here we update the position :\
        SC_MonopolyLogic.Instance.SetPositionLogic(boardPosition, steps);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public IEnumerator JailPawn(GameObject _TargetPawn, MonopolyBoard _Board)
    {
        // Debug.Log("Waiting");
        
        steps = 1;
        Debug.Log("steps: "+steps);
        SC_MonopolyLogic.Instance.unityObjects["Txt_DiceResults"].SetActive(false);
        
        if (!_TargetPawn.gameObject.name.Contains(pawnColor))
        {
            //Debug.LogError("Oops!!");
            yield break;
        }
        
        if (isMoving)
        {
            //Debug.LogError("Was moving!!");
            yield break;
        }
        isMoving = true;
        
        while (steps > 0)
        {
            Vector3 nextTile = MonopolyBoard.Instance.childTileList[30].position;
            while (MoveToNextTile(_TargetPawn, nextTile)){yield return null;}

            yield return new WaitForSeconds(0.1f);
            steps--;
        }
        turnStartad = false;
        isMoving = false;
        diceResultsGotten = true;
        //Debug.Log("A");
        // here we update the position :\
        SC_MonopolyLogic.Instance.OnMovedToJail();
    }
    private bool MoveToNextTile(GameObject targetPawn, Vector3 goal)
    {
        return goal !=(targetPawn.transform.position = Vector3.MoveTowards(targetPawn.transform.position, goal, 4f * Time.deltaTime));
    }

    #region Logic

    public void PawnStartMovementLogic(int curPosition, MonopolyBoard _Board)
    {
        turnStartad = true;
        
        pawnColor = SC_MonopolyLogic.Instance.unityObjects["Img_CurrentTurn"].GetComponent<Image>().sprite.name;
        pawnColor = pawnColor.Replace("Sprite_Pawn_", string.Empty);
        currentBoard = _Board;
        boardPosition = curPosition;
        //Debug.Log("Pawn to move: "+pawnColor);
        StartCoroutine(MovePawn(GameObject.Find("Pawn" + pawnColor), _Board));
        
    }

    public void SendToJail(MonopolyBoard _Board)
    {
        pawnColor = SC_MonopolyLogic.Instance.unityObjects["Img_CurrentTurn"].GetComponent<Image>().sprite.name;
        pawnColor = pawnColor.Replace("Sprite_Pawn_", string.Empty);
        currentBoard = _Board;
        boardPosition = 30;
        StartCoroutine(JailPawn(GameObject.Find("Pawn" + pawnColor), _Board));
    }
    #endregion
}
