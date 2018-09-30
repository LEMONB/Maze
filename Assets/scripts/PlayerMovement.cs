using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = 0, Right, Up, Down };

public class PlayerMovement : MonoBehaviour
{
    MapGeneratorKruskal mapGen;
    GameController gController;

    int currI = 0;
    int currJ = 0;

    int screenWidth;
    int screenHeight;

    void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
        gController = GameObject.Find("GameController").GetComponent<GameController>();
        mapGen.nodes[currI, currJ].GetComponent<Node>().IsVisited = true;

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Update()
    {
        if (!mapGen.mapIsGenerated)
            return;

        if (Input.GetKeyDown(KeyCode.A))
            Move(Side.Left);
        if (Input.GetKeyDown(KeyCode.D))
            Move(Side.Right);
        if (Input.GetKeyDown(KeyCode.W))
            Move(Side.Up);
        if (Input.GetKeyDown(KeyCode.S))
            Move(Side.Down);

        // Touch input
        if (Input.GetMouseButtonDown(0))
        {
            int x = (int)Input.mousePosition.x;
            int y = (int)Input.mousePosition.y;

            if (y >= screenHeight / 2f)
            {
                Move(Side.Up);
            }
            else if (x <= screenWidth / 3f && y > x)
            {
                Move(Side.Left);
            }
            else if (x >= 2f / 3f * screenWidth && (screenWidth - x) < y)
            {
                Move(Side.Right);
            }
            else if (y <= screenWidth / 3f && y <= x)
            {
                Move(Side.Down);
            }
        }
    }

    void Move(Side movement)
    {
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
        mapGen.nodes[currI, currJ].GetComponent<Node>().IsVisited = true;
        if (mapGen.nodes[currI, currJ].GetComponent<Node>().isFinish)
        {
            gController.FinishGame();
        }
    }
}
