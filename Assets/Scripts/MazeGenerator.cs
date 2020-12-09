using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    float m_sizeCell = 0.64f;

    public MazeCell[,] m_maze;
    public GameObject[,] m_linesHorizontal;
    public GameObject[,] m_linesVertical;

    Stack<MazeCell> m_stackCells;

    [Header("Container")]
    public Transform m_container;

    [Header("Prefabs")]
    public GameObject m_prefabCell;
    public GameObject m_prefabHorizontalLine;
    public GameObject m_prefabVerticalLine;

    [Header("Values for maze - must be in Range")]
    public int m_rows;
    public int m_columns;
    public Vector2Int m_init;
    public Vector2Int m_end;

    void Awake()
    {
        m_maze = new MazeCell[m_rows, m_columns];
        m_linesHorizontal = new GameObject[m_rows + 1, m_columns];
        m_linesVertical = new GameObject[m_rows, m_columns + 1];

        m_stackCells = new Stack<MazeCell>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_maze.GetLength(0); i++)
        {
            for (int j = 0; j < m_maze.GetLength(1); j++)
            {
                Debug.Log((i + 1) + " " + (j + 1));
            }
        }

        GenerateBaseMaze();
        CreateMaze();
        EventManager.instance.CallLoadedMaze();
    }

    void CreateMaze()
    {
        MazeCell currentCell = m_maze[0, 0];
        currentCell.m_isVisited = true;
        m_stackCells.Push(currentCell);
        while (m_stackCells.Count > 0)
        {
            currentCell = m_stackCells.Pop();
            Vector2Int currentPos = currentCell.m_position;
            Vector2Int move = GetNewNeighbour(currentPos);
            if (move.x != -100 && move.y != -100) // it means it has an usable neighbour
            {
                m_stackCells.Push(currentCell);
                // Left
                if (move.y == -1)
                {
                    m_linesVertical[currentPos.x, currentPos.y].SetActive(false);
                }
                // Right
                if (move.y == 1)
                {
                    m_linesVertical[currentPos.x, currentPos.y + 1].SetActive(false);
                }
                // Up
                if (move.x == -1)
                {
                    m_linesHorizontal[currentPos.x, currentPos.y].SetActive(false);
                }
                // Down
                if (move.x == 1)
                {
                    m_linesHorizontal[currentPos.x + 1, currentPos.y].SetActive(false);
                }
                currentPos += move;
                m_maze[currentPos.x, currentPos.y].m_isVisited = true;
                m_stackCells.Push(m_maze[currentPos.x, currentPos.y]);
            }
        }
    }

    Vector2Int GetNewNeighbour(Vector2Int pos)
    {
        List<Vector2Int> moveList = new List<Vector2Int>();
        // Move left (0, -1)
        if (pos.y > 0)
        {
            if (!m_maze[pos.x, pos.y - 1].m_isVisited)
            {
                moveList.Add(new Vector2Int(0, -1));
            }
        }
        // Move right (0, 1)
        if (pos.y < m_columns - 1)
        {
            if (!m_maze[pos.x, pos.y + 1].m_isVisited)
            {
                moveList.Add(new Vector2Int(0, 1));
            }
        }
        // Move up (-1, 0)
        if (pos.x > 0)
        {
            if (!m_maze[pos.x - 1, pos.y].m_isVisited)
            {
                moveList.Add(new Vector2Int(-1, 0));
            }
        }
        // Move down (1, 0)
        if (pos.x < m_rows - 1)
        {
            if (!m_maze[pos.x + 1, pos.y].m_isVisited)
            {
                moveList.Add(new Vector2Int(1, 0));
            }
        }
        if (moveList.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, moveList.Count);
            return moveList[randIndex];
        }
        return new Vector2Int(-100, -100);
    }

    void GenerateBaseMaze()
    {
        float initX = (m_columns - 1) * -m_sizeCell * 0.5f;
        float initY = (m_rows - 1) * m_sizeCell * 0.5f;
        for (int i = 0; i < m_maze.GetLength(0); i++)
        {
            for (int j = 0; j < m_maze.GetLength(1); j++)
            {
                GameObject objCell = Instantiate(m_prefabCell, m_container);
                objCell.transform.position = new Vector3(initX + j * m_sizeCell, initY - i * m_sizeCell);
                m_maze[i, j] = objCell.GetComponent<MazeCell>();
                m_maze[i, j].m_position = new Vector2Int(i, j);
            }
        }

        initX = (m_columns - 1) * -m_sizeCell * 0.5f;
        initY = m_rows * m_sizeCell * 0.5f;

        for (int i = 0; i < m_linesHorizontal.GetLength(0); i++)
        {
            for (int j = 0; j < m_linesHorizontal.GetLength(1); j++)
            {
                GameObject objLine = Instantiate(m_prefabHorizontalLine, m_container);
                objLine.transform.position = new Vector3(initX + j * m_sizeCell, initY - i * m_sizeCell);
                m_linesHorizontal[i, j] = objLine;
            }
        }

        initX = m_columns * -m_sizeCell * 0.5f;
        initY = (m_rows - 1) * m_sizeCell * 0.5f;

        for (int i = 0; i < m_linesVertical.GetLength(0); i++)
        {
            for (int j = 0; j < m_linesVertical.GetLength(1); j++)
            {
                GameObject objLine = Instantiate(m_prefabVerticalLine, m_container);
                objLine.transform.position = new Vector3(initX + j * m_sizeCell, initY - i * m_sizeCell);
                m_linesVertical[i, j] = objLine;
            }
        }

        // To be improved.
        if (m_init.x <= 0)
        {
            m_linesHorizontal[0, m_init.y].SetActive(false);
        }
        else if (m_init.x >= m_rows - 1)
        {
            m_linesHorizontal[m_rows, m_init.y].SetActive(false);
        }
        else if (m_init.y <= 0)
        {
            m_linesVertical[m_init.x, 0].SetActive(false);
        }
        else if (m_init.y >= m_columns - 1)
        {
            m_linesVertical[m_init.x, m_columns].SetActive(false);
        }

        // To be improved.
        if (m_end.x <= 0)
        {
            m_linesHorizontal[0, m_end.y].SetActive(false);
        }
        else if (m_end.x >= m_rows - 1)
        {
            m_linesHorizontal[m_rows, m_end.y].SetActive(false);
        }
        else if (m_end.y <= 0)
        {
            m_linesVertical[m_end.x, 0].SetActive(false);
        }
        else if (m_end.y >= m_columns - 1)
        {
            m_linesVertical[m_end.x, m_columns].SetActive(false);
        }
    }
}
