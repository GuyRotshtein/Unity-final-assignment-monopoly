using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static GameBoard;


public class SC_MonopolyLogic : MonoBehaviour
{
    #region Singleton

    private static SC_MonopolyLogic instance;
    public static SC_MonopolyLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MonopolyLogic").GetComponent<SC_MonopolyLogic>();
            
            return instance;
        }
    }
    
    #endregion
    
    #region Variables
    
    private GameBoard gameBoard;
    public MonopolyBoard monopolyBoard;
    private ColorState curState; // state of each slot (tile)
    public Dictionary<string, GameObject> unityObjects;
    private ViewLogic curView; // Displays which pawn''s turn it is in the top left corner.
    private SC_DiceScript diceController;
    private SC_Pawn pawnController;
    private int playerCount;
    #endregion
    
    #region MonoBehaviour

    private void Awake()
    {
        InitAwake();
    }

    private void Start()
    {
        InitStart();
    }

    #endregion
    
    #region Logic

    private void InitAwake()
    {
        curView = new ViewLogic();// creating turn indicator
        gameBoard = new GameBoard();// creating game board
        diceController = gameObject.AddComponent<SC_DiceScript>();
        pawnController = gameObject.AddComponent<SC_Pawn>();
        curState = ColorState.Red;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _obj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach(GameObject g in _obj)
            unityObjects.Add(g.name, g);
    }

    private void InitStart()
    {
        // TODO: add a way to tell number of players here!
        
        
        // resetting all slots to their default states
        curView.ChangeCurTurnState(SC_GameData.Instance.GetSprite("Sprite_Pawn_Red"));
        for (int i = 0; i < GameBoard.maxSlots; i++)
        {
            unityObjects["Slot" + i].GetComponent<SC_Slot>().ChangeSlotState(SlotState.Empty);
        }

        gameBoard.ResetGroupOwners();
        gameBoard.ResetSlotHouses();
        gameBoard.SetSlotState(2, SlotState.CommunityChest);
        gameBoard.SetSlotState(17, SlotState.CommunityChest);
        gameBoard.SetSlotState(33, SlotState.CommunityChest);
        gameBoard.SetSlotState(7, SlotState.Chance);
        gameBoard.SetSlotState(22, SlotState.Chance);
        gameBoard.SetSlotState(36, SlotState.Chance);
        gameBoard.SetSlotState(4, SlotState.Tax);
        gameBoard.SetSlotState(38, SlotState.Chance);
        
        // Reseting and hiding dices and dice related text
        unityObjects["Txt_DiceResults"].SetActive(false);
        unityObjects["Popup_PawnDetails"].SetActive(false);
        unityObjects["Popup_PropDetails"].SetActive(false);
        unityObjects["Btn_Popup_PropDetails"].SetActive(false);
        unityObjects["Btn_Game_ReRoll"].SetActive(false);
        unityObjects["dice1"].SetActive(false);
        unityObjects["dice2"].SetActive(false);
        unityObjects["Btn_Game_EndTurn"].SetActive(false);
        unityObjects["Popup_TurnChange"].SetActive(false);
        unityObjects["Popup_JailMenu"].SetActive(false);
        unityObjects["Popup_DisplayMessage"].SetActive(false);
        unityObjects["Popup_BuyMenu"].SetActive(false);
        unityObjects["Popup_ManageProp"].SetActive(false);
        
        // reset all players to slot 0! --------------------------------------DONE
        gameBoard.SetPawnPosition(ColorState.Red, 0);
        gameBoard.SetPawnPosition(ColorState.Blue, 0);
        gameBoard.SetPawnPosition(ColorState.Green, 0);
        gameBoard.SetPawnPosition(ColorState.Yellow, 0);
        
        // reset their balance values too
        gameBoard.SetPawnBalance(ColorState.Red, 1500);
        gameBoard.SetPawnBalance(ColorState.Blue, 1500);
        gameBoard.SetPawnBalance(ColorState.Green, 1500);
        gameBoard.SetPawnBalance(ColorState.Yellow, 1500);
        
        // reset all players' "Get out of jail for free" cards count 0! --------------------------------------DONE
        gameBoard.SetPawnJailCardCount(ColorState.Red, 0);
        gameBoard.SetPawnJailCardCount(ColorState.Blue, 0);
        gameBoard.SetPawnJailCardCount(ColorState.Green, 0);
        gameBoard.SetPawnJailCardCount(ColorState.Yellow, 0);
        
        //reset all players' "is in jail" status to false"! --------------------------------------DONE
        gameBoard.SetPawnInJail(ColorState.Red, false);
        gameBoard.SetPawnInJail(ColorState.Blue, false);
        gameBoard.SetPawnInJail(ColorState.Green, false);
        gameBoard.SetPawnInJail(ColorState.Yellow, false);
        // reset all cards.
        
        // reset the balance text:
        curView.ChangeCurTurnBalance(curState, gameBoard.GetPawnBalance(curState));
    }

    private void ChangeTurn()
    {
        unityObjects["Btn_Game_EndTurn"].SetActive(false);
        unityObjects["Btn_Game_RollDice"].SetActive(false);
        while (true)
        {
            // change to next pawn color. If next has less  than 0, skip to the one after it.
            if (curState == ColorState.Red)
                curState = ColorState.Blue;
            else if (curState == ColorState.Blue)
                curState = ColorState.Green;
            else if (curState == ColorState.Green) //(playerCount == 3 || playerCount == 4) && 
                curState = ColorState.Yellow;
            else if (curState == ColorState.Yellow) //playerCount == 4 && 
                curState = ColorState.Red;
            else
                Debug.LogError("curState is not Red, Blue, Green or Yellow (" + curState + ")");

            curView.ChangeCurTurnState(SC_GameData.Instance.GetSprite("Sprite_Pawn_" + curState.ToString()));
            //get unityObject text that shows current turn owner's balance:
            curView.ChangeCurTurnBalance(curState, gameBoard.GetPawnBalance(curState));
            //Debug.Log("Test 00");
            if (gameBoard.GetPawnBalance(curState) < 0)
            {
                continue;
            }
            break;
        }
        StartCoroutine(gameBoard.GetPawnInJail(curState) == 0 ? ShowTurnChangeMessage(false) : ShowTurnChangeMessage(true));
    }


    // function for updating pawn position. Used to continue turn:
    public void SetPositionLogic(int _PawnBoardPosition, int _DiceResult)
    {
        if (!gameBoard.SetPawnPosition(curState, _PawnBoardPosition))
        {
            Debug.LogError("Error: Could not update thingy :(");
        }
        
        //GET PAWN'S NEW POSITION!
        var curSlotType = gameBoard.GetSlotState(_PawnBoardPosition);
        var curPawnBalance = gameBoard.GetPawnBalance(curState);
        unityObjects["Btn_Popup_PropDetails"].SetActive(true);
        //taking action based on the type of tile the pawn landed on
        if (_PawnBoardPosition == 0)
        {
            // Debug.Log("Congrats!! you landed on ''GO'' and got paid 200$");
            StartCoroutine(ShowMessage("Congrats!! you landed on ''GO'' and got paid 200$"));
            gameBoard.SetPawnBalance(curState,
                curPawnBalance + 200);
            
        } else if (_PawnBoardPosition == 30)
        {
            StartCoroutine(ShowMessage(curState + " pawn landed on tile 30 and was sent to jail."));
            SendToJail(curState);
        } else if (_PawnBoardPosition == 10 || _PawnBoardPosition == 20)
        {
            Debug.Log("Free Parking? in this day and age? unbelievable!");
        } 
        else if (curSlotType == SlotState.Empty){
            //Open the buy menu function:
            if (curPawnBalance - SC_GameData.Instance.GetSlotPrice(_PawnBoardPosition) >= 0)
            {
                // ask if u want to buy.
                //Edit Txt_Popup_BuyMenu_WantToBuy to include the property's name, edit Txt_Popup_BuyMenu_Details 's details
                unityObjects["Txt_Popup_BuyMenu_WantToBuy"].GetComponent<TMPro.TextMeshProUGUI>().text = "Do you want to buy "+SC_GameData.Instance.GetPropName(gameBoard.GetPawnPosition(curState))+"?";
                //IF to decide what set to designate the tile to
                unityObjects["Txt_Popup_BuyMenu_Details"].GetComponent<TMPro.TextMeshProUGUI>().text = 
                    SC_GameData.Instance.GetSlotPrice(_PawnBoardPosition).ToString()+"₩\n"
                    +SC_GameData.Instance.GetSlotSet(_PawnBoardPosition)+"\n"
                    +SC_GameData.Instance.GetSlotRentBasic(_PawnBoardPosition)+"₩\n"
                    +SC_GameData.Instance.GetSlotRentSameColor(_PawnBoardPosition)+"₩\n"
                    +SC_GameData.Instance.GetSlotRentHotel(_PawnBoardPosition)+"₩"
                    ;
                
                unityObjects["Popup_BuyMenu"].SetActive(true);
            }
            else
            {
                Debug.Log("Oops: not enough money. Better luck next time....");
                // display message that he's f*cking poor and can't buy shit
                //continue to next turn
            }
            
        } else if (curSlotType == SlotState.CommunityChest){
            // take a community chest card and do what it says
            Debug.Log("Community Chest card");

            SendToJail(curState);

        } else if (curSlotType == SlotState.Chance){
            // take a chance card and do what it says
            Debug.Log("Chance card");
            SendToJail(curState);

        } else if (curSlotType == SlotState.Error){
            // here we cry an error because fuck
        } else if (curSlotType.ToString().Contains(curState.ToString())){
            //User has landed on a building owned by himself.
            //get details about tile, update Popup_ManageProp
            //Edit Txt_Popup_ManageProp_Propname to include the property's name
            unityObjects["Txt_Popup_ManageProp_PropName"].GetComponent<TMPro.TextMeshProUGUI>().text = SC_GameData.Instance.GetPropName(gameBoard.GetPawnPosition(curState));
            //Updating property's details
            unityObjects["Txt_Popup_ManageProp_Details"].GetComponent<TMPro.TextMeshProUGUI>().text = 
                // SC_GameData.Instance.GetSlotPrice(_PawnBoardPosition).ToString()+"₩\n"
                SC_GameData.Instance.GetSlotSet(_PawnBoardPosition)+"\n \n"
                +GetRent(_PawnBoardPosition,_DiceResult)+"₩\n \n"
                +(SC_GameData.Instance.GetSlotPrice(_PawnBoardPosition) / 2)+"₩"
                ;
                
            unityObjects["Popup_ManageProp"].SetActive(true);
            //Debug.Log("Hey! you landed on your own place! how nice ;)");
            // i.e. red house / hotel when you're the red pawn
            // see if you got all the properties in the same color group from sc_gamedata.instance
            if ((_PawnBoardPosition == 1 || _PawnBoardPosition == 3) && gameBoard.GetGroupOwner("brown") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 6 || _PawnBoardPosition == 8 || _PawnBoardPosition == 9) && gameBoard.GetGroupOwner("sky") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 11 || _PawnBoardPosition == 13 || _PawnBoardPosition == 14) && gameBoard.GetGroupOwner("pink") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 16 || _PawnBoardPosition == 18 || _PawnBoardPosition == 19) && gameBoard.GetGroupOwner("orange") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 21 || _PawnBoardPosition == 23 || _PawnBoardPosition == 24) && gameBoard.GetGroupOwner("red") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 26 || _PawnBoardPosition == 27 || _PawnBoardPosition == 29) && gameBoard.GetGroupOwner("yellow") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 31 || _PawnBoardPosition == 32 || _PawnBoardPosition == 34) && gameBoard.GetGroupOwner("green") == curState)
            {
                
            }
            if ((_PawnBoardPosition == 37 || _PawnBoardPosition == 39) && gameBoard.GetGroupOwner("blue") == curState)
            {
                
            }
            // if player has a monopoly (owns all properties in this color) he can buy houses for the slot he's on.
            // if he can buy houses, and the slot's type isn't hotel, display popup.
            
        } else if (curSlotType == SlotState.Tax)
        {
            // Deducting the tax from the pawn's balance, and checking if he's bankrupt afterwards
            if (SC_GameData.Instance.GetSlotTax(_PawnBoardPosition) == -999)
            {
                Debug.LogError("ERROR: Failed to get tax value for specified slot.");
            }
            else
            {
                gameBoard.SetPawnBalance(curState,curPawnBalance - SC_GameData.Instance.GetSlotTax(_PawnBoardPosition));
                if (gameBoard.GetPawnBalance(curState) >= 0)
                {
                    StartCoroutine(ShowMessage(curState+ " pawn landed on a tax tile, and had to pay "+SC_GameData.Instance.GetSlotTax(_PawnBoardPosition)+"₩. \nRemaining balance: "+gameBoard.GetPawnBalance(curState)+"₩"));
                    unityObjects["Btn_Game_EndTurn"].SetActive(true);
                }
                else
                {
                    Debug.Log("Tough luck! You're out!!!");
                    // add a function here yo free all of his houses! or something >_>
                }
                
            }
            
        }  else {
            // landed on someone else's property, i.e.: red pawn on yellow house
            ColorState propertyOwner = ColorState.None;
            if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Red"))
            {
                propertyOwner = ColorState.Red;
            }else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Green"))
            {
                propertyOwner = ColorState.Green;
            }
            else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Blue"))
            {
                propertyOwner = ColorState.Blue;
            }
            else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Yellow"))
            {
                propertyOwner = ColorState.Yellow;
            }
            
            int rentSum = GetRent(_PawnBoardPosition, _DiceResult);
            //Debug.Log("Rent owed is: "+rentSum);
            gameBoard.SetPawnBalance(curState, gameBoard.GetPawnBalance(curState) - rentSum);
            gameBoard.SetPawnBalance(propertyOwner, gameBoard.GetPawnBalance(propertyOwner) + rentSum);
            StartCoroutine(ShowMessage(curState.ToString() + " Pawn landed on "+propertyOwner.ToString()+"'s property, and had to pay "+rentSum+"₩."));
            //Debug.Log("Rent was paid! "+curState+ " pawn has "+gameBoard.GetPawnBalance(curState)+"₩ left. \n"+propertyOwner.ToString()+" pawn now has "+gameBoard.GetPawnBalance(propertyOwner)+"₩.");
            if (gameBoard.GetPawnBalance(curState) < 0)
            {
                
                Debug.Log("Tough luck! You're out!!!");
                // add a function here yo free all of his houses! or something >_>
            }
            else
            {
                unityObjects["Btn_Game_EndTurn"].SetActive(true);
            }
        }
        unityObjects["Txt_DiceResults"].SetActive(false);
        unityObjects["dice1"].SetActive(false);
        unityObjects["dice2"].SetActive(false);
        //check winners? NO - DO IN CLIKING FUNCTION YOU DOLT
        if (gameBoard.IsMatchOver() == WinnerState.Winner)
        {
            Debug.Log("winner: curview");
            //display win screen, including back to menu button and rematch button!!!
        }
        else
        {
            Debug.Log("darn. no winner yet...");
            //if pawn isn't in jail, show the "End turn" button
            if (gameBoard.GetPawnInJail(curState) != 1 && _PawnBoardPosition != 40)
            {
                //unityObjects["Btn_Game_EndTurn"].SetActive(true);
            }
        }
    }

    private void UpdateSlotIcon(int _Index, SlotState _NewState)
    {
        // Debug.Log("OnUpdateSlotIcon  "  + _Index);
        if (gameBoard.GetSlotState(_Index) == GameBoard.SlotState.Empty &&
            unityObjects.ContainsKey("Slot" + _Index))
        {
            // Debug.Log("Empty");
            SC_Slot _curSlot = unityObjects["Slot" + _Index].GetComponent<SC_Slot>();
            if (_curSlot != null)
            {
                gameBoard.SetSlotState(_Index, _NewState);

                // Debug.Log("Changing.");
                _curSlot.ChangeSlotState(_NewState);
                //ChangeTurn();
            }
        }
    }
    
    IEnumerator ShowTurnChangeMessage (bool _IsInJail) {
        //Debug.Log("Test 11");
        unityObjects["Txt_Popup_TurnChange_PawnName"].GetComponent<TMPro.TextMeshProUGUI>().text = curState.ToString()+" Pawn's Turn";
        unityObjects["Popup_TurnChange"].SetActive(true);
        yield return new WaitForSeconds(2);
        unityObjects["Popup_TurnChange"].SetActive(false);
        if (!_IsInJail)
        {
            unityObjects["Btn_Game_RollDice"].SetActive(true);
        }
        else
        {
            unityObjects["Txt_Popup_JailMenu_JailedPawnName"].GetComponent<TMPro.TextMeshProUGUI>().text = curState.ToString()+" Pawn is still in jail,";
            unityObjects["Popup_JailMenu"].SetActive(true);
        }
    }
    
    public IEnumerator ShowMessage (string _Message) {
        unityObjects["Txt_Popup_DisplayMessage_MessageText"].GetComponent<TMPro.TextMeshProUGUI>().text = _Message;
        unityObjects["Popup_DisplayMessage"].SetActive(true);
        yield return new WaitForSeconds(2);
        unityObjects["Popup_DisplayMessage"].SetActive(false);
    }

    public void ShowNotEscapedMessage()
    {
        StartCoroutine(ShowMessage("Pawn didn't roll doubles, so he's still in jail"));
        unityObjects["Btn_Game_EndTurn"].SetActive(true);
    }

    private void SendToJail(ColorState _Color)
    {
        // just  Y E E T
        int pawnPrevPosition = gameBoard.GetPawnPosition(_Color);
        if (pawnPrevPosition != -999)
        {
            //Debug.Log("Pawn: "+_Color.ToString()+", Position on board: "+ (pawnPrevPosition + 1));
            gameBoard.SetPawnPosition(_Color, 40);
            pawnController.SendToJail(pawnPrevPosition, monopolyBoard,_Color);
        }
        else
        {
            Debug.LogError("Pawn's given position not found!");
        }
    }

    public bool GetJailStatus()
    {
        if (gameBoard.GetPawnInJail(curState) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetRent(int _PawnBoardPosition, int _DiceResult)
    {
        ColorState propertyOwner = ColorState.None;
            if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Red"))
            {
                propertyOwner = ColorState.Red;
            }else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Green"))
            {
                propertyOwner = ColorState.Green;
            }
            else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Blue"))
            {
                propertyOwner = ColorState.Blue;
            }
            else if (gameBoard.GetSlotState(_PawnBoardPosition).ToString().Contains("Yellow"))
            {
                propertyOwner = ColorState.Yellow;
            }
            
            int rentSum = 0;
            if (_PawnBoardPosition == 5|| _PawnBoardPosition == 15 || _PawnBoardPosition == 25 || _PawnBoardPosition == 35)
            {
                int amountOfRailroads = 0;
                if (gameBoard.GetSpecialSlotOwner(5) == propertyOwner)
                    amountOfRailroads++;
                if (gameBoard.GetSpecialSlotOwner(15) == propertyOwner)
                    amountOfRailroads++;
                if (gameBoard.GetSpecialSlotOwner(25) == propertyOwner)
                    amountOfRailroads++;
                if (gameBoard.GetSpecialSlotOwner(35) == propertyOwner)
                    amountOfRailroads++;
                
                //implement a check for owner of business, and check how many shits they got. based on that u pay
                switch (amountOfRailroads)
                {
                    case 1:
                        rentSum = 25;
                        break;
                    case 2:
                        rentSum = 100;
                        break;
                    case 3:
                        rentSum = 300;
                        break;
                    case 4:
                        rentSum = 800;
                        break;
                }
            }
            else if(_PawnBoardPosition == 12|| _PawnBoardPosition == 28)
            {
                if (gameBoard.GetSlotState(12).ToString().Contains(propertyOwner.ToString()) && gameBoard.GetSlotState(28).ToString().Contains(propertyOwner.ToString()))
                {
                    rentSum = _DiceResult * 4;
                }
                else
                {
                    rentSum = _DiceResult * 10;
                }
            }else{
                switch (gameBoard.GetSlotHouses(_PawnBoardPosition))
                {
                    case 0:
                        rentSum = SC_GameData.Instance.GetSlotRentBasic(_PawnBoardPosition);
                        break;
                    case 1:
                        rentSum = SC_GameData.Instance.GetSlotRentSameColor(_PawnBoardPosition);
                        break;
                    case 2:
                        rentSum = SC_GameData.Instance.GetSlotRentHouse1(_PawnBoardPosition);
                        break;
                    case 3:
                        rentSum = SC_GameData.Instance.GetSlotRentHouse2(_PawnBoardPosition);
                        break;
                    case 4:
                        rentSum = SC_GameData.Instance.GetSlotRentHouse3(_PawnBoardPosition);
                        break;
                    case 5:
                        rentSum = SC_GameData.Instance.GetSlotRentHouse4(_PawnBoardPosition);
                        break;
                    case 6:
                        rentSum = SC_GameData.Instance.GetSlotRentHotel(_PawnBoardPosition);
                        break;
                }
            }

            return rentSum;
    }

    #endregion
    
    #region Events

    public void OnMovedToJail()
    {
        
        // this is called after you're moved to the jail cell :>
        if (!gameBoard.SetPawnInJail(curState, true))
        {
            Debug.Log("ERROR: CANNOT MOVE USER TO JAIL!");
        }
        
        if (!gameBoard.SetPawnPosition(curState, 10))
        {
            Debug.LogError("Error: Could not update thingy :( to jail");
        }
        Debug.Log("Moved to jail here!");
        StartCoroutine(ShowMessage(curState.ToString() + " Pawn was moved to jail."));
        //now we show the end turn button
        unityObjects["Btn_Game_EndTurn"].SetActive(true);
    }

    public void OnPassGo()
    {
        //adding 200$ to the player on passing the "GO" tile
        gameBoard.SetPawnBalance(curState, gameBoard.GetPawnBalance(curState) + 200);
        Debug.Log("You passed ''GO'' and got 200₩.");
    }
    private void OnRollDice()
    {
        
        // hide button!
        unityObjects["Btn_Game_RollDice"].SetActive(false);
        unityObjects["Btn_Popup_PropDetails"].SetActive(false);
        unityObjects["Popup_PropDetails"].SetActive(false);
        unityObjects["Txt_DiceResults"].SetActive(true);
        unityObjects["dice1"].SetActive(true);
        unityObjects["dice2"].SetActive(true);
        unityObjects["dice1"].transform.position = new Vector3 (1000, 1000 ,-3);
        unityObjects["dice2"].transform.position = new Vector3 (1000, 1000 ,-3);
        unityObjects["Btn_Game_ReRoll"].SetActive(true);
        unityObjects["Popup_JailMenu"].SetActive(false);
        // set dice to visible
        
        
        // throw dices
        diceController.Btn_Game_RollDices();
        
        // move pawn (which one is automatically calculated in their script :D )
        // how to? just set a public variable to one?
        int pawnPrevPosition = gameBoard.GetPawnPosition(curState);
        if (pawnPrevPosition != -999)
        {
            //Debug.Log("Pawn: "+curState.ToString()+", Position on board: "+ (pawnPrevPosition + 1));
            pawnController.PawnStartMovementLogic(pawnPrevPosition, monopolyBoard);
        }
        else
        {
            Debug.LogError("Pawn's given not found!");
        }
    }
    
    private void OnPawnDetails()
    {
        // hide button!
        unityObjects["Btn_Game_PawnDetails"].SetActive(false);
        unityObjects["Popup_PawnDetails"].SetActive(true);
    }
    
    private void OnPawnDetailsClose()
    {
        // hide button!
        unityObjects["Popup_PawnDetails"].SetActive(false);
        unityObjects["Btn_Game_PawnDetails"].SetActive(true);
    }
    private void OnPropDetails()
    {
        // first, get the name of the tile you're standing on and put it into the respective field
        int curTile = gameBoard.GetPawnPosition(curState);
        unityObjects["Txt_PopUp_PropDetails_Name"].GetComponent<TMPro.TextMeshProUGUI>().text = SC_GameData.Instance.GetPropName(curTile);
        
        // get the owner of said tile
        ColorState propertyOwner = ColorState.None;
        if (gameBoard.GetSlotState(curTile).ToString().Contains("Red"))
        {
            propertyOwner = ColorState.Red;
        }else if (gameBoard.GetSlotState(curTile).ToString().Contains("Green"))
        {
            propertyOwner = ColorState.Green;
        }
        else if (gameBoard.GetSlotState(curTile).ToString().Contains("Blue"))
        {
            propertyOwner = ColorState.Blue;
        }
        else if (gameBoard.GetSlotState(curTile).ToString().Contains("Yellow"))
        {
            propertyOwner = ColorState.Yellow;
        }
        unityObjects["Txt_PopUp_PropDetails_Owner"].GetComponent<TMPro.TextMeshProUGUI>().text = propertyOwner.ToString();
        
        // hide button! AND SHOW POPUP!
        unityObjects["Btn_Popup_PropDetails"].SetActive(false);
        unityObjects["Popup_PropDetails"].SetActive(true);
    }
    
    private void OnPropDetailsClose()
    {
        // hide button!
        unityObjects["Popup_PropDetails"].SetActive(false);
        unityObjects["Btn_Popup_PropDetails"].SetActive(true);
    }
    
    private void OnReRoll()
    {
        unityObjects["dice1"].transform.position = new Vector3 (1000, 1000 ,-3);
        unityObjects["dice2"].transform.position = new Vector3 (1000, 1000 ,-3);
        diceController.Btn_Game_RollDices();
        int pawnPrevPosition = gameBoard.GetPawnPosition(curState);
        if (pawnPrevPosition != -999)
        {
            //Debug.Log("Pawn: "+curState.ToString()+", Position on board: "+ (pawnPrevPosition + 1));
            pawnController.PawnStartMovementLogic(pawnPrevPosition, monopolyBoard);
        }
        else
        {
            Debug.LogError("Pawn's given not found!");
        }
    }

    private void OnJailRoll()
    {
        //roll dices. if user rolls a double, advance. If not, end his turn.
        OnRollDice();
    }

    private void OnJailPay()
    {
        // check pawn's balance
        if (gameBoard.GetPawnBalance(curState) >= 50)
        {
            //deduct 50$ from pawn's balance
            gameBoard.SetPawnBalance(curState, (gameBoard.GetPawnBalance(curState) - 50));
            // change pawn's position to slot 10, only in gameboard. don't need to move him, he'll move by next turn
            gameBoard.SetPawnPosition(curState, 10);
            // change pawn's jail status to false
            gameBoard.SetPawnInJail(curState, false);
            
            //hiding jail menu, and showing "end turn" button
            unityObjects["Popup_JailMenu"].SetActive(false);
            //show message confirming change.
            StartCoroutine(ShowMessage(curState.ToString() + " pawn payed 50₩, and will move out of jail in the next round"));
            
            unityObjects["Btn_Game_EndTurn"].SetActive(true);
        }
        else
        {
            StartCoroutine(ShowMessage(curState.ToString() + " pawn has less than 50₩ left, and thus they can't  exit jail. Try another option"));
        }
    }
    
    private void OnJailUseCard()
    {
        // check if pawn got any "get out of jail free" cards
        if (gameBoard.GetPawnJailCardCount(curState) > 0)
        {
            //remove one of the pawn's cards
            gameBoard.SetPawnJailCardCount(curState, (gameBoard.GetPawnJailCardCount(curState) - 1));
            // change pawn's position to slot 10, only in gameboard. don't need to move him, he'll move by next turn
            gameBoard.SetPawnPosition(curState, 10);
            // change pawn's jail status to false
            gameBoard.SetPawnInJail(curState, false);
            
            //hiding jail menu, and showing "end turn" button
            unityObjects["Popup_JailMenu"].SetActive(false);
            //show message confirming change.
            StartCoroutine(ShowMessage(curState.ToString() + " pawn used one of his ''Get out of jail free'' cards, and will move out of jail in the next round"));
            
            unityObjects["Btn_Game_EndTurn"].SetActive(true);
        }
        else
        {
            StartCoroutine(ShowMessage(curState.ToString() + " pawn has no ''Get out of jail free'' cards left, and thus they can't exit jail yet. Try another option"));
        }
    }

    private void OnBuyMenuYes()
    {
        var curPawnBalance = gameBoard.GetPawnBalance(curState);
        var pawnBoardPosition = gameBoard.GetPawnPosition(curState);
        if (curPawnBalance - SC_GameData.Instance.GetSlotPrice(pawnBoardPosition) >= 0)
        {
            gameBoard.SetPawnBalance(curState, curPawnBalance - SC_GameData.Instance.GetSlotPrice(pawnBoardPosition));
            // if it's a railway or utility company, set it to "Owned"+the pawn's color.
            if (pawnBoardPosition == 5 || pawnBoardPosition == 12 || pawnBoardPosition == 15 || pawnBoardPosition == 25 || pawnBoardPosition == 28 || pawnBoardPosition == 35)
            {
                switch (curState)
                {
                    case ColorState.Red:
                    {
                        // add call to function that changes the fucking slot image here
                        UpdateSlotIcon(gameBoard.GetPawnPosition(curState), SlotState.OwnedRed);
                        break;
                    }
                    case ColorState.Blue:
                    {
                        gameBoard.SetSlotState(gameBoard.GetPawnPosition(curState),SlotState.OwnedBlue);
                        break;
                    }
                    case ColorState.Green:
                    {
                        gameBoard.SetSlotState(gameBoard.GetPawnPosition(curState),SlotState.OwnedGreen);
                        break;
                    }
                    case ColorState.Yellow:
                    {
                        gameBoard.SetSlotState(gameBoard.GetPawnPosition(curState),SlotState.OwnedYellow);
                        break;
                    }
                }
            }
            else
            {
                switch (curState)
                {
                    case ColorState.Red:
                    {
                        UpdateSlotIcon(gameBoard.GetPawnPosition(curState), SlotState.HouseRed);
                        gameBoard.SetSlotHouses(gameBoard.GetPawnPosition(curState), 0);
                        break;
                    }
                     case ColorState.Blue:
                    {
                        UpdateSlotIcon(gameBoard.GetPawnPosition(curState), SlotState.HouseBlue);
                        gameBoard.SetSlotHouses(gameBoard.GetPawnPosition(curState), 0);
                        break;
                    }
                    case ColorState.Green:
                    {
                        UpdateSlotIcon(gameBoard.GetPawnPosition(curState), SlotState.HouseGreen);
                        gameBoard.SetSlotHouses(gameBoard.GetPawnPosition(curState), 0);
                        break;
                    }
                    case ColorState.Yellow:
                    {
                        UpdateSlotIcon(gameBoard.GetPawnPosition(curState), SlotState.HouseYellow);
                        gameBoard.SetSlotHouses(gameBoard.GetPawnPosition(curState), 0);
                        break;
                    }
                }
            }
            StartCoroutine(ShowMessage(curState +" pawn has bought "+SC_GameData.Instance.GetPropName(gameBoard.GetPawnPosition(curState))+"\n Remaining balance: "+gameBoard.GetPawnBalance(curState)+"₩"));
    
            // Debug.Log("Bought the house for you my man! now you got "+gameBoard.GetPawnBalance(curState)+"$.");
               // hiding buy panel, show end turn button 
               unityObjects["Popup_BuyMenu"].SetActive(false);
               unityObjects["Btn_Game_EndTurn"].SetActive(true);
        }
        else
        {
            StartCoroutine(ShowMessage("You do not have enough money to buy "+SC_GameData.Instance.GetPropName(gameBoard.GetPawnPosition(curState))+"\n Your balance: "+gameBoard.GetPawnBalance(curState)+"₩"));

            Debug.Log("Oops: not enough money. Better luck next time....");
            // display message that he's f*cking poor and can't buy shit
            //continue to next turn
        }
    }

    private void OnBuyMenuNo()
    {
        //closing buy menu and showing "end turn"
        unityObjects["Popup_BuyMenu"].SetActive(false);
        unityObjects["Btn_Game_EndTurn"].SetActive(true);
    }
    
    #endregion
    
    #region Controller

    public void Btn_PopUp_RollDices()
    {
        OnRollDice();
    }
    public void Btn_PopUp_PawnDetails()
    {
        OnPawnDetails();
    }
    public void Btn_PopUp_PawnDetailsClose()
    {
        OnPawnDetailsClose();
    }
    public void Btn_PopUp_PropDetails()
    {
        OnPropDetails();
    }
    public void Btn_PopUp_PropDetailsClose()
    {
        OnPropDetailsClose();
    }
    public void Btn_Game_EndTurn()
    {
        ChangeTurn();
    }
    public void Btn_Game_ReRoll()
    {
        OnReRoll();
    }

    public void Btn_JailMenu_Roll()
    {
        OnJailRoll();
    }
    
    public void Btn_JailMenu_Pay()
    {
        OnJailPay();
    }
    
    public void Btn_JailMenu_UseCard()
    {
        OnJailUseCard();
    }

    public void Btn_BuyMenu_Yes()
    {
        OnBuyMenuYes();
    }
    public void Btn_BuyMenu_No()
    {
        OnBuyMenuNo();
    }
    
    #endregion
}