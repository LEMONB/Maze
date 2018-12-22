using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
	private struct WallStruct
	{
		public GameObject gameObject;
		public GameObject firstAdjacentNode;
		public GameObject secondAdjacentNode;
		public bool horizontal;
	}

	public GameObject[,] nodes;

	public static GameObject startNode;
	public static GameObject finishNode;
	public static GameObject player;
	public static GameObject finish;

	public bool mapIsGenerated = false;

	private List<GameObject> outerWalls = new List<GameObject>();
	private List<WallStruct> walls = new List<WallStruct>();
	private Dictionary<int, List<GameObject>> sets = new Dictionary<int, List<GameObject>>();

	public void GenerateOuterWalls(int width, int height)
	{
		nodes = new GameObject[height, width];

		for (int i = 0; i < height; i++)        // Spawn nodes and outer walls
		{
			for (int j = 0; j < width; j++)
			{
				nodes[i, j] = Instantiate(Resources.Load<GameObject>("node"), new Vector3(j, (-i), 1), Quaternion.identity);
				nodes[i, j].GetComponent<Node>().i = i;
				nodes[i, j].GetComponent<Node>().j = j;

				if (i == 0)
					outerWalls.Add(BuildWall(i, j, Side.Up));
				else if (i == height - 1)
					outerWalls.Add(BuildWall(i, j, Side.Down));

				if (j == 0)
					outerWalls.Add(BuildWall(i, j, Side.Left));
				else if (j == width - 1)
					outerWalls.Add(BuildWall(i, j, Side.Right));
			}
		}
	}

	public void SpawnPlayerAndFinish()
	{
		if (Random.Range(0, 2) == 0)
		{
			finishNode = nodes[Random.Range(0, Utilities.FieldHeight), Utilities.FieldWidth - 1];
			startNode = nodes[Random.Range(0, Utilities.FieldHeight), 0];
		}
		else
		{
			finishNode = nodes[Utilities.FieldHeight - 1, Random.Range(0, Utilities.FieldWidth)];
			startNode = nodes[0, Random.Range(0, Utilities.FieldWidth)];
		}

		player = Instantiate(Resources.Load<GameObject>("player"), startNode.transform.position, Quaternion.identity);
		finish = Instantiate(Resources.Load<GameObject>("finish"), finishNode.transform.position, Quaternion.identity);
	}

	public void GenerateMaze(NodesModel nodesModel)
	{
		ResetMap();

		GenerateOuterWalls(nodesModel.width, nodesModel.height);

		UnpackNodesModel(nodesModel);

		player.transform.position = startNode.transform.position;
		finish.transform.position = finishNode.transform.position;
		player.GetComponent<PlayerMovement>().currI = startNode.GetComponent<Node>().i;
		player.GetComponent<PlayerMovement>().currJ = startNode.GetComponent<Node>().j;

		for (int i = 0; i < nodesModel.height; i++)        // Spawn Vertical Walls
		{
			for (int j = 0; j < nodesModel.width - 1; j++)
			{
				if (nodesModel.walls[i, j, 1])
				{
					WallStruct newWall = new WallStruct
					{
						firstAdjacentNode = nodes[i, j],
						secondAdjacentNode = nodes[i, j + 1],
						gameObject = BuildWall(i, j, Side.Right),
						horizontal = false
					};

					walls.Add(newWall);
				}
			}
		}

		for (int i = 0; i < nodesModel.height - 1; i++)        // Spawn Horizontal Walls
		{
			for (int j = 0; j < nodesModel.width; j++)
			{
				if (nodesModel.walls[i, j, 3])
				{
					WallStruct newWall = new WallStruct
					{
						firstAdjacentNode = nodes[i, j],
						secondAdjacentNode = nodes[i + 1, j],
						gameObject = BuildWall(i, j, Side.Down),
						horizontal = true
					};

					walls.Add(newWall);
				}
			}
		}

		List<GameObject> list = new List<GameObject>();     // Assign every node to one set
		for (int i = 0; i < nodes.GetLength(0); i++)
		{
			for (int j = 0; j < nodes.GetLength(1); j++)
			{
				list.Add(nodes[i, j]);
			}
		}
		sets.Add(0, list);
		UpdateNodesSetNumbers(sets);

		if (Utilities.ShowConstruction)
		{
			StartCoroutine(Step());
		}
		else
		{
			foreach (var wall in walls)
			{
				if (wall.firstAdjacentNode.GetComponent<Node>().setNumber != wall.secondAdjacentNode.GetComponent<Node>().setNumber)
				{
					sets[wall.firstAdjacentNode.GetComponent<Node>().setNumber].AddRange(sets[wall.secondAdjacentNode.GetComponent<Node>().setNumber]);
					sets.Remove(wall.secondAdjacentNode.GetComponent<Node>().setNumber);
					UpdateNodesSetNumbers(sets);
					DeconstructWall(wall);
				}
			}
			mapIsGenerated = true;
		}

		PushNeighbors();
	}

	public void GenerateMaze(int width, int height)
	{
		GenerateOuterWalls(width, height);
		SpawnPlayerAndFinish();

		SpawnWalls(width, height);

		walls.Shuffle();

		for (int i = 0; i < height; i++)        // Assign each node to its own set
		{
			for (int j = 0; j < width; j++)
			{
				List<GameObject> list = new List<GameObject>
				{
					nodes[i, j]
				};
				sets.Add(i * height + j, list);
			}
		}
		UpdateNodesSetNumbers(sets);

		if (Utilities.ShowConstruction)
		{
			StartCoroutine(Step());
		}
		else
		{
			foreach (var wall in walls)
			{
				if (wall.firstAdjacentNode.GetComponent<Node>().setNumber != wall.secondAdjacentNode.GetComponent<Node>().setNumber)
				{
					sets[wall.firstAdjacentNode.GetComponent<Node>().setNumber].AddRange(sets[wall.secondAdjacentNode.GetComponent<Node>().setNumber]);
					sets.Remove(wall.secondAdjacentNode.GetComponent<Node>().setNumber);
					UpdateNodesSetNumbers(sets);
					DeconstructWall(wall);
				}
			}
			mapIsGenerated = true;
		}

		PushNeighbors();
	}

	private void SpawnWalls(int width, int height)
	{
		for (int i = 0; i < height; i++)        // Spawn Vertical Walls
		{
			for (int j = 0; j < width - 1; j++)
			{
				WallStruct newWall = new WallStruct
				{
					firstAdjacentNode = nodes[i, j],
					secondAdjacentNode = nodes[i, j + 1],
					gameObject = BuildWall(i, j, Side.Right),
					horizontal = false
				};

				walls.Add(newWall);
			}
		}

		for (int i = 0; i < height - 1; i++)        // Spawn Horizontal Walls
		{
			for (int j = 0; j < width; j++)
			{
				WallStruct newWall = new WallStruct
				{
					firstAdjacentNode = nodes[i, j],
					secondAdjacentNode = nodes[i + 1, j],
					gameObject = BuildWall(i, j, Side.Down),
					horizontal = true
				};

				walls.Add(newWall);
			}
		}
	}

	private void PushNeighbors()
	{
		foreach (var item in nodes)
		{
			item.GetComponent<Node>().PushNeighbors(nodes);
		}
	}

	private void ResetMap()
	{
		foreach (var item in outerWalls)
		{
			Destroy(item);
		}

		for (int i = 0; i < nodes.GetLength(0); i++)
		{
			for (int j = 0; j < nodes.GetLength(1); j++)
			{
				Destroy(nodes[i, j]);
			}
		}

		sets.Clear();

		foreach (var wall in walls)
		{
			Destroy(wall.gameObject);
		}
		walls = new List<WallStruct>();
	}

	private IEnumerator Step()
	{
		foreach (var wall in walls)
		{
			if (wall.firstAdjacentNode.GetComponent<Node>().setNumber != wall.secondAdjacentNode.GetComponent<Node>().setNumber)
			{
				sets[wall.firstAdjacentNode.GetComponent<Node>().setNumber].AddRange(sets[wall.secondAdjacentNode.GetComponent<Node>().setNumber]);
				sets.Remove(wall.secondAdjacentNode.GetComponent<Node>().setNumber);
				UpdateNodesSetNumbers(sets);
				DeconstructWall(wall);
			}

			yield return new WaitForSeconds(3f / walls.Count);
		}

		mapIsGenerated = true;
	}

	private void UnpackNodesModel(NodesModel nodesModel)
	{
		for (int i = 0; i < nodesModel.height; i++)
		{
			for (int j = 0; j < nodesModel.width; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					nodes[i, j].GetComponent<Node>().walls[k] = nodesModel.walls[i, j, k];
				}
			}
		}

		startNode = nodes[nodesModel.startPosI, nodesModel.startPosJ];
		finishNode = nodes[nodesModel.finishPosI, nodesModel.finishPosJ];
	}

	private void UpdateNodesSetNumbers(Dictionary<int, List<GameObject>> dict)
	{
		foreach (var kvp in dict)
		{
			foreach (var item in kvp.Value)
			{
				item.GetComponent<Node>().setNumber = kvp.Key;
			}
		}
	}

	private GameObject BuildWall(int i, int j, Side side)
	{
		GameObject wallPrefab = Resources.Load<GameObject>("wall");
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
				if (i + 1 < Utilities.FieldHeight)
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
				if (j + 1 < Utilities.FieldWidth)
					nodes[i, j + 1].GetComponent<Node>().AddWall(Side.Left);
				break;
			default:
				wallGO = null;
				break;
		}

		return wallGO;
	}

	private void DeconstructWall(WallStruct wall)
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
