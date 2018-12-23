using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
	private GameObject[] wallObjects = new GameObject[2];

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

	public int setNumber = 0;

	public int i;
	public int j;
	public float f = 0;
	public float h = 0;
	public float g = 0;
	public bool isClosed = false;
	public bool isOpened = false;

	protected void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!SceneManager.GetActiveScene().name.Equals("creation") || Time.timeScale < 0.1f)
			return;

		if (Utilities.SelectedBuilding == Building.HorizontalWall)
		{
			if (Utilities.DeconstructingBuilding)
			{
				if (WallExists(Side.Down))
				{
					Destroy(wallObjects[0]);

					RemoveWall(Side.Down);
				}
			}
			else
			{
				if (!WallExists(Side.Down))
				{
					wallObjects[0] = Instantiate(Resources.Load<GameObject>("wall"), new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));

					AddWall(Side.Down);
				}
			}
		}
		else if (Utilities.SelectedBuilding == Building.VerticalWall)
		{
			if (Utilities.DeconstructingBuilding)
			{
				if (WallExists(Side.Right))
				{
					Destroy(wallObjects[1]);

					RemoveWall(Side.Right);
				}
			}
			else
			{
				if (!WallExists(Side.Right))
				{
					wallObjects[1] = Instantiate(Resources.Load<GameObject>("wall"), new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.identity);

					AddWall(Side.Right);
				}
			}
		}
		else if (Utilities.SelectedBuilding == Building.Start)
		{
			MapGenerator.startNode = this.gameObject;
			MapGenerator.player.transform.position = transform.position;
		}
		else
		{
			MapGenerator.finishNode = this.gameObject;
			MapGenerator.finish.transform.position = transform.position;

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
			neighbors.Add(nodes[i, j - 1]);
		}
		if (!WallExists(Side.Right))
		{
			neighbors.Add(nodes[i, j + 1]);
		}
		if (!WallExists(Side.Up))
		{
			neighbors.Add(nodes[i - 1, j]);
		}
		if (!WallExists(Side.Down))
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

	public void ResetNode()
	{
		isClosed = false;
		isOpened = false;
		f = 0;
		h = 0;
		g = 0;
	}

	private void OnDestroy()
	{
		foreach (var item in wallObjects)
		{
			Destroy(item);
		}
	}
}
