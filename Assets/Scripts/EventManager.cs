using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public delegate void LoadedMaze();
    public event LoadedMaze OnLoadedMaze;

    public delegate void PressedCell(Vector2Int position);
    public event PressedCell OnPressedCell;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CallLoadedMaze()
    {
        if (OnLoadedMaze != null)
        {
            OnLoadedMaze();
        }
    }

    public void CallPressedCell(Vector2Int position)
    {
        if (OnPressedCell != null)
        {
            OnPressedCell(position);
        }
    }
}
