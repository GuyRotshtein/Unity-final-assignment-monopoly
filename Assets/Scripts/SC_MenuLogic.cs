using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SC_MenuLogic : MonoBehaviour
{
    public enum Screens
    {
        MainMenu, Loading, Options, StudentInfo, Multiplayer, Game
    };

    public enum Sliders
    {
        Music, Sfx, Money
    };

    #region Variables

    public Image menuBackground;
    private Dictionary<string, GameObject> unityObjects;
    private Screens curScreen;
    private Screens prevScreen;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        InitLogic();
    }

    #endregion

    #region Logic

    private void Init()
    {
        curScreen = Screens.MainMenu;
        prevScreen = Screens.MainMenu;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _unityObj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach(GameObject g in _unityObj)
        {
            if (unityObjects.ContainsKey(g.name) == false)
                unityObjects.Add(g.name, g);
            else Debug.LogError("This key " + g.name + " Is Already inside the Dictionary!!!");
        }
    }

    private void InitLogic()
    {
        if (unityObjects.ContainsKey("Screen_Loading"))
        {
            unityObjects["Screen_Loading"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Options"))
        {
            unityObjects["Screen_Options"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_StudentInfo"))
        {
            unityObjects["Screen_StudentInfo"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Multiplayer"))
        {
            unityObjects["Screen_Multiplayer"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Game"))
        {
            unityObjects["Screen_Game"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_GameSettings"))
        {
            unityObjects["Screen_GameSettings"].SetActive(false);
        }
        
    }

    public void ChangeScreen(Screens _ToScreen)
    {
        prevScreen = curScreen;
        curScreen = _ToScreen;

        if (unityObjects.ContainsKey("Screen_" + prevScreen))
            unityObjects["Screen_" + prevScreen].SetActive(false);

        if (unityObjects.ContainsKey("Screen_" + curScreen))
            unityObjects["Screen_" + curScreen].SetActive(true);
    }
    
    public void Slider_Volume(Sliders _toSlider)
    {
        if (_toSlider != Sliders.Money)
        {
            GameObject.Find("Txt_SliderValue_"+_toSlider).GetComponent<TextMeshProUGUI>().text = 
                GameObject.Find("Slider_"+_toSlider).GetComponent<Slider>().value.ToString();
        }
        else
        {
            GameObject.Find("Txt_SliderValue_"+_toSlider).GetComponent<TextMeshProUGUI>().text = 
                GameObject.Find("Slider_"+_toSlider).GetComponent<Slider>().value.ToString()+"$";
        }
        Debug.Log(_toSlider.ToString() +" slider's New value is: " + GameObject.Find("Slider_"+_toSlider).GetComponent<Slider>().value.ToString());
    }

    #endregion

    #region Controller


    public void Btn_BackLogic()
    {
        ChangeScreen(prevScreen);
    }

    public void Btn_MainMenu_PlayLogic()
    {
        // ChangeScreen(Screens.Loading);
        ChangeScreen(Screens.Game);
        menuBackground.gameObject.SetActive(false);
         //TODO:
         //1: move to screen asking for amount of bots (max 4?)
         //2: choose your character!
         //3: add initialization of game with said amount of bots - randomizer for types of bots!!
         //4: ??? profit?
    }

    public void Btn_MainMenu_OptionsLogic()
    {
        ChangeScreen(Screens.Options);
    }
    
    public void Btn_MainMenu_MultiplayerLogic()
    {
        ChangeScreen(Screens.Multiplayer);
    }
    public void Btn_MainMenu_StudentInfoLogic()
    {
        ChangeScreen(Screens.StudentInfo);
    }
    public void Btn_Game_Options()
    {
        ChangeScreen(Screens.Options);
    }
    public void Slider_Options_SfxLogic()
    {
        Slider_Volume(Sliders.Sfx);
    }
    
    public void Slider_Options_MusicLogic()
    {
        Slider_Volume(Sliders.Music);
    }
    
    public void Slider_Multiplayer_MoneyLogic()
    {
        Slider_Volume(Sliders.Money);
    }
    
    #endregion
}
