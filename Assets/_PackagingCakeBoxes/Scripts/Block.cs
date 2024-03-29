using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GridCell Node;
    public Vector2 Pos => transform.position;

    public Vector2Int CurPosArr { get; set; }

    public void SetBlock(GridCell node)
    {
        if (Node != null) Node.OccupiedBlock = null;
        Node = node;
        Node.OccupiedBlock = this;
    }


}
