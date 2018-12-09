using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class CreationController : BaseController
{
	public Text selectedBuildingText;
	public Image bulldozerImage;
	public GameObject solveWarning;

	private AStar aStar;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateOuterWalls(Utilities.FieldWidth, Utilities.FieldHeight);
		mapGen.SpawnPlayerAndFinish();
		aStar = GetComponent<AStar>();
	}

	public void SelectBuilding(int building)
	{
		Utilities.SelectedBuilding = (Building)building;
	}

	public void DeconstructBuilding(bool value)
	{
		bulldozerImage.color = new Color(bulldozerImage.color.r, bulldozerImage.color.g, bulldozerImage.color.b, value ? 1f : 0.5f);
		Utilities.DeconstructingBuilding = value;
	}

	public void SaveMapToFile()
	{
		if (ValidateMap())
		{
			BinaryFormatter bf = new BinaryFormatter();
			string fileName = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss") + ".dat";
			using (FileStream file = File.Create(Path.Combine(Application.persistentDataPath, fileName)))
			{
				NodesModel nm = new NodesModel(mapGen.nodes, MapGenerator.startNode, MapGenerator.finishNode);

				bf.Serialize(file, nm);
			}

			StartCoroutine(ShowMessage("Лабиринт успешно сохранен!", new Color(0.098f, 0.756f, 0.181f, 0.9725f)));
		}
		else
		{
			StartCoroutine(ShowMessage("Лабиринт не имеет решения!", new Color(0.755f, 0.105f, 0.098f, 0.972f)));
		}
	}

	private bool ValidateMap()
	{
		foreach (var item in mapGen.nodes)
		{
			item.GetComponent<Node>().PushNeighbors(mapGen.nodes);
		}

		return aStar.FindPath(MapGenerator.startNode.GetComponent<Node>(), MapGenerator.finishNode.GetComponent<Node>(), false);
	}

	IEnumerator ShowMessage(string message, Color color)
	{
		solveWarning.SetActive(true);
		solveWarning.GetComponent<Image>().color = color;
		solveWarning.GetComponentInChildren<Text>().text = message;

		for (int i = 0; i < 3; i++)
		{
			yield return new WaitForSecondsRealtime(0.5f);
		}

		solveWarning.SetActive(false);
	}
}