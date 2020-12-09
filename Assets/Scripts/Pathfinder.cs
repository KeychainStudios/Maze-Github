using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [Header("References")]
    public Transform m_circle;
    public MazeGenerator m_mazeGen;

    public List<MazeCell> m_pathCells;

    Vector2Int m_currentPos;
    Vector2Int m_targetPos;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnLoadedMaze += OnLoadedMaze;
        EventManager.instance.OnPressedCell += OnPressedCell;
    }

    void OnLoadedMaze()
    {
        MazeCell firstCell = m_mazeGen.m_maze[0, 0];
        m_circle.position = firstCell.transform.position;
        m_currentPos = new Vector2Int(0, 0);
    }

    void OnPressedCell(Vector2Int position)
    {
        Debug.Log("pressed cell " + position);
        m_targetPos = position;
        StartCoroutine(GetNextPath(m_currentPos));
        for (int i = 0; i < m_mazeGen.m_maze.GetLength(0); i++)
        {
            for (int j = 0; j < m_mazeGen.m_maze.GetLength(1); j++)
            {
                m_mazeGen.m_maze[i, j].m_checkPath = false;
            }
        }
        m_currentPos = m_targetPos;
    }

    IEnumerator GetNextPath(Vector2Int pos)
    {
        yield return new WaitForSeconds(0.05f);
        m_mazeGen.m_maze[pos.x, pos.y].m_checkPath = true;
        m_circle.position = m_mazeGen.m_maze[pos.x, pos.y].transform.position;
        List<Vector2Int> moveList = new List<Vector2Int>();
        // Move left (0, -1)
        if (pos.y > 0 && !m_mazeGen.m_linesVertical[pos.x, pos.y].gameObject.activeSelf)
        {
            if (!m_mazeGen.m_maze[pos.x, pos.y - 1].m_checkPath)
            {
                StartCoroutine(GetNextPath(new Vector2Int(pos.x, pos.y - 1)));
            }
        }
        // Move right (0, 1)
        if (pos.y < m_mazeGen.m_columns - 1 && !m_mazeGen.m_linesVertical[pos.x, pos.y + 1].gameObject.activeSelf)
        {
            if (!m_mazeGen.m_maze[pos.x, pos.y + 1].m_checkPath)
            {
                StartCoroutine(GetNextPath(new Vector2Int(pos.x, pos.y + 1)));
            }
        }
        // Move up (-1, 0)
        if (pos.x > 0 && !m_mazeGen.m_linesHorizontal[pos.x, pos.y].gameObject.activeSelf)
        {
            if (!m_mazeGen.m_maze[pos.x - 1, pos.y].m_checkPath)
            {
                StartCoroutine(GetNextPath(new Vector2Int(pos.x - 1, pos.y)));
            }
        }
        // Move down (1, 0)
        if (pos.x < m_mazeGen.m_rows - 1 && !m_mazeGen.m_linesHorizontal[pos.x + 1, pos.y].gameObject.activeSelf)
        {
            if (!m_mazeGen.m_maze[pos.x + 1, pos.y].m_checkPath)
            {
                StartCoroutine(GetNextPath(new Vector2Int(pos.x + 1, pos.y)));
            }
        }
        if (pos == m_targetPos)
        {
            StopAllCoroutines();
            Debug.Log("Found............");
        }
    }

    void Update()
    {

    }
}
