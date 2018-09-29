using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public bool[] walls = new bool[4];

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
            GetComponentInChildren<Text>().text = value.ToString();
        }
    }

    void Start()
    {
    }

    void Update()
    {

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

    public bool IsAllowed(Side side)
    {
        return !walls[(int)side];
    }
}
