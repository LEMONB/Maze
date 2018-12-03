using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
	private bool[] walls = new bool[4];

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

	void OnMouseDown()
	{
		print("dsadasd");
		if (Utilities.BuildVerticalWall)
		{
			//wallGO = Instantiate(wallPrefab, new Vector3(nodes[i, j].transform.position.x, nodes[i, j].transform.position.y - 0.5f, nodes[i, j].transform.position.z), Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));

			AddWall(Side.Right);
		}
		else
		{
			AddWall(Side.Down);
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
			neighbors.Add(nodes[I, J - 1]);
		}
		if (AllowedMove(Side.Right))
		{
			neighbors.Add(nodes[I, J + 1]);
		}
		if (AllowedMove(Side.Up))
		{
			neighbors.Add(nodes[I - 1, J]);
		}
		if (AllowedMove(Side.Down))
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
