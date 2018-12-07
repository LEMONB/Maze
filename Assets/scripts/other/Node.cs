using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
	private CommonPrefabs prefabs;

	private GameObject[] wallsObject = new GameObject[2];

	public bool[] walls = new bool[4];

	public Sprite visitedNodeSprite;
	public Node previous;
	public List<GameObject> neighbors = new List<GameObject>();

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

	public int SetNumber { get; set; } = 0;

	public int I { get; set; }

	public int J { get; set; }

	public float F { get; set; } = 0;

	public float H { get; set; } = 0;

	public float G { get; set; } = 0;

	public bool IsClosed { get; set; } = false;

	public bool IsOpened { get; set; } = false;

	public bool IsFinish { get; set; } = false;

	public void Start()
	{
		prefabs = FindObjectOfType<CommonPrefabs>();
	}

	protected void OnMouseDown()
	{
		if (!SceneManager.GetActiveScene().name.Equals("creation"))
		{
			return;
		}

		if (Utilities.SelectedBuilding == Building.HorizontalWall)
		{
			if (!WallExists(Side.Down))
			{
				wallsObject[0] = Instantiate(prefabs.WallPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));

				AddWall(Side.Down);
			}
		}
		else if (Utilities.SelectedBuilding == Building.VerticalWall)
		{
			if (!WallExists(Side.Right))
			{
				wallsObject[1] = Instantiate(prefabs.WallPrefab, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.identity);

				AddWall(Side.Right);
			}
		}
		else if (Utilities.SelectedBuilding == Building.RemoveHorizontalWall)
		{
			if (WallExists(Side.Down))
			{
				Destroy(wallsObject[0]);

				RemoveWall(Side.Down);
			}
		}
		else
		{
			if (WallExists(Side.Right))
			{
				Destroy(wallsObject[1]);

				RemoveWall(Side.Right);
			}
		}
	}

	public void AddWall(Side side)
	{
		walls[(int)side] = true;
	}

	public void RemoveWall(Side side)
	{
		walls[(int)side] = false;
	}

	public bool WallExists(Side side)
	{
		return walls[(int)side];
	}

	public void PushNeighbors(GameObject[,] nodes)
	{
		neighbors.Clear();

		//LRUD
		if (!WallExists(Side.Left))
		{
			neighbors.Add(nodes[I, J - 1]);
		}
		if (!WallExists(Side.Right))
		{
			neighbors.Add(nodes[I, J + 1]);
		}
		if (!WallExists(Side.Up))
		{
			neighbors.Add(nodes[I - 1, J]);
		}
		if (!WallExists(Side.Down))
		{
			neighbors.Add(nodes[I + 1, J]);
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
