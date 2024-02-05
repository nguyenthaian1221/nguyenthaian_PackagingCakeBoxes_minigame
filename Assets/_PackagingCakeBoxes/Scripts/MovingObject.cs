using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public enum BlockName { cake, candyblock, box, coin };


    //public Transform targetPosition =;
    public float moveSpeed = 5f;
    int x;
    int y;

    public GridCell cell;

    public Vector2Int CurPosArr { get; set; }
    public BlockName blockName { get; set; }


    bool canMove = false;
    Vector3 dir;

    private void Start()
    {
        x = GameManager.instance.mapHeight;
        y = GameManager.instance.mapWidth;

        //transform.Translate(2, 0, 0);
    }

    private void Update()
    {
        // if (GameManager.instance.gameState != GameState.playing) return;

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    Shift(Vector3.left);
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    Shift(Vector3.right);
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    //Debug.Log("Hit UP");
        //    Shift(Vector3.up);
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    Shift(Vector3.down);
        //}

    }






    //public void Shift(Vector3 dir)
    //{

    //    if (dir == Vector3.up)
    //    {
    //        int i;
    //        int height;
    //        for (i = CurPosArr.x, height = i + 1; i < x && height < x; i++)
    //        {
    //            //var height = i++;
    //            var width = CurPosArr.y;
    //            if (!GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied)
    //            {
    //                GameObject temp = GridManager.instance.mapArray[i, width];
    //                GridManager.instance.mapArray[i, width].GetComponent<GridCell>().isOccupied = false;
    //                this.transform.position = GridManager.instance.mapArray[height, width].transform.position;
    //                GridManager.instance.mapArray[height, width] = temp;
    //                GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied = true;



    //            }

    //        }
    //    }
    //    else if (dir == Vector3.down)
    //    {
    //        int i;
    //        int height;
    //        for (i = CurPosArr.x, height = i - 1; i >= 0 && height >= 0; i--)
    //        {
    //            //var height = i++;
    //            var width = CurPosArr.y;
    //            if (!GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied)
    //            {
    //                GameObject temp = GridManager.instance.mapArray[i, width];
    //                GridManager.instance.mapArray[i, width].GetComponent<GridCell>().isOccupied = false;
    //                this.transform.position = GridManager.instance.mapArray[height, width].transform.position;
    //                GridManager.instance.mapArray[height, width] = temp;
    //                GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied = true;



    //            }

    //        }


    //    }
    //    else if (dir == Vector3.left)
    //    {
    //        int j, width;
    //        for (j = CurPosArr.y, width = j - 1; j >= 0 && width >= 0; j--)
    //        {
    //            var height = CurPosArr.x;
    //            if (!GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied)
    //            {
    //                GameObject temp = GridManager.instance.mapArray[height, j];
    //                GridManager.instance.mapArray[height, j].GetComponent<GridCell>().isOccupied = false;
    //                this.transform.position = GridManager.instance.mapArray[height, width].transform.position;
    //                GridManager.instance.mapArray[height, width] = temp;
    //                GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                   



    //            }




    //        }
    //    }
    //    else if (dir == Vector3.right)
    //    {
    //        int j, width;
    //        for (j = CurPosArr.y, width = j + 1; j < y && width < y; j++)
    //        {
    //            var height = CurPosArr.x;
    //            if (!GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied)
    //            {
    //                GameObject temp = GridManager.instance.mapArray[height, j];
    //                GridManager.instance.mapArray[height, j].GetComponent<GridCell>().isOccupied = false;
    //                this.transform.position = GridManager.instance.mapArray[height, width].transform.position;
    //                GridManager.instance.mapArray[height, width] = temp;
    //                GridManager.instance.mapArray[height, width].GetComponent<GridCell>().isOccupied = true;

    //            }
    //        }
    //    }

    //}


    //IEnumerator Move(Vector3 direction)
    //{
    //    MoveToCell(direction);
    //    yield return null;
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    canMove = false;
    //    Debug.Log("Va cham");
    //}



    // Change state of the Grid Cell
    public void SetBlock(GridCell gc)
    {
        if (cell != null)
        {
            cell.isOccupied = false;
        }

        cell = gc;
        cell.isOccupied = true;
    }





}
