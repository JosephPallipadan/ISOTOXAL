using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    private int[,] arr = new int[16,8];
    private int minimumPathLength = 60;

    private int startRow;
    private int startColumn;
    private int prevRow;
    private int prevColumn;

    private int initialLengthToCover;
    private int lengthToCoverForPlayer;

    public int LengthToCoverForPlayer
    {
        get
        {
            return lengthToCoverForPlayer;
        }

        set
        {
            lengthToCoverForPlayer = value;
            transform.GetChild(startRow).GetChild(startColumn).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, (lengthToCoverForPlayer*1.0f)/initialLengthToCover);
        }
    }

    void Start () {
        startRow = UnityEngine.Random.Range(0, 16);
        startColumn = UnityEngine.Random.Range(0, 8);
        arr[startRow, startColumn] = 1;

        GameObject.Find("Player").GetComponent<PlayerInner>().Destination = transform.GetChild(startRow).GetChild(startColumn).position;
        GameObject.Find("Player").transform.position = transform.GetChild(startRow).GetChild(startColumn).position;

        int startRowCopy = startRow;
        int startColumnCopy = startColumn;

        int pathLength;
        for (pathLength = 0; pathLength < minimumPathLength || !isOnEdge(startRow, startColumn, 8, 16); pathLength++)
        {
            int action = getAction(startRow, startColumn, 8, 16, arr);

            if (action == 5)
            {
                break;
            }

            switch (action)
            {
                case 1://Left
                    startColumn = Mathf.Clamp(startColumn-2, 0, 7);
                    for(int x = prevColumn; x > startColumn; x--)
                    {
                        arr[startRow, x] = 1;
                    }
                    break;
                case 2://Right
                    startColumn = Mathf.Clamp(startColumn + 2, 0, 7);
                    for (int x = prevColumn; x < startColumn; x++)
                    {
                        arr[startRow, x] = 1;
                    }
                    break;
                case 3://Up
                    startRow = Mathf.Clamp(startRow - 2, 0, 15);
                    for (int x = prevRow; x > startRow; x--)
                    {
                        arr[x, startColumn] = 1;
                    }
                    break;
                case 4://Down
                    startRow = Mathf.Clamp(startRow + 2, 0, 15);
                    for (int x = prevRow; x < startRow; x++)
                    {
                        arr[x, startColumn] = 1;
                    }
                    break;
            }

            arr[startRow, startColumn] = 1;

            prevRow = startRow;
            prevColumn = startColumn;
        }
        createMaze();

        transform.GetChild(startRowCopy).transform.GetChild(startColumnCopy).GetComponent<SpriteRenderer>().color = Color.gray;
        transform.GetChild(startRow).GetChild(startColumn).GetComponent<SpriteRenderer>().color = Color.red;

        LengthToCoverForPlayer = pathLength;
        initialLengthToCover = pathLength;

        Invoke("destroy", 10f);
    }

    private void createMaze()
    {
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if(arr[y, x] != 1)
                {
                    transform.GetChild(y).transform.GetChild(x).GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
    }

    private static bool isOnEdge(int startRow, int startColumn, int width, int height)
    {
        return startRow == 0 || startRow == height - 1 || startColumn == 0 || startColumn == width - 1;
    }

    private static int getAction(int row, int column, int width, int height, int[,] arr)
    {
        string[] actions = { "Left", "Right", "Up", "Down" };

        if (row == 0 || arr[row - 1, column] == 1)
            actions = remove(actions, "Up");
        if (row == height - 1 || arr[row + 1,column] == 1)
            actions = remove(actions, "Down");

        if (column == 0 || arr[row, column - 1] == 1)
            actions = remove(actions, "Left");
        if (column == width - 1 || arr[row, column + 1] == 1)
            actions = remove(actions, "Right");

        if (actions.Length == 0)
        {
            return 5;
        }
        string action = actions[UnityEngine.Random.Range(0, actions.Length)];

        switch (action)
        {
            case "Left":
                return 1;
            case "Right":
                return 2;
            case "Up":
                return 3;
            default:
                return 4;
        }
    }

    private static string[] remove(string[] actions, string toRemove)
    {
        string[] newArr = new string[actions.Length - 1];
        for (int n = 0, x = 0; x < actions.Length; x++)
        {
            if (actions[x].Equals(toRemove))
                continue;

            newArr[n++] = actions[x];
        }
        return newArr;
    }

    void destroy()
    {
        GameObject.Find("Player").GetComponent<PlayerInner>().Health -= 0.3f;
        Destroy(gameObject);
    }
}
