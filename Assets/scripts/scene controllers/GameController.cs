using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : BaseController
{
	public GameObject playerPrefab;
	PlayerMovement playerScript;

	public Button showGenerationButton;
	public Button showControlsButton;
	public GameObject controlsImage;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateMaze(Utilities.FieldWidth, Utilities.FieldHeight);

		playerScript = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity).GetComponent<PlayerMovement>();

		if (Utilities.ShowControls)
			controlsImage.SetActive(true);
	}

	public void Switch()
	{
		Utilities.ShowConstruction = !Utilities.ShowConstruction;

		if (Utilities.ShowConstruction)
			showGenerationButton.GetComponent<Text>().text = "Показ строительства (выкл)";
		else
			showGenerationButton.GetComponent<Text>().text = "Показ строительства (вкл)";
	}

	public void ShowControls()
	{
		controlsImage.SetActive(true);
	}

	public void HintMove()
	{
		Node hintNode = GetComponent<AStar>().GetHint(mapGen.nodes[playerScript.currI, playerScript.currJ].GetComponent<Node>(), mapGen.nodes[mapGen.nodes.GetLength(0) - 1, mapGen.nodes.GetLength(1) - 1].GetComponent<Node>());
		int dI = hintNode.I - playerScript.currI;
		int dJ = hintNode.J - playerScript.currJ;
		playerScript.Move(dI, dJ);
	}
}
