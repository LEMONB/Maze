using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public bool[] walls = new bool[4];

    public Sprite visitedNodeSprite;

    public int i;
    public int j;

    public float f = 0;
    public float h = 0;
    public float g = 0;
    public bool isClosed = false;
    public bool isOpened = false;
    public Node previous;
    public List<GameObject> neighbors = new List<GameObject>();

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

    public void PushNeighbors(GameObject[,] nodes)
    {
        neighbors.Clear();

        //LRUD
        if (AllowedMove(Side.Left))
        {
            neighbors.Add(nodes[i, j - 1]);
        }
        if (AllowedMove(Side.Right))
        {
            neighbors.Add(nodes[i, j + 1]);
        }
        if (AllowedMove(Side.Up))
        {
            neighbors.Add(nodes[i - 1, j]);
        }
        if (AllowedMove(Side.Down))
        {
            neighbors.Add(nodes[i + 1, j]);
        }


        // if (this.i < arrSize - 1 && _nodes[this.i + 1][this.j].isWall == false)
        // {
        //     this.neighbors.push(_nodes[this.i + 1][this.j]);
        // }
        // if (this.i > 0 && _nodes[this.i - 1][this.j].isWall == false)
        // {
        //     this.neighbors.push(_nodes[this.i - 1][this.j]);
        // }
        // if (this.j < arrSize - 1 && _nodes[this.i][this.j + 1].isWall == false)
        // {
        //     this.neighbors.push(_nodes[this.i][this.j + 1]);
        // }
        // if (this.j > 0 && _nodes[this.i][this.j - 1].isWall == false)
        // {
        //     this.neighbors.push(_nodes[this.i][this.j - 1]);
        // }

    }
}
