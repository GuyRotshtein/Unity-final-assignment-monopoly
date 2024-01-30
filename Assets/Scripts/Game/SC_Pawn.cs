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
        //SC_MonopolyLogic.Instance.unityObjects["Txt_DiceResults"].SetActive(true);
        StartCoroutine(SC_MonopolyLogic.Instance.ShowMessage("Rolled "+dice1Number+ " and "+dice2Number));
        
        if (!targetPawn.gameObject.name.Contains(pawnColor))
        {
            Debug.LogError("Oops!!");
            yield break;
        }
        // enter check here if pawn is on position 40, if yes check for doubles. if doubles, proceed as usual
        if (SC_MonopolyLogic.Instance.GetJailStatus() && (dice1Number != dice2Number))
        {
            //Pawn is in jail, and rolled anything other than a double
            steps = 0;
            boardPosition = 40;
            SC_MonopolyLogic.Instance.ShowNotEscapedMessage();
            //calling a logic func to show that pawn rolled something other than doubles
        } else if (SC_MonopolyLogic.Instance.GetJailStatus() && (dice1Number == dice2Number))
        {
                boardPosition = 10;
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
            if ((boardPosition > 39))
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
        if (diceResultsGotten)
        {
            diceResultsGotten = false;
            dice1Number = 0;
            dice2Number = 1;
                
        }
        yield return new WaitUntil(() => (dice1Number == 0 && dice2Number == 1));
        SC_MonopolyLogic.Instance.unityObjects["Btn_Game_ReRoll"].SetActive(false);
        SC_MonopolyLogic.Instance.unityObjects["Btn_Game_ReRoll"].SetActive(false);
        steps = dice1Number + dice2Number;
        Debug.Log("steps: "+steps);
        
        if (!_TargetPawn.gameObject.name.Contains(pawnColor))
        {
            Debug.LogError("Oops!!");
        }
        
        if (isMoving)
        {
            // Debug.LogError("Was moving!!");
            yield break;
        }
        isMoving = true;
        
        while (steps > 0)
        {
            boardPosition = 40;

            Vector3 nextTile = MonopolyBoard.Instance.childTileList[boardPosition].position;
            while (MoveToNextTile(_TargetPawn, nextTile)){yield return null;}

            yield return new WaitForSeconds(0.1f);
            steps--;
        }
        turnStartad = false;
        isMoving = false;
        diceResultsGotten = true;
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
        
        pawnColor = SC_MonopolyLogic.Instance.unityObjects["Img_Panel_Details_PawnSprite"].GetComponent<Image>().sprite.name;
        pawnColor = pawnColor.Replace("Sprite_Pawn_", string.Empty);
        currentBoard = _Board;
        boardPosition = curPosition;
        //Debug.Log("Pawn to move: "+pawnColor);
        StartCoroutine(MovePawn(GameObject.Find("Pawn" + pawnColor), _Board));
        
    }

    public void SendToJail(int curPosition, MonopolyBoard _Board, GameBoard.ColorState _Color)
    {
        pawnColor = _Color.ToString();
        pawnColor = pawnColor.Replace("Sprite_Pawn_", string.Empty);
        currentBoard = _Board;
        boardPosition = curPosition;
        StartCoroutine(JailPawn(GameObject.Find("Pawn" + pawnColor), _Board));
    }
    #endregion
}
