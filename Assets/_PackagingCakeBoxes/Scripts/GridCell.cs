using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2 Pos => transform.position;
    public bool isOccupied;
    public Block OccupiedBlock;

}
