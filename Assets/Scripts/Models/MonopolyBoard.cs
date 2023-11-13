using System;
using System.Collections.Generic;
using UnityEngine;

public class MonopolyBoard : MonoBehaviour
{
    
    #region Variables
    
    private Transform[] childObjects;
    public List<Transform> childTileList ;
    #endregion
    
    #region Singleton

    private static MonopolyBoard instance;
    public static MonopolyBoard Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("Board").GetComponent<MonopolyBoard>();
            
            return instance;
        }
    }
    
    #endregion

    
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        FillTiles();

        for (int i = 0; i < childTileList.Count; i++)
        {
            Vector3 currentTile = childTileList[i].position;
            if (i > 0)
            {
                Vector3 previousTile = childTileList[i-1].position;
                Gizmos.DrawLine(previousTile, currentTile);
            }
        }
    }

    private void FillTiles()
    {
        childTileList.Clear();

        childObjects = GetComponentsInChildren<Transform>();

        foreach (Transform child in childObjects)
        {
            if (child != this.transform)
            {
                childTileList.Add(child);
            }
        }
    }
}
