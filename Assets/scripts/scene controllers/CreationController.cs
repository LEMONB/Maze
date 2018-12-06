using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class CreationController : BaseController
{
	public Text selectedBuildingText;

	private ScrollRect savesScrollView;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateOuterWalls(Utilities.FieldWidth, Utilities.FieldHeight);

		savesScrollView = FindObjectOfType<ScrollRect>();
	}

	public void SwitchSelectedBuilding()
	{
		Utilities.SelectedBuilding++;
		if ((int)Utilities.SelectedBuilding == Enum.GetNames(typeof(Building)).Length)
		{
			Utilities.SelectedBuilding = 0;
		}

		switch (Utilities.SelectedBuilding)
		{
			case Building.HorizontalWall:
				selectedBuildingText.text = "_";
				break;
			case Building.VerticalWall:
				selectedBuildingText.text = "|";
				break;
			case Building.RemoveHorizontalWall:
				selectedBuildingText.text = "_X";
				break;
			case Building.RemoveVerticalWall:
				selectedBuildingText.text = "|X";
				break;
			default:
				break;
		}
	}

	private void CreateMapFromFile(GameObject[] nodes)
	{

	}

	public void SaveMapToFile()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + @"\" + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss"));

		NodesModel nm = new NodesModel(mapGen.nodes.GetLength(0), mapGen.nodes.GetLength(1));
		for (int i = 0; i < mapGen.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < mapGen.nodes.GetLength(1); j++)
			{
				nm.nodes[i, j] = mapGen.nodes[i, j].GetComponent<Node>();
			}
		}

		bf.Serialize(file, nm);
		file.Close();
	}

	public void LoadAllSaves()
	{
		var fileNames = Directory.GetFiles(Application.persistentDataPath);
		//savesScrollView.content.
	}
}

[Serializable]
public class NodesModel
{
	public Node[,] nodes;

	public NodesModel(int i, int j)
	{
		nodes = new Node[i, j];
	}
}