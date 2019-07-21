using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int _size;
    // two dimensional bool array for storing info about grid usage
    private bool[,] _gridFields;

    public Grid(int size)
    {
        _size = size;

        // create new grid filled with False
        _gridFields = new bool[_size, _size];
    }

    /// <summary>
    /// Adding object to grid
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool AddObject(Vector2Int position, Vector2Int size)
    {
        if (IsFree(position, size))
        {
            SetUsage(position, size, true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removing object from grid
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    public void RemoveObject(Vector2Int position, Vector2Int size)
    {
        SetUsage(position, size, false);
    }

    /// <summary>
    /// Sets usage (true or false) on grid fields 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <param name="usage"></param>
    private void SetUsage(Vector2Int position, Vector2Int size, bool usage)
    {
        for (int i = position.x; i < position.x + size.x; i++)
        {
            for (int j = position.y; j < position.y + size.y; j++)
            {
                _gridFields[i, j] = usage;
            }
        }
    }

    /// <summary>
    /// Checks whether the fields are free
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool IsFree(Vector2Int position, Vector2Int size)
    {
        if (OutOfBounds(position, size))
            return false;

        // if exists used field 
        for (int i = position.x; i < position.x + size.x; i++)
        {
            for (int j = position.y; j < position.y + size.y; j++)
            {
                if (_gridFields[i, j])
                    return false;
            }
        }
        return true;
    }

    public bool IsOutOfGrid(Vector2Int position, Vector2Int size)
    {
        return position.x >= _size || position.y >= _size || position.x + size.x <= 0 || position.y + size.y <= 0;
    }

    private bool OutOfBounds(Vector2Int position, Vector2Int size)
    {
        return position.x < 0 || position.y < 0 || position.x + size.x > _size || position.y + size.y > _size;
    }
}
