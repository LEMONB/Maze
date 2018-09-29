using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject wallPrefab;
    public GameObject finishPrefab;
    public GameObject playerPrefab;

    private int fieldWidth = 10;
    private int fieldHeight = 10;

    public GameObject[,] nodes;
    int setsGlobalNumber = 1;

    Dictionary<int, List<GameObject>> sets = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        GenerateMaze(fieldWidth, fieldHeight);
        Instantiate(playerPrefab, nodes[0, 0].transform.position, Quaternion.identity);
    }

    void GenerateMaze(int width, int height)
    {
        nodes = new GameObject[height, width];

        for (int i = 0; i < height; i++)        // Spawn nodes and outer walls
        {
            for (int j = 0; j < width; j++)
            {
                if (i == height - 1 && j == width - 1)
                    nodes[i, j] = Instantiate(finishPrefab, new Vector3(3 * j, 3 * (-i), 1), Quaternion.identity);
                nodes[i, j] = Instantiate(nodePrefab, new Vector3(3 * j, 3 * (-i), 1), Quaternion.identity);

                if (i == 0)
                    BuildWall(i, j, Side.Up);
                else if (i == height - 1)
                    BuildWall(i, j, Side.Down);

                if (j == 0)
                    BuildWall(i, j, Side.Left);
                else if (j == width - 1)
                    BuildWall(i, j, Side.Right);
            }
        }

        for (int i = 0; i < height; i++)            // Building Maze itself
        {
            for (int j = 0; j < width; j++)             // Putting each node into a set
            {
                if (nodes[i, j].GetComponent<Node>().SetNumber == 0)
                {
                    // nodes[i, j].GetComponent<Node>().SetNumber = j + 1;
                    sets[setsGlobalNumber].Add(nodes[i, j]);
                    nodes[i, j].GetComponent<Node>().SetNumber = setsGlobalNumber++;
                }
            }

            for (int j = 1; j < width; j++)         // Building Vertical walls and removing redundant sets
            {
                if (i > 0 && nodes[i, j].GetComponent<Node>().SetNumber == nodes[i, j - 1].GetComponent<Node>().SetNumber               // Removing cycles
                && nodes[i, j].GetComponent<Node>().IsAllowed(Side.Up) && nodes[i, j - 1].GetComponent<Node>().IsAllowed(Side.Up))
                {
                    BuildWall(i, j, Side.Left);
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        nodes[i, j].GetComponent<Node>().SetNumber = nodes[i, j - 1].GetComponent<Node>().SetNumber;
                    }
                    else
                    {
                        if (i == height - 1)
                        {
                            if (nodes[i, j].GetComponent<Node>().SetNumber == nodes[i, j - 1].GetComponent<Node>().SetNumber)
                                BuildWall(i, j, Side.Left);
                        }
                        else
                        {
                            BuildWall(i, j, Side.Left);
                        }
                    }
                }
            }

            bool exitFromSetExists = false;
            for (int j = 0; j < width; j++)         // Building Horizontal walls
            {
                if (j > 0 && nodes[i, j - 1].GetComponent<Node>().SetNumber != nodes[i, j].GetComponent<Node>().SetNumber)
                {
                    exitFromSetExists = false;
                }

                if (Random.Range(0, 2) == 0)
                {
                    exitFromSetExists = true;
                }
                else if (exitFromSetExists || (j < width - 1 && nodes[i, j].GetComponent<Node>().SetNumber == nodes[i, j + 1].GetComponent<Node>().SetNumber))
                {
                    BuildWall(i, j, Side.Down);
                }
            }

            if (i < height - 1)
                for (int j = 0; j < width; j++)                 // Creating new row
                {
                    if (nodes[i + 1, j].GetComponent<Node>().IsAllowed(Side.Up))
                    {
                        nodes[i + 1, j].GetComponent<Node>().SetNumber = nodes[i, j].GetComponent<Node>().SetNumber;
                    }
                }
        }
    }


    public void BuildWall(int i, int j, Side side)
    {
        switch (side)
        {
            case Side.Up:
                Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x, nodes[i, j].transform.position.y + 1.5f, nodes[i, j].transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (i - 1 >= 0)
                    nodes[i - 1, j].GetComponent<Node>().AddWall(Side.Down);
                break;
            case Side.Down:
                Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x, nodes[i, j].transform.position.y - 1.5f, nodes[i, j].transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (i + 1 < fieldHeight)
                    nodes[i + 1, j].GetComponent<Node>().AddWall(Side.Up);
                break;
            case Side.Left:
                Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x - 1.5f, nodes[i, j].transform.position.y, nodes[i, j].transform.position.z), Quaternion.identity);
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (j - 1 >= 0)
                    nodes[i, j - 1].GetComponent<Node>().AddWall(Side.Right);
                break;
            case Side.Right:
                Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x + 1.5f, nodes[i, j].transform.position.y, nodes[i, j].transform.position.z), Quaternion.identity);
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (j + 1 < fieldWidth)
                    nodes[i, j + 1].GetComponent<Node>().AddWall(Side.Left);
                break;
            default:
                break;
        }
    }
}
