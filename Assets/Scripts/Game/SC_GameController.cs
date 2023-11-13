using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_MenuLogic;
using System;
public class SC_GameController : MonoBehaviour
{
    
    public SC_MenuLogic curMenuLogic;
    public void Btn_PopUp_RollDices()
    {
        SC_MonopolyLogic.Instance.Btn_PopUp_RollDices();
    }
    
    public void Btn_Game_Options()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_Game_Options();
    }
    
    public void Btn_Game_PawnDetails()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_PopUp_PawnDetails();
    }
    
    public void Btn_Game_PawnDetails_close()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_PopUp_PawnDetailsClose();
    }
    public void Btn_Game_ReRoll()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_Game_ReRoll();
    }
    public void Btn_Game_PropDetails()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_PopUp_PropDetails();
    }
    
    public void Btn_Game_PropDetails_close()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_PopUp_PropDetailsClose();
    }
    
    public void Btn_Game_EndTurn()
    {
        if (curMenuLogic != null)
            SC_MonopolyLogic.Instance.Btn_Game_EndTurn();
    }
}
