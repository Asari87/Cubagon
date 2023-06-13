using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;

public class GenericGrid<T>
{
    private int width;
    private int height;
    private float cellSize;
    private T[,] gridArray;
    //private TMP_Text[,] textObjects;
    private Vector3 originPosiiton;

    public Action<int, int> OnGridPositionUpdate;
    public GenericGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosiiton = originPosition;

        gridArray = new T[width, height];
        //textObjects = new TMP_Text[width, height];
        DrawGrid();
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100);
    }


    private void DrawGrid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = default(T);
                //textObjects[x, y] = Utilities.CreateWorldText(null, GetWorldPosition(x, y) + (Vector3.one / 2 * cellSize), $"0", Color.white, 20);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100);

            }

        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosiiton;
    }
    public bool GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosiiton).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosiiton).y / cellSize);
        if (x < 0 || y < 0 || x >= width || y >= height) return false;


        return true;
    }
    public void SetValue(int x, int y, T value)
    {
        if (InGridBounds(x, y))
        {
            gridArray[x, y] = value;
            //textObjects[x,y].text = value.ToString();
            OnGridPositionUpdate?.Invoke(x,y);  
        }
    }
    public void SetValue(Vector3 worldPosition, T value)
    {
        if(GetXY(worldPosition,out int x, out int y))
        {
            SetValue(x, y, value);
        }
    }

    private bool InGridBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public T GetValue(int x, int y)
    {
        if (InGridBounds(x, y))
        {
            return gridArray[x, y];
        }
        return default(T);
    }
    public T GetValue(Vector3 worldPosition)
    {
        if (GetXY(worldPosition, out int x, out int y))
        {
            return GetValue(x, y);
        }
        return default(T);
    }

    /// <summary>
    /// Test if ALL elements of the grid complies with a predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public bool All(Func<T, bool> predicate)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!predicate.Invoke(gridArray[x,y]))
                    return false;
            }
        }
        return true;
    }


    /// <summary>
    /// Test if ANY of the elements in the grid complies with a predicate
    /// </summary>
    /// <param name="hasAnyTiles"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Any(Func<T, bool> predicate)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (predicate.Invoke(gridArray[x, y]))
                    return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Select items in grid by a filter function
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public List<Coords> SelectByFilter(Func<T, bool> predicate)
    {
        List<Coords> list = new();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (predicate.Invoke(gridArray[x, y]))
                    list.Add(new(x, y));
            }
        }
        return list;
    }

    public List<Coords> GetTilesAroundPoint(int x, int y, int distance)
    {
        List<Coords> list = new();

        int xMin = Mathf.Clamp(x - distance, 0 , x - distance);
        int xMax = Mathf.Clamp(x + distance, x + distance ,width - 1);
        int yMin = Mathf.Clamp(y - distance, 0, y - distance);
        int yMax = Mathf.Clamp(y + distance, y + distance, height - 1);

        for (int i = xMin; i <= xMax; i++)
        {
            for (int j = yMin; j <= yMax; j++)
            {
                if (i == x && j == y) continue;
                list.Add(new(i, j));
            }
        }
        return list;

    }


}

public struct Coords
{
    public int x;
    public int y;

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}