using System.Collections;
using System.Collections.Generic;
// using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;
using static GameBoard;

public class ViewLogic
{
    public ViewLogic() { }

    public void ChangeCurTurnState(Sprite _NewSprite)
    {
        if (_NewSprite != null)
        {
            SC_MonopolyLogic.Instance.unityObjects["Img_Popup_TurnChange_PawnSprite"].GetComponent<Image>().sprite = _NewSprite;
            SC_MonopolyLogic.Instance.unityObjects["Img_Panel_Details_PawnSprite"].GetComponent<Image>().sprite = _NewSprite;
            
        }
        else Debug.LogError("Sprite is Null");
    }

    public void ChangeCurTurnBalance(ColorState _PawnColor, int _NewBalance)
    {
        SC_MonopolyLogic.Instance.unityObjects["Txt_Panel_Details_PawnBalance"].GetComponent<TMPro.TextMeshProUGUI>().text =
            _NewBalance + "₩";
    }

    public void ChangeCurTurnJail(ColorState _PawnColor)
    {
        SC_MonopolyLogic.Instance.unityObjects["Txt_Popup_JailMenu_JailedPawnName"].GetComponent<TMPro.TextMeshProUGUI>().text =
            _PawnColor.ToString() + " pawn is in jail!";
    }
}
