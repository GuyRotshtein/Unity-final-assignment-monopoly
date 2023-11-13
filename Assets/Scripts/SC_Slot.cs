using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameBoard;

public class SC_Slot : MonoBehaviour
{
    #region Variables

    public int slotIndex;
    public SpriteRenderer curImage;

    public static Action<int> OnClickSlot;

    #endregion

    #region MonoBehaviour

    void OnMouseUpAsButton()
    {
        if (OnClickSlot != null)
            OnClickSlot(slotIndex);
    }

    #endregion

    #region Logic

    private void SetSprite(Sprite _NewSprite)
    {
        if (_NewSprite != null)
            curImage.sprite = _NewSprite;
        else Debug.LogError("Sprite Is Null");
    }

    public void ChangeSlotState(SlotState _SlotState)
    {
        switch (_SlotState)
        {
            case SlotState.HouseRed: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_House_Red"));
                break;
            case SlotState.HouseBlue: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_House_Blue"));
                break;
            case SlotState.HouseGreen: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_House_Green"));
                break;
            case SlotState.HouseYellow: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_House_Yellow"));
                break;
            case SlotState.HotelRed: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_Hotel_Red"));
                break;
            case SlotState.HotelBlue: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_Hotel_Blue"));
                break;
            case SlotState.HotelGreen: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_Hotel_Green"));
                break;
            case SlotState.HotelYellow: 
                curImage.enabled = true;
                SetSprite(SC_GameData.Instance.GetSprite("Sprite_Hotel_Yellow"));
                break;
            case SlotState.Empty:curImage.enabled = false;break;
        }
    }
    #endregion
}
