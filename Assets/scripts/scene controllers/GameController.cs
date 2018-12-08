using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : BaseController
{
	public Button showGenerationButton;
	public Button showControlsButton;
	public GameObject controlsImage;

	private AStar aStar;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateMaze(Utilities.FieldWidth, Utilities.FieldHeight);
		aStar = GetComponent<AStar>();

		if (Utilities.ShowControls)
			controlsImage.SetActive(true);
	}

	public void Switch()
	{
		Utilities.ShowConstruction = !Utilities.ShowConstruction;

		if (Utilities.ShowConstruction)
			showGenerationButton.GetComponent<Text>().text = "Показ генерации (выкл)";
		else
			showGenerationButton.GetComponent<Text>().text = "Показ генерации (вкл)";
	}

	public void ShowControls()
	{
		controlsImage.SetActive(true);
	}

	public void SolveMaze()
	{
		aStar.FindPath(MapGenerator.startNode.GetComponent<Node>(), MapGenerator.finishNode.GetComponent<Node>());
	}
}
