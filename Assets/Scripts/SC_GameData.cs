using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameData : MonoBehaviour
{
    private Dictionary<string,Sprite> unitySprites;
    private Dictionary<int, int> slotPrices;
    private Dictionary<int, int> slotTaxes;
    private Dictionary<int, int> slotRentBasic;
    private Dictionary<int, int> slotRentSameColor;
    private Dictionary<int, int> slotRentHouse1;
    private Dictionary<int, int> slotRentHouse2;
    private Dictionary<int, int> slotRentHouse3;
    private Dictionary<int, int> slotRentHouse4;
    private Dictionary<int, int> slotRentHotel;
    private Dictionary<int, string> propNames;
    private Dictionary<int, string> slotSet;
    #region Singleton

    private static SC_GameData instance;
    public static SC_GameData Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_GameData").GetComponent<SC_GameData>();

            return instance;
        }

    }

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        unitySprites = new Dictionary<string, Sprite>()
        {
            {"Sprite_Pawn_Red", Resources.Load<Sprite>("Sprites/Sprite_Pawn_Red") },
            {"Sprite_Pawn_Blue", Resources.Load<Sprite>("Sprites/Sprite_Pawn_Blue") },
            {"Sprite_Pawn_Green", Resources.Load<Sprite>("Sprites/Sprite_Pawn_Green") },
            {"Sprite_Pawn_Yellow", Resources.Load<Sprite>("Sprites/Sprite_Pawn_Yellow") },
            {"Sprite_House_Red", Resources.Load<Sprite>("Sprites/Sprite_House_Red") },
            {"Sprite_House_Blue", Resources.Load<Sprite>("Sprites/Sprite_House_Blue") },
            {"Sprite_House_Green", Resources.Load<Sprite>("Sprites/Sprite_House_Green") },
            {"Sprite_House_Yellow", Resources.Load<Sprite>("Sprites/Sprite_House_Yellow") },
            {"Sprite_Hotel_Red", Resources.Load<Sprite>("Sprites/Sprite_Hotel_Red") },
            {"Sprite_Hotel_Blue", Resources.Load<Sprite>("Sprites/Sprite_Hotel_Blue") },
            {"Sprite_Hotel_Green", Resources.Load<Sprite>("Sprites/Sprite_Hotel_Green") },
            {"Sprite_Hotel_Yellow", Resources.Load<Sprite>("Sprites/Sprite_Hotel_Yellow") }
        };
        slotPrices = new Dictionary<int, int>()
        {
            {1,60},
            {3,60},
            {5,200},
            {6,100},
            {8,100},
            {9,100},
            {11,140},
            {12,150},
            {13,140},
            {14,160},
            {15,200},
            {16,180},
            {18,180},
            {19,200},
            {21,220},
            {23,220},
            {24,240},
            {25,200},
            {26,260},
            {27,260},
            {28,150},
            {29,280},
            {31,300},
            {32,300},
            {34,320},
            {35,200},
            {37,350},
            {39,400}
        };
        slotTaxes = new Dictionary<int, int>()
        {
            {4,200},
            {38,100}
        };
        slotPrices = new Dictionary<int, int>()
        {
            {1,60},
            {3,60},
            {5,200},
            {6,100},
            {8,100},
            {9,100},
            {11,140},
            {12,150},
            {13,140},
            {14,160},
            {15,200},
            {16,180},
            {18,180},
            {19,200},
            {21,220},
            {23,220},
            {24,240},
            {25,200},
            {26,260},
            {27,260},
            {28,150},
            {29,280},
            {31,300},
            {32,300},
            {34,320},
            {35,200},
            {37,350},
            {39,400}
        };
        slotRentBasic = new Dictionary<int, int>()
        {
            {1,2},
            {3,4},
            {5,25},
            {6,6},
            {8,6},
            {9,8},
            {11,10},
            {12,25},
            {13,10},
            {14,12},
            {15,25},
            {16,14},
            {18,14},
            {19,16},
            {21,18},
            {23,18},
            {24,20},
            {25,25},
            {26,22},
            {27,22},
            {28,25},
            {29,24},
            {31,26},
            {32,26},
            {34,28},
            {35,25},
            {37,35},
            {39,50}
        };
        slotRentSameColor = new Dictionary<int, int>()
        {
            {1,4},
            {3,8},
            {5,200},
            {6,12},
            {8,12},
            {9,16},
            {11,20},
            {12,200},
            {13,20},
            {14,24},
            {15,200},
            {16,28},
            {18,28},
            {19,32},
            {21,36},
            {23,36},
            {24,40},
            {25,200},
            {26,44},
            {27,44},
            {28,200},
            {29,48},
            {31,52},
            {32,52},
            {34,56},
            {35,200},
            {37,70},
            {39,100}
        };
        slotRentHouse1 = new Dictionary<int, int>()
        {
            {1,10},
            {3,20},
            {5,200},
            {6,30},
            {8,30},
            {9,40},
            {11,50},
            {12,200},
            {13,50},
            {14,60},
            {15,200},
            {16,70},
            {18,70},
            {19,80},
            {21,90},
            {23,90},
            {24,100},
            {25,200},
            {26,110},
            {27,110},
            {28,200},
            {29,120},
            {31,130},
            {32,130},
            {34,150},
            {35,200},
            {37,175},
            {39,200}
        };
        slotRentHouse2 = new Dictionary<int, int>()
        {
            {1,30},
            {3,60},
            {5,200},
            {6,90},
            {8,90},
            {9,100},
            {11,150},
            {12,200},
            {13,150},
            {14,180},
            {15,200},
            {16,200},
            {18,200},
            {19,220},
            {21,250},
            {23,250},
            {24,300},
            {25,200},
            {26,330},
            {27,330},
            {28,200},
            {29,360},
            {31,390},
            {32,390},
            {34,450},
            {35,200},
            {37,500},
            {39,600}
        };
        slotRentHouse3 = new Dictionary<int, int>()
        {
            {1,90},
            {3,180},
            {5,200},
            {6,270},
            {8,270},
            {9,300},
            {11,450},
            {12,200},
            {13,450},
            {14,500},
            {15,200},
            {16,550},
            {18,550},
            {19,600},
            {21,700},
            {23,700},
            {24,750},
            {25,200},
            {26,800},
            {27,800},
            {28,200},
            {29,850},
            {31,900},
            {32,900},
            {34,1000},
            {35,200},
            {37,1100},
            {39,1400}
        };
        slotRentHouse4 = new Dictionary<int, int>()
        {
            {1,160},
            {3,320},
            {5,200},
            {6,400},
            {8,400},
            {9,450},
            {11,625},
            {12,200},
            {13,625},
            {14,700},
            {15,200},
            {16,750},
            {18,750},
            {19,800},
            {21,875},
            {23,875},
            {24,925},
            {25,200},
            {26,975},
            {27,975},
            {28,200},
            {29,1025},
            {31,1100},
            {32,1100},
            {34,1200},
            {35,200},
            {37,1300},
            {39,1700}
        };
        slotRentHotel = new Dictionary<int, int>()
        {
            {1,250},
            {3,450},
            {5,200},
            {6,550},
            {8,550},
            {9,600},
            {11,750},
            {12,200},
            {13,750},
            {14,900},
            {15,200},
            {16,950},
            {18,950},
            {19,1000},
            {21,1050},
            {23,1050},
            {24,1100},
            {25,200},
            {26,1150},
            {27,1150},
            {28,200},
            {29,1200},
            {31,1275},
            {32,1275},
            {34,1400},
            {35,200},
            {37,1500},
            {39,2000}
        };
        propNames = new Dictionary<int, string>()
        {
            {0,"GO / starting Tile"},
            {1,"Mediterranean Avenue"},
            {2,"Community Chest"},
            {3,"Baltic Avenue"},
            {4,"Income Tax"},
            {5,"Reading Railroad"},
            {6,"Oriental Avenue"},
            {7,"Chance"},
            {8,"Vermont Avenue"},
            {9,"Connecticut Avenue"},
            {10,"Jail"},
            {11,"St. Charles Place"},
            {12,"Electric Company (Utility)"},
            {13,"States Avenue"},
            {14,"Virginia Avenue"},
            {15,"Pennsylvania Railroad"},
            {16,"St. James Place"},
            {17,"Community Chest"},
            {18,"Tennessee Avenue"},
            {19,"New York Avenue"},
            {20,"Free Parking"},
            {21,"Kentucky Avenue"},
            {22,"Chance"},
            {23,"Indiana Avenue"},
            {24,"Illinois Avenue"},
            {25,"B&O Railroad"},
            {26,"Atlantic Avenue"},
            {27,"Ventnor Avenue"},
            {28,"Water Works (Utility)"},
            {29,"Marvin Gardens"},
            {30,"GO TO JAIL"},
            {31,"Pacific Avenue"},
            {32,"North Carolina Avenue"},
            {33,"Community chest"},
            {34,"Pennsylvania Avenue"},
            {35,"Short Line"},
            {36,"Chance"},
            {37,"Park Place"},
            {38,"Luxury Tax"},
            {39,"Boardwalk"},
            {40,"Jail"}
        };
        slotSet = new Dictionary<int, string>()
        {
            {0,"none"},
            {1,"brown"},
            {2,"none"},
            {3,"brown"},
            {4,"none"},
            {5,"Reading Railroad"},
            {6,"sky"},
            {7,"none"},
            {8,"sky"},
            {9,"sky"},
            {10,"none"},
            {11,"pink"},
            {12,"Electric Company (Utility)"},
            {13,"pink"},
            {14,"pink"},
            {15,"Pennsylvania Railroad"},
            {16,"orange"},
            {17,"none"},
            {18,"orange"},
            {19,"orange"},
            {20,"none"},
            {21,"red"},
            {22,"none"},
            {23,"red"},
            {24,"red"},
            {25,"B&O Railroad"},
            {26,"yellow"},
            {27,"yellow"},
            {28,"Water Works (Utility)"},
            {29,"yellow"},
            {30,"none"},
            {31,"green"},
            {32,"green"},
            {33,"none"},
            {34,"green"},
            {35,"Short Line"},
            {36,"none"},
            {37,"blue"},
            {38,"none"},
            {39,"blue"},
            {40,"none"}
        };
    }

    public Sprite GetSprite(string _SpriteName)
    {
        if (unitySprites.ContainsKey(_SpriteName))
            return unitySprites[_SpriteName];
        return null;
    }

    public int GetSlotPrice(int _SlotNumber)
    {
        if (slotPrices.ContainsKey(_SlotNumber))
            return slotPrices[_SlotNumber];
        return -999;
    }
    public int GetSlotRentBasic(int _SlotNumber)
    {
        if (slotRentBasic.ContainsKey(_SlotNumber))
            return slotRentBasic[_SlotNumber];
        return -999;
    }
    public int GetSlotRentSameColor(int _SlotNumber)
    {
        if (slotRentSameColor.ContainsKey(_SlotNumber))
            return slotRentSameColor[_SlotNumber];
        return -999;
    }
    public int GetSlotRentHouse1(int _SlotNumber)
    {
        if (slotRentHouse1.ContainsKey(_SlotNumber))
            return slotRentHouse1[_SlotNumber];
        return -999;
    }
    public int GetSlotRentHouse2(int _SlotNumber)
    {
        if (slotRentHouse2.ContainsKey(_SlotNumber))
            return slotRentHouse2[_SlotNumber];
        return -999;
    }
    public int GetSlotRentHouse3(int _SlotNumber)
    {
        if (slotRentHouse3.ContainsKey(_SlotNumber))
            return slotRentHouse3[_SlotNumber];
        return -999;
    }
    public int GetSlotRentHouse4(int _SlotNumber)
    {
        if (slotRentHouse4.ContainsKey(_SlotNumber))
            return slotRentHouse4[_SlotNumber];
        return -999;
    }
    public int GetSlotRentHotel(int _SlotNumber)
    {
        if (slotRentHotel.ContainsKey(_SlotNumber))
            return slotRentHotel[_SlotNumber];
        return -999;
    }
    public int GetSlotTax(int _SlotNumber)
    {
        if (slotTaxes.ContainsKey(_SlotNumber))
            return slotTaxes[_SlotNumber];
        return -999;
    }
    public string GetPropName(int _SlotNumber)
    {
        if (propNames.ContainsKey(_SlotNumber))
            return propNames[_SlotNumber];
        return "ERROR";
    }
    public string GetSlotSet(int _SlotNumber)
    {
        if (slotSet.ContainsKey(_SlotNumber))
            return slotSet[_SlotNumber];
        return "ERROR";
    }
}
