// using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GameBoard
{
    #region Variables 
    
    public enum ColorState
    {
        Red, Blue, Green, Yellow, None, error
    };
    
    public enum SlotState
    {
        Empty, Chance, CommunityChest, Tax, OwnedRed, OwnedBlue, OwnedGreen, OwnedYellow, HouseRed, HouseBlue, HouseGreen, HouseYellow, HotelRed, HotelBlue, HotelGreen, HotelYellow, Error
    };
    public enum WinnerState
    {
        NoWinner,Winner,Tie
    };

    public static int maxSlots = 40;
    private List<SlotState> board;
    private Dictionary<ColorState,int> pawnPositions;
    private Dictionary<ColorState,int> pawnBalances;
    private Dictionary<ColorState,int> pawnJailCards;
    private Dictionary<ColorState,bool> pawnInJail;
    private Dictionary<int, int> SlotHouseCount;
    private Transform[] childObjects;
    private Dictionary<int,ColorState> specialSlotOwners;
    private Dictionary<string, ColorState> ColorGroupOwners;
    // private int moveCount = 0;
    
    #endregion
    //what to implement: 
    //1: 16 "chance" cards, 16 "Community chest" cards
    //2: 2 dice, each rolling between 1-6 on the player's turn          DONE the rolling and numbers
    //3: at least 4 different colors to choose from for your pawn.          - automatically chosen because fuk u
    //4: pawn moving x spaces, x being the number from the dice throw           --DONE
    //5: Placing houses, upgrading existing houses on purchased tiles, GUI for interacting with a slot/tile
    //6: slot types, slot color/groups by number to check for streaks(is that the right word??)
    //7: money counters for all the boys :)
    //8: GUI so you can tell how much shit u got :)
    //9: Maybe monopoly money at the bottom? it'll be nice
    //10: checks for bankrupcy, as well as if users have enough money to buy houses or pay rent(?)
    //11: Special events
    //12: THE AI IS PROBABLY A GOOD IDEA
    
    #region MonoBehaviour
    
    public GameBoard()
    {
        // moveCount = 0;
        board = new List<SlotState>();
        for (int i = 0; i < maxSlots; i++)
            board.Add(SlotState.Empty);
        
        
        SetSlotState(2, SlotState.CommunityChest);
        SetSlotState(17, SlotState.CommunityChest);
        SetSlotState(33, SlotState.CommunityChest);
        SetSlotState(7, SlotState.Chance);
        SetSlotState(22, SlotState.Chance);
        SetSlotState(36, SlotState.Chance);
        
        pawnPositions = new Dictionary<ColorState, int>()
        {
            {ColorState.Red, 0},
            {ColorState.Blue, 0},
            {ColorState.Green, 0},
            {ColorState.Yellow, 0},
        };
        pawnBalances = new Dictionary<ColorState, int>()
        {
            {ColorState.Red, 0},
            {ColorState.Blue, 0},
            {ColorState.Green, 0},
            {ColorState.Yellow, 0},
        };
        pawnJailCards = new Dictionary<ColorState, int>()
        {
            {ColorState.Red, 0},
            {ColorState.Blue, 0},
            {ColorState.Green, 0},
            {ColorState.Yellow, 0},
        };
        pawnInJail = new Dictionary<ColorState, bool>()
        {
            {ColorState.Red, false},
            {ColorState.Blue, false},
            {ColorState.Green, false},
            {ColorState.Yellow, false},
        };
        SlotHouseCount = new Dictionary<int, int>()
        {
            {1,0},
            {3,0},
            {6,0},
            {8,0},
            {9,0},
            {11,0},
            {13,0},
            {14,0},
            {16,0},
            {18,0},
            {19,0},
            {21,0},
            {23,0},
            {24,0},
            {26,0},
            {27,0},
            {29,0},
            {31,0},
            {32,0},
            {34,0},
            {37,0},
            {39,0}
        };
        ColorGroupOwners = new Dictionary<string, ColorState>()
        {
            {"brown",ColorState.None},
            {"sky",ColorState.None},
            {"pink",ColorState.None},
            {"orange",ColorState.None},
            {"red",ColorState.None},
            {"yellow",ColorState.None},
            {"green",ColorState.None},
            {"blue",ColorState.None},
        };
        ResetSlotHouses();
        specialSlotOwners = new Dictionary<int, ColorState>
        {
            {5, ColorState.None},
            {12, ColorState.None},
            {15, ColorState.None},
            {25, ColorState.None},
            {28, ColorState.None},
            {35, ColorState.None},
        };
    }
    
    #endregion

    #region Logic
    
    public SlotState GetSlotState(int _Index)
    {
        if (_Index >= 0 && _Index < maxSlots)
            return board[_Index];
        return SlotState.Error;
    }

    public bool SetSlotState(int _Index,SlotState _NewState)
    {
        if(_Index >= 0 && _Index < maxSlots && (_NewState == SlotState.HouseRed || _NewState == SlotState.HouseBlue || _NewState == SlotState.HouseGreen || _NewState == SlotState.HouseYellow || _NewState == SlotState.HotelRed || _NewState == SlotState.HotelBlue || _NewState == SlotState.HotelGreen || _NewState == SlotState.HotelYellow || _NewState == SlotState.Chance || _NewState == SlotState.CommunityChest || _NewState == SlotState.OwnedRed || _NewState == SlotState.OwnedBlue || _NewState == SlotState.OwnedGreen || _NewState == SlotState.OwnedYellow || _NewState == SlotState.Tax))
        {
            board[_Index] = _NewState;
            return true;
        }
        return false;
    }

    public int GetPawnPosition(ColorState _Pawn)
    {
        if (pawnPositions.ContainsKey(_Pawn))
        {
            return pawnPositions[_Pawn];
        }
        return -999;
    }
    public bool SetPawnPosition(ColorState PawnColor, int NewPosition)
    {
        if (pawnPositions.ContainsKey(PawnColor))
        {
            pawnPositions[PawnColor] = NewPosition;
            return true;
        }
        return false;
    }
    
    public int GetPawnBalance(ColorState _Pawn)
    {
        if (pawnBalances.ContainsKey(_Pawn))
        {
            return pawnBalances[_Pawn];
        }
        return -9999;
    }
    
    public bool SetPawnBalance(ColorState PawnColor, int NewBalance)
    {
        if (pawnBalances.ContainsKey(PawnColor))
        {
            pawnBalances[PawnColor] = NewBalance;
            return true;
        }
        return false;
    }
    
    public int GetSlotHouses(int _Index)
    {
        if (_Index >= 0 && _Index < maxSlots)
            return SlotHouseCount[_Index];
        return -999;
    }

    public bool SetSlotHouses(int _Index,int _NewCount)
    {
        if(_Index >= 0 && _Index < maxSlots && (_NewCount == 1 || _NewCount == 2 || _NewCount == 3 || _NewCount == 4))
        {
            SlotHouseCount[_Index] = _NewCount;
            return true;
        }
        return false;
    }
    public ColorState GetSpecialSlotOwner(int _Index)
    {
        if (specialSlotOwners.ContainsKey(_Index))
        {
            return specialSlotOwners[_Index];
        }
        return ColorState.error;
    }

    public bool SetSpecialSlotOwner(int _Index,ColorState _NewOwner)
    {
        if (specialSlotOwners.ContainsKey(_Index))
        {
            specialSlotOwners[_Index] = _NewOwner;
            return true;
        }
        return false;
    }
    public ColorState GetGroupOwner(string _GroupColor)
    {
        if (ColorGroupOwners.ContainsKey(_GroupColor))
        {
            return ColorGroupOwners[_GroupColor];
        }
        return ColorState.error;
    }
    public bool SetGroupOwner(ColorState _PawnColor, string _GroupColor)
    {
        if (ColorGroupOwners.ContainsKey(_GroupColor))
        {
            ColorGroupOwners[_GroupColor] = _PawnColor;
            return true;
        }
        return false;
    }
    
    public void ResetSlotHouses()
    {
        foreach (var key in SlotHouseCount.Keys.ToList())
        {
            SlotHouseCount[key] = 0;
        }
    }
    
    public void ResetGroupOwners()
    {
        foreach (var key in ColorGroupOwners.Keys.ToList())
        {
            ColorGroupOwners[key] = ColorState.None;
        }
    }

    public int GetPawnJailCardCount(ColorState _Pawn)
    {
        if (pawnJailCards.ContainsKey(_Pawn))
        {
            return pawnJailCards[_Pawn];
        }

        return -999;
    }

    public bool SetPawnJailCardCount(ColorState PawnColor, int NewBalance)
    {
        if (pawnJailCards.ContainsKey(PawnColor))
        {
            pawnJailCards[PawnColor] = NewBalance;
            return true;
        }
        return false;
    }
    
    public int GetPawnInJail(ColorState _Pawn)
    {
        if (pawnBalances.ContainsKey(_Pawn))
        {
            if (pawnInJail[_Pawn])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        return -9999;
    }
    
    public bool SetPawnInJail(ColorState PawnColor, bool NewState)
    {
        if (pawnBalances.ContainsKey(PawnColor))
        {
            pawnInJail[PawnColor] = NewState;
            return true;
        }
        return false;
    }
    
    public override string ToString()
    {
        string _print = string.Empty;
        foreach (SlotState s in board)
            _print += s.ToString() + " ";

        return _print;
    }
    
    public WinnerState IsMatchOver()
    {
        if (CheckWinner(ColorState.Red) || CheckWinner(ColorState.Blue) || CheckWinner(ColorState.Yellow) || CheckWinner(ColorState.Green))
        {
            return WinnerState.Winner;
        }

        return WinnerState.NoWinner;
    }
    
    private bool CheckWinner(ColorState _Color)
    {
        // check balances, for all if (colorstate != _Color && balance >= 0) return false
        foreach (var _Pawncolor in pawnBalances)
        {
            if ((_Pawncolor.Key != _Color) && (GetPawnBalance(_Pawncolor.Key) >= 0))
            {
                return false;
            }
        }
        return true;
    }
    
    #endregion
}
