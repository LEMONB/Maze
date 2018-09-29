using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = 0, Right, Up, Down };

public class PlayerMovement : MonoBehaviour
{
    // MapGenerator mapGen;
    MapGeneratorKruskal mapGen;

    int currI = 0;
    int currJ = 0;

    void Start()
    {
        // mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Move(Side.Left);
        if (Input.GetKeyDown(KeyCode.D))
            Move(Side.Right);
        if (Input.GetKeyDown(KeyCode.W))
            Move(Side.Up);
        if (Input.GetKeyDown(KeyCode.S))
            Move(Side.Down);
    }

    void Move(Side movement)
    {
        // if (mapGen.GetComponent<MapGenerator>().nodes[currI, currJ].GetComponent<Node>().WallExists(movement))
        if (mapGen.GetComponent<MapGeneratorKruskal>().nodes[currI, currJ].GetComponent<Node>().WallExists(movement))
            switch (movement)
            {
                case Side.Left:
                    Move(-1, 0);
                    break;
                case Side.Right:
                    Move(1, 0);
                    break;
                case Side.Up:
                    Move(0, 1);
                    break;
                case Side.Down:
                    Move(0, -1);
                    break;
                default:
                    break;
            }
    }

    void Move(int x, int y)
    {
        currI -= y;
        currJ += x;
        transform.position = mapGen.nodes[currI, currJ].transform.position;
    }
}
