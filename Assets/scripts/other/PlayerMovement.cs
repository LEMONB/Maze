﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	private MapGenerator mapGen;
	private GameController gController;

	public int currI = 0;
	public int currJ = 0;

	private int screenWidth;
	private int screenHeight;

	protected void Start()
	{
		if (SceneManager.GetActiveScene().name.Equals("creation"))
			return;

		mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
		gController = GameObject.Find("GameController").GetComponent<GameController>();

		currI = MapGenerator.startNode.GetComponent<Node>().i;
		currJ = MapGenerator.startNode.GetComponent<Node>().j;
		mapGen.nodes[currI, currJ].GetComponent<Node>().IsVisited = true;

		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	protected void Update()
	{
		if (SceneManager.GetActiveScene().name.Equals("creation") || !mapGen.mapIsGenerated || Time.timeScale < 0.1f)
			return;

		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			Move(Side.Left);
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			Move(Side.Right);
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			Move(Side.Up);
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
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

	public void Move(Side side)
	{
		if (gController.controlsImage.activeSelf)
		{
			gController.controlsImage.SetActive(false);
			Utilities.ShowControls = false;
		}

		if (!mapGen.GetComponent<MapGenerator>().nodes[currI, currJ].GetComponent<Node>().WallExists(side))
			switch (side)
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

	public void Move(int x, int y)
	{
		currI -= y;
		currJ += x;
		transform.position = mapGen.nodes[currI, currJ].transform.position;
		mapGen.nodes[currI, currJ].GetComponent<Node>().IsVisited = true;

		if (mapGen.nodes[currI, currJ] == MapGenerator.finishNode)
		{
			gController.LoadScene("main");
		}
	}
}
