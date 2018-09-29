using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGeneratorKruskal : MonoBehaviour
{
    struct WallStruct
    {
        public GameObject gameObject;
        public GameObject firstAdjacentNode;
        public GameObject secondAdjacentNode;
        public bool horizontal;
    }

    public GameObject nodePrefab;
    public GameObject wallPrefab;
    public GameObject finishPrefab;
    public GameObject playerPrefab;

    private int fieldWidth = 10;
    private int fieldHeight = 10;

    public GameObject[,] nodes;

    private List<WallStruct> walls = new List<WallStruct>();
    private Dictionary<int, List<GameObject>> sets = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        GenerateMaze(fieldWidth, fieldHeight);
        Instantiate(playerPrefab, nodes[0, 0].transform.position, Quaternion.identity);
        Camera.main.transform.position = new Vector3(fieldWidth / 2, -fieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
        Camera.main.orthographicSize = fieldHeight / 2 + 1;
    }

    void GenerateMaze(int width, int height)
    {
        nodes = new GameObject[height, width];

        for (int i = 0; i < height; i++)        // Spawn nodes and outer walls
        {
            for (int j = 0; j < width; j++)
            {
                if (i == height - 1 && j == width - 1)
                    nodes[i, j] = Instantiate(finishPrefab, new Vector3(j, (-i), 1), Quaternion.identity);
                else
                    nodes[i, j] = Instantiate(nodePrefab, new Vector3(j, (-i), 1), Quaternion.identity);

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

        /////////////////
        for (int i = 0; i < height; i++)        // Spawn Vertical Walls
        {
            for (int j = 0; j < width - 1; j++)
            {
                WallStruct newWall = new WallStruct();
                newWall.firstAdjacentNode = nodes[i, j];
                newWall.secondAdjacentNode = nodes[i, j + 1];
                newWall.gameObject = BuildWall(i, j, Side.Right);
                newWall.horizontal = false;

                walls.Add(newWall);
            }
        }

        for (int i = 0; i < height - 1; i++)        // Spawn Horizontal Walls
        {
            for (int j = 0; j < width; j++)
            {
                WallStruct newWall = new WallStruct();
                newWall.firstAdjacentNode = nodes[i, j];
                newWall.secondAdjacentNode = nodes[i + 1, j];
                newWall.gameObject = BuildWall(i, j, Side.Down);
                newWall.horizontal = true;

                walls.Add(newWall);
            }
        }

        walls.Shuffle();
        /////////////////


        /////////////////
        for (int i = 0; i < height; i++)        // Assign each node to its own set
        {
            for (int j = 0; j < width; j++)
            {
                List<GameObject> list = new List<GameObject>();
                list.Add(nodes[i, j]);
                sets.Add(i * height + j, list);
            }
        }
        UpdateNodesSetNumbers(sets);
        /////////////////


        foreach (var wall in walls)
        {
            if (wall.firstAdjacentNode.GetComponent<Node>().SetNumber != wall.secondAdjacentNode.GetComponent<Node>().SetNumber)
            {
                sets[wall.firstAdjacentNode.GetComponent<Node>().SetNumber].AddRange(sets[wall.secondAdjacentNode.GetComponent<Node>().SetNumber]);
                sets.Remove(wall.secondAdjacentNode.GetComponent<Node>().SetNumber);
                UpdateNodesSetNumbers(sets);
                DeconstructWall(wall);
            }
        }
    }

    private void UpdateNodesSetNumbers(Dictionary<int, List<GameObject>> dict)
    {
        foreach (var kvp in dict)
        {
            foreach (var item in kvp.Value)
            {
                item.GetComponent<Node>().SetNumber = kvp.Key;
            }
        }
    }

    public GameObject BuildWall(int i, int j, Side side)
    {
        GameObject wallGO;
        switch (side)
        {
            case Side.Up:
                wallGO = Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x, nodes[i, j].transform.position.y + 0.5f, nodes[i, j].transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (i - 1 >= 0)
                    nodes[i - 1, j].GetComponent<Node>().AddWall(Side.Down);
                break;
            case Side.Down:
                wallGO = Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x, nodes[i, j].transform.position.y - 0.5f, nodes[i, j].transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (i + 1 < fieldHeight)
                    nodes[i + 1, j].GetComponent<Node>().AddWall(Side.Up);
                break;
            case Side.Left:
                wallGO = Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x - 0.5f, nodes[i, j].transform.position.y, nodes[i, j].transform.position.z), Quaternion.identity);
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (j - 1 >= 0)
                    nodes[i, j - 1].GetComponent<Node>().AddWall(Side.Right);
                break;
            case Side.Right:
                wallGO = Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x + 0.5f, nodes[i, j].transform.position.y, nodes[i, j].transform.position.z), Quaternion.identity);
                nodes[i, j].GetComponent<Node>().AddWall(side);
                if (j + 1 < fieldWidth)
                    nodes[i, j + 1].GetComponent<Node>().AddWall(Side.Left);
                break;
            default:
                wallGO = null;
                break;
        }

        return wallGO;
    }

    void DeconstructWall(WallStruct wall)
    {
        Destroy(wall.gameObject);
        if (wall.horizontal)
        {
            wall.firstAdjacentNode.GetComponent<Node>().RemoveWall(Side.Down);
            wall.secondAdjacentNode.GetComponent<Node>().RemoveWall(Side.Up);
        }
        else
        {
            wall.firstAdjacentNode.GetComponent<Node>().RemoveWall(Side.Right);
            wall.secondAdjacentNode.GetComponent<Node>().RemoveWall(Side.Left);
        }
    }
}
