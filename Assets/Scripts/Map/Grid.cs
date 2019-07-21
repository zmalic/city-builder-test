using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int _size;
    private bool[,] _gridFields;

    public Grid(int size)
    {
        _size = size;

        // create new grid filled with False
        _gridFields = new bool[_size, _size];
    }

    public bool AddObject(Vector2Int position, Vector2Int size)
    {
        if (IsFree(position, size))
        {
            SetUsage(position, size, true);
            return true;
        }
        return false;
    }

    public void RemoveObject(Vector2Int position, Vector2Int size)
    {
        SetUsage(position, size, false);
    }

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
