using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool m_isVisited;
    public bool m_checkPath;
    public Vector2Int m_position;

    void Awake()
    {
        m_isVisited = false;
        m_checkPath = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        EventManager.instance.CallPressedCell(m_position);
    }
}
