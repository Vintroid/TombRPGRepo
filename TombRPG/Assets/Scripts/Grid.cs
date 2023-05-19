using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Node[,] grid;
    [SerializeField] int width = 25;
    [SerializeField] int length = 25;
    [SerializeField] float cellSize = 1.0f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask terrainLayer;

    private void Awake()
    {
        GenerateGrid();
     
    }

    private void GenerateGrid()
    {
        grid = new Node[length, width];

        for(int y= 0; y < width; y++)
        {
            for(int x = 0; x < length; x++) {
                grid[x, y] = new Node();
            }
        }
        CalculateElevation();
        CheckPassableTerrain();
    }

    private void CalculateElevation()
    {
        for(int y = 0; y < width; y++) {
            
            for(int x = 0; x < length; x++)
            {
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f,Vector3.down);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer)) 
                {
                    grid[x, y].elevation = hit.point.y;
                }
            }
        }
    }

    private void CheckPassableTerrain()
    {
        for(int y = 0; y < length; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                bool passable = !Physics.CheckBox(worldPosition, Vector3.one / 2, Quaternion.identity, obstacleLayer);
                grid[x, y].passable = passable;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.DrawCube(pos, Vector3.one / 4);

                }
            }
        }
        else
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y,true);
                    Gizmos.color = grid[x, y].passable ? Color.white : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one / 4);

                }
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        return new Vector3(x * cellSize, elevation == true ? grid[x,y].elevation : 0f , y * cellSize);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / cellSize), (int)(worldPosition.z / cellSize));
        return positionOnGrid;
    }

    public void PlaceObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        if (CheckBoundary(positionOnGrid) == true)
        {
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = gridObject;
        }
        else
        {
            Debug.Log("You are trying to place an object outside the boundaries!");
        }
    }

    public bool CheckBoundary(Vector2Int positionOnGrid)
    {
        if(positionOnGrid.x < 0 || positionOnGrid.x >= length) { return false; }
        if(positionOnGrid.y < 0 || positionOnGrid.y >= width) { return false; }
        return true;
    }

    public GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        if(CheckBoundary(gridPosition) == true)
        {
            GridObject gridObject = grid[gridPosition.x, gridPosition.y].gridObject;
            return gridObject;
        }
        else
        {
            return null;
        }
    }
}
