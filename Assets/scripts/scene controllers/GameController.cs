using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : BaseController
{
	private PlayerMovement playerMovement;

	public Button showGenerationButton;
	public Button showControlsButton;
	public GameObject controlsImage;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateMaze(Utilities.FieldWidth, Utilities.FieldHeight);

		playerMovement = MapGenerator.player.GetComponent<PlayerMovement>();

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

	public void HintMove()
	{
		Node hintNode = GetComponent<AStar>().GetHint(mapGen.nodes[playerMovement.currI, playerMovement.currJ].GetComponent<Node>(), mapGen.nodes[mapGen.nodes.GetLength(0) - 1, mapGen.nodes.GetLength(1) - 1].GetComponent<Node>());
		int dI = hintNode.i - playerMovement.currI;
		int dJ = hintNode.j - playerMovement.currJ;
		playerMovement.Move(dI, dJ);
	}
}
