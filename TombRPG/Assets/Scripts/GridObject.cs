using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        Vector2Int positionOnGrid = targetGrid.GetGridPosition(transform.position);
        targetGrid.PlaceObject(positionOnGrid, this);
    }
}
