using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using static UnityEditor.Progress;
using LevelUnlockSystem;

public enum GameState { pause, playing, moving, lose, win, selectLevel };


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public GameState gameState;

    public int mapHeight;
    public int mapWidth;

    public GameObject background;

    public GameObject[,] mapArray;
    public GameObject[] objects;  // 0 - gird, 1 - cake, 2 - box, 3 - candyblock, 4 - coin || Never delete this
    public Transform parentGrid;

    //private List<GridCell> _nodes;
    private List<GameObject> _blocks;  // movable objects
    private List<GameObject> _notMovingBlocks;  // movable objects

    private int cakeCount;


    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private GameObject panel_gameplay;
    [SerializeField] private GameObject panel_lose;
    [SerializeField] private GameObject panel_win;
    [SerializeField] private GameObject panel_selectlevel;
    [SerializeField] private GameObject panel_guide;
    [SerializeField] private GameObject panel_menu;
    [SerializeField] private GameObject Canvas_result;



    // Register Event Swipe

    private void OnEnable()
    {
        swipeListener.OnSwipe.AddListener(OnSwipe);
        EventManager.TimerStop += EventManagerOnTimerStop;
    }

    private void OnDisable()
    {
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
        EventManager.TimerStop -= EventManagerOnTimerStop;
    }


    private void OnSwipe(string swipe)
    {
        if (gameState != GameState.playing) return;

        switch (swipe)
        {
            case "Up":
                Shift(Vector2.up);
                break;
            case "Down":
                Shift(Vector2.down);
                break;
            case "Left":
                Shift(Vector2.left);
                break;
            case "Right":
                Shift(Vector2.right);
                break;


        }
    }

    private void EventManagerOnTimerStop()
    {
        ChangeState(GameState.lose);
    }


    private void Awake()
    {
        instance ??= this;
    }

    // will subscribe more event here




    void Start()
    {
        _blocks = new List<GameObject>();
        _notMovingBlocks = new List<GameObject>();

        // the default state is select level
        ChangeState(GameState.selectLevel);
        //LoadMap(4);
        //EventManager.OnTimerStart();  // start time unity 
        InitPanel();
        Debug.Log(Application.persistentDataPath);
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
                EventManager.OnTimerStart();
                break;
            case GameState.lose:
                Canvas_result.SetActive(true);
                panel_lose.SetActive(true);
                break;
            case GameState.win:
                Canvas_result.SetActive(true);
                panel_win.SetActive(true);
                //LevelUIManager.Instance.InitializedUI();
                break;
            case GameState.selectLevel:
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
                _notMovingBlocks.Add(grid);
            }
        }
        //Debug.LogError(mapArray[0, 0].GetComponent<GridCell>().isOccupied + "|| an generategrid");
        return mapArray;
    }

    void LoadMap(int level)  // insert parameter
    {

        string levelstring = String.Format("/level/level{0}.txt", level);
        //bool isMap = false;
        string filePath = Application.dataPath + "/_PackagingCakeBoxes" + levelstring;

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
                            blockObjects.GetComponent<Block>().SetBlock(mapArray[mapHeight - 1 - y, x].GetComponent<GridCell>());
                            if (currentNumber != 3)  // candyBlock can't move
                            {
                                if (currentNumber == 1)
                                {
                                    cakeCount++;
                                }
                                blockObjects.GetComponent<Block>().CurPosArr = new Vector2Int(mapHeight - 1 - y, x); //Save init 
                                _blocks.Add(blockObjects);
                            }
                            else
                            {
                                _notMovingBlocks.Add(blockObjects);
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
        //var board = Instantiate(background, center, Quaternion.identity);

        //Setting camera
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        //Camera.main.orthographicSize = 7;


    }


    void Shift(Vector2 dir)
    {
        var orderedBlocks = _blocks.OrderBy(b => b.GetComponent<Block>().Pos.x).ThenBy(b => b.GetComponent<Block>().Pos.y).ToList();
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

        foreach (var block in orderedBlocks)
        {
            if (!block.gameObject.activeSelf)
            {
                continue;
            }

            //Debug.Log(block.name);
            if (dir == Vector2.up)
            {
                int i;
                int height;
                for (i = (int)block.GetComponent<Block>().CurPosArr.x, height = i + 1; i < mapHeight && height < mapHeight; i++)
                {
                    //Debug.Log(block.tag + " " + i);

                    //var height = i++;
                    var width = (int)block.GetComponent<Block>().CurPosArr.y;
                    Vector3 outputPosition;
                    if (block.tag == "box")
                    {
                        if (mapArray[height, width].GetComponent<GridCell>().isOccupied)
                        {
                            if (mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.tag == "cake")
                            {
                                var sequence = DOTween.Sequence();
                                // Xoa object
                                //mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.gameObject.SetActive(false);
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                                mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.gameObject.SetActive(false);
                                // Di chuyen toi vi tri moi
                                //block.transform.position = mapArray[height, width].transform.position;
                                outputPosition = mapArray[height, width].transform.position;
                                block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                mapArray[height - 1, width].GetComponent<GridCell>().isOccupied = false;
                                sequence.Insert(0, block.transform.DOMove(outputPosition, 0.25f));

                                //Debug.Log("OnComplete: " + i);
                                sequence.OnComplete(() =>
                                {
                                    //Debug.Log("OnComplete: " + i);
                                    CheckWin();
                                    //Debug.Log("Checkwin()");
                                });


                            }
                            else if (mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.tag == "coin")
                            {
                                var sequence = DOTween.Sequence();

                                // Xoa object
                                mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.gameObject.SetActive(false);
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                                // Di chuyen toi vi tri moi
                                //block.transform.position = mapArray[height, width].transform.position;
                                outputPosition = mapArray[height, width].transform.position;
                                block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                mapArray[height - 1, width].GetComponent<GridCell>().isOccupied = false;
                                block.transform.DOMove(outputPosition, 0.25f);


                            }

                        }
                    }

                    while (!mapArray[height, width].GetComponent<GridCell>().isOccupied)
                    {
                        //GameObject temp = mapArray[i, width];
                        mapArray[i, width].GetComponent<GridCell>().isOccupied = false;
                        //block.transform.position = mapArray[height, width].transform.position;
                        outputPosition = mapArray[height, width].transform.position;
                        //mapArray[height, width] = temp;
                        block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                        mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                        block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());

                        //Debug.Log("Height: " + height);
                        var temp = height;
                        if (++temp < mapHeight)
                        {
                            if (!mapArray[temp, width].GetComponent<GridCell>().isOccupied)
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                                //block.GetComponent<Block>().SetBlock(null);
                            }
                            else
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                            }

                            height++;
                        }
                        block.transform.DOMove(outputPosition, 0.25f);
                    }


                }


            }
            if (dir == Vector2.down)
            {
                int i;
                int height;
                for (i = (int)block.GetComponent<Block>().CurPosArr.x, height = i - 1; i >= 0 && height >= 0; i--)
                {
                    //var height = i++;
                    var width = (int)block.GetComponent<Block>().CurPosArr.y;
                    Vector3 outputPosition;
                    if (block.tag == "cake")
                    {
                        if (mapArray[height, width].GetComponent<GridCell>().isOccupied)
                        {
                            if (mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.tag == "box")
                            {
                                var sequence = DOTween.Sequence();
                                // Di chuyen toi vi tri moi
                                //block.transform.position = mapArray[height, width].transform.position;
                                outputPosition = mapArray[height, width].transform.position;
                                block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                                // Xoa object
                                mapArray[height + 1, width].GetComponent<GridCell>().isOccupied = false;
                                mapArray[height + 1, width].GetComponent<GridCell>().OccupiedBlock = null;


                                sequence.Insert(0, block.transform.DOMove(outputPosition, 0.25f));

                                sequence.OnComplete(() =>
                                {
                                    block.gameObject.SetActive(false);
                                    CheckWin();
                                    Debug.Log("Checkwin()");
                                });
                                height--;


                            }

                        }
                    }
                    else if (block.tag == "coin")
                    {
                        if (mapArray[height, width].GetComponent<GridCell>().isOccupied)
                        {
                            if (mapArray[height, width].GetComponent<GridCell>().OccupiedBlock.tag == "box")
                            {
                                var sequence = DOTween.Sequence();
                                // Di chuyen toi vi tri moi
                                //block.transform.position = mapArray[height, width].transform.position;
                                outputPosition = mapArray[height, width].transform.position;
                                block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                                // Xoa object
                                mapArray[height + 1, width].GetComponent<GridCell>().isOccupied = false;
                                mapArray[height + 1, width].GetComponent<GridCell>().OccupiedBlock = null;

                                sequence.Insert(0, block.transform.DOMove(outputPosition, 0.25f));
                                // block.transform.DOMove(outputPosition, 0.25f);
                                sequence.OnComplete(() =>
                                {
                                    block.gameObject.SetActive(false);
                                });
                                height--;
                            }

                        }
                    }




                    while (!mapArray[height, width].GetComponent<GridCell>().isOccupied)
                    {
                        //GameObject temp = mapArray[i, width];
                        mapArray[i, width].GetComponent<GridCell>().isOccupied = false;
                        //block.transform.position = mapArray[height, width].transform.position;
                        outputPosition = mapArray[height, width].transform.position;
                        //mapArray[height, width] = temp;
                        block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                        mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                        block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                        var temp = height;
                        if (--temp >= 0)
                        {
                            if (!mapArray[temp, width].GetComponent<GridCell>().isOccupied)
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                            }
                            else
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                            }

                            height--;
                        }
                        block.transform.DOMove(outputPosition, 0.25f);
                    }

                }

            }



            else if (dir == Vector2.left)
            {
                int j, width;
                for (j = (int)block.GetComponent<Block>().CurPosArr.y, width = j - 1; j >= 0 && width >= 0; j--)
                {
                    var height = (int)block.GetComponent<Block>().CurPosArr.x;
                    Vector3 outputPosition;
                    while (!mapArray[height, width].GetComponent<GridCell>().isOccupied)
                    {
                        //GameObject temp = mapArray[i, width];
                        mapArray[height, j].GetComponent<GridCell>().isOccupied = false;
                        //block.transform.position = mapArray[height, width].transform.position; // di chuyen 
                        outputPosition = mapArray[height, width].transform.position;
                        //mapArray[height, width] = temp;
                        block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                        mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                        block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                        var temp = width;
                        if (--temp >= 0)
                        {
                            if (!mapArray[height, temp].GetComponent<GridCell>().isOccupied)
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                            }
                            else
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                            }
                            width--;
                        }
                        block.transform.DOMove(outputPosition, 0.25f);
                    }




                }
            }
            else if (dir == Vector2.right)
            {
                int j, width;
                for (j = (int)block.GetComponent<Block>().CurPosArr.y, width = j + 1; j < mapWidth && width < mapWidth; j++)
                {
                    var height = (int)block.GetComponent<Block>().CurPosArr.x;
                    Vector3 outputPosition;
                    while (!mapArray[height, width].GetComponent<GridCell>().isOccupied)
                    {
                        //GameObject temp = mapArray[i, width];
                        mapArray[height, j].GetComponent<GridCell>().isOccupied = false;
                        //block.transform.position = mapArray[height, width].transform.position;
                        outputPosition = mapArray[height, width].transform.position;
                        //mapArray[height, width] = temp;
                        block.GetComponent<Block>().CurPosArr = new Vector2Int(height, width);
                        mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                        block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                        var temp = width;
                        if (++temp < mapWidth)
                        {
                            if (!mapArray[height, temp].GetComponent<GridCell>().isOccupied)
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = false;
                            }
                            else
                            {
                                mapArray[height, width].GetComponent<GridCell>().isOccupied = true;
                                block.GetComponent<Block>().SetBlock(mapArray[height, width].GetComponent<GridCell>());
                            }
                            width++;
                        }
                        block.transform.DOMove(outputPosition, 0.25f);
                    }
                }
            }







        }
    }
    private void CheckWin()
    {
        cakeCount--;
        if (cakeCount == 0)
        {
            Debug.LogError("You win!!!");
            EventManager.OnTimerPausing();
            var res = Timer.Instance.TimeToDisplay;
            var startReturn = 0;
            ChangeState(GameState.win);
            if (res > 0 && res <= 15)
            {
                //Debug.Log("Duoc 1 sao");
                startReturn = 1;
            }
            else if (res > 15 && res <= 30)
            {
                //Debug.Log("Duoc 2 sao");
                startReturn = 2;
            }
            else if (res > 30 && res <= 45)
            {
                startReturn = 3;
            }
            GameUI.Instance.GameOver(startReturn);
        }
    }



    // Retry 
    public void ResetMap()
    {
        Timer.Instance.ResetTimer();
        // reset state
        ChangeState(GameState.playing);
        // Delete object and reset list
        ResetList();
        // Reset list
        // spawn new object
        cakeCount = 0;

        LoadMap(LevelSystemManager.Instance.CurrentLevel + 1); // add 1 unit because array start from 0
        //spawn lai 
    }

    [ContextMenu("ResetList")]
    public void ResetList()
    {
        mapArray = null;

        foreach (var item1 in _blocks)
        {
            Destroy(item1.gameObject);
        }

        foreach (var item2 in _notMovingBlocks)
        {
            Destroy(item2.gameObject);
        }

        _blocks.Clear();
        _notMovingBlocks.Clear();
    }

    public void InitPanel()
    {
        panel_gameplay.SetActive(false);
        panel_lose.SetActive(false);
        panel_win.SetActive(false);
        panel_selectlevel.SetActive(false);
        panel_guide.SetActive(false);
        panel_menu.SetActive(true);
        Canvas_result.SetActive(false);
    }


    public void StartAGame(int level)
    {
        Timer.Instance.ResetTimer();
        ChangeState(GameState.playing);
        cakeCount = 0;
        panel_selectlevel.SetActive(false);
        panel_gameplay.SetActive(true);

        LoadMap(level);
    }


    public void ReturnSelectMenu()
    {
        LevelUIManager.Instance.ResetList();
        ChangeState(GameState.selectLevel);
        EventManager.OnTimerPausing();
        ResetList();
    }

    public void NextLevelGame()
    {
        LevelSystemManager.Instance.CurrentLevel++;

        ChangeState(GameState.playing);
        ResetList();
        // Reset list
        // spawn new object
        cakeCount = 0;
        Timer.Instance.ResetTimer();
        LoadMap(LevelSystemManager.Instance.CurrentLevel + 1);
    }

}

