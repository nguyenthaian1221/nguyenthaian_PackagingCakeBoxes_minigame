using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Unity.Collections.AllocatorManager;
using System.Linq;

public enum GameState { pause, playing, lose, win };


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public GameState gameState;

    public int mapHeight;
    public int mapWidth;

    public GameObject background;

    public GameObject[,] mapArray;
    public GameObject[] objects;  // 0 - gird, 1 - cake, 2 - box, 3 - candyblock, 4 - coin 
    public Transform parentGrid;

    private List<GridCell> _nodes;
    private List<Block> _blocks;




    private void Awake()
    {
        instance ??= this;
    }

    void Start()
    {
        ChangeState(GameState.playing);
        LoadMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameState.playing) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) Shift(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Shift(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Shift(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Shift(Vector2.down);
    }


    private void ChangeState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.pause:
                break;
            case GameState.playing:
                break;
            case GameState.lose:
                break;
            case GameState.win:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);


        }

    }

    private GameObject[,] GenerateGrid(int row, int col)
    {
        mapArray = new GameObject[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject grid = Instantiate(objects[0]) as GameObject;
                grid.transform.position = new Vector3(j, i, 0) * 2;
                grid.transform.parent = parentGrid;
                mapArray[i, j] = grid;
            }
        }
        //Debug.LogError(mapArray[0, 0].GetComponent<GridCell>().isOccupied + "|| an generategrid");
        return mapArray;
    }

    void LoadMap()
    {

        //bool isMap = false;
        string filePath = Application.dataPath + "/_PackagingCakeBoxes" + "/Level/level1.txt";

        if (!File.Exists(filePath)) return;

        string input = File.ReadAllText(filePath);


        string[] f = input.Split(new string[] { "\n", "\r", "\r\n" },
                             System.StringSplitOptions.RemoveEmptyEntries);

        if (f[0].Length > 0 && f[1].Length > 0)
        {
            int.TryParse(f[0], out mapHeight);
            int.TryParse(f[1], out mapWidth);
        }


        if (mapWidth > 0 && mapHeight > 0)
        {
            // Step 1: Spawn grid 
            mapArray = GenerateGrid(mapHeight, mapWidth);
            int y = 0, x = 0;

            // Step 2: Place Objects
            for (int i = 2; i < f.Length; i++)
            {
                string currentLine = f[i].Trim();

                string[] numbers = currentLine.Split(' ');

                x = 0;
                for (int j = 0; j < numbers.Length; j++)
                {
                    int currentNumber;
                    if (int.TryParse(numbers[j], out currentNumber))
                    {
                        //Debug.Log("Number: " + currentNumber);
                        if (currentNumber != 0)
                        {
                            //GameObject.Instantiate(objects[currentNumber], new Vector3(x, mapHeight - 1 - y, 0) * 2, Quaternion.Euler(0, 0, 0)); // place object 
                            mapArray[mapHeight - 1 - y, x].GetComponent<GridCell>().isOccupied = true;
                            GameObject blockObjects = GameObject.Instantiate(objects[currentNumber], mapArray[mapHeight - 1 - y, x].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject; // place object 

                            if (currentNumber != 3)  // candyBlock can't move
                            {
                                blockObjects.GetComponent<MovingObject>().CurPosArr = new Vector2Int(mapHeight - 1 - y, x); //Save init pos
                            }

                        }

                    }
                    else
                    {
                        Debug.LogError("Invalid number format at line " + (i + 1) + ", position " + (j + 1));
                    }
                    x++;
                }
                y++;
            }

        }

        // Spawn Background
        var center = new Vector2(mapWidth - 1, mapHeight - 1);
        var board = Instantiate(background, center, Quaternion.identity);

        //Setting camera
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        //Camera.main.orthographicSize = 7;


    }


    void Shift(Vector2 dir)
    {





    }

    GridCell GetNodeAtPosition(Vector2 pos)
    {
        return _nodes.FirstOrDefault(n => n.Pos == pos);
    }

}


