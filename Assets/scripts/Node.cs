using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public bool[] walls = new bool[4];

    public Sprite visitedNodeSprite;

    public bool isFinish = false;
    private bool isVisited = false;
    public bool IsVisited
    {
        get
        {
            return isVisited;
        }
        set
        {
            isVisited = value;
            GetComponent<SpriteRenderer>().sprite = visitedNodeSprite;
            // GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f, 0.5f);
        }
    }

    private int setNumber = 0;
    public int SetNumber
    {
        get
        {
            return setNumber;
        }
        set
        {
            setNumber = value;
            // GetComponent<SpriteRenderer>().color = new Color(1f, (1f / value) / 8f, (8 - 1f / value) / 8f, 0.5f);
            // GetComponentInChildren<Text>().text = value.ToString();
        }
    }

    public void AddWall(Side side)
    {
        switch (side)
        {
            case Side.Left:
                walls[(int)Side.Left] = true;
                break;
            case Side.Right:
                walls[(int)Side.Right] = true;
                break;
            case Side.Up:
                walls[(int)Side.Up] = true;
                break;
            case Side.Down:
                walls[(int)Side.Down] = true;
                break;
            default:
                break;
        }
    }

    public void RemoveWall(Side side)
    {
        switch (side)
        {
            case Side.Left:
                walls[(int)Side.Left] = false;
                break;
            case Side.Right:
                walls[(int)Side.Right] = false;
                break;
            case Side.Up:
                walls[(int)Side.Up] = false;
                break;
            case Side.Down:
                walls[(int)Side.Down] = false;
                break;
            default:
                break;
        }
    }

    public bool AllowedMove(Side side)
    {
        return !walls[(int)side];
    }
}
