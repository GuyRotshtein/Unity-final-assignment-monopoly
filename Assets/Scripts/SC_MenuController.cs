using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_MenuLogic;

public class SC_MenuController : MonoBehaviour
{
    public SC_MenuLogic curMenuLogic;

    public void Btn_BackLogic()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_BackLogic();
    }
    public void Btn_MainMenu_Singleplayer()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_PlayLogic();
    }
    
    public void Btn_MainMenu_Options()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_OptionsLogic();
    }
    
    public void Btn_MainMenu_StudentInfo()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_StudentInfoLogic();
    }
    public void Btn_MainMenu_Multiplayer()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_MultiplayerLogic();
    }

    public void Slider_Options_Sfx()
    {
        if (curMenuLogic != null)
            curMenuLogic.Slider_Options_SfxLogic();
    }
    public void Slider_Options_Music()
    {
        if (curMenuLogic != null)
            curMenuLogic.Slider_Options_MusicLogic();
    }
    public void Slider_Multiplayer_Money()
    {
        if (curMenuLogic != null)
            curMenuLogic.Slider_Multiplayer_MoneyLogic();
    }
    
    public void Btn_ChangeScreen(string _ScreenName)
    {
        if (curMenuLogic != null)
        {
            try
            {
                Screens _toScreen = (Screens)Enum.Parse(typeof(Screens), _ScreenName);
                curMenuLogic.ChangeScreen(_toScreen);
            }
            catch (Exception e)
            {
                Debug.LogError("Fail to convert: " + e.ToString());
            }
        }
    }
}
