using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class CreationController : BaseController
{
	public Text selectedBuildingText;

	protected override void Start()
	{
		base.Start();

		mapGen.GenerateOuterWalls(Utilities.FieldWidth, Utilities.FieldHeight);
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

	public void SaveMapToFile()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + @"\" + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss"));

		NodesModel nm = new NodesModel(mapGen.nodes);

		bf.Serialize(file, nm);
		file.Close();
	}

	//public void LoadAllSavesToScrollView()
	//{
	//	contentGO.transform.DetachChildren();

	//	var fileNames = Directory.GetFiles(Application.persistentDataPath).Select(Path.GetFileName).ToArray();
	//	foreach (var item in fileNames)
	//	{
	//		GameObject button = Instantiate(savedFileButton);
	//		button.GetComponentInChildren<Text>().text = item;
	//		button.transform.SetParent(contentGO.transform);
	//		button.GetComponent<Button>().onClick.AddListener(() => CreateMapFromFile(item));
	//		button.GetComponent<Button>().onClick.AddListener(() => SwitchCanvas(GameObject.Find("SavesToLoadCanvas")));
	//		button.GetComponent<Button>().onClick.AddListener(() => SwitchCanvas(GameObject.Find("PauseCanvas")));
	//	}
	//}

	//public void CreateMapFromFile(string fileName)
	//{
	//	FileStream file = File.Open(Application.persistentDataPath + @"\" + fileName, FileMode.Open);
	//	BinaryFormatter bf = new BinaryFormatter();

	//	NodesModel nm = (NodesModel)bf.Deserialize(file);

	//	file.Close();

	//	mapGen.GenerateMaze(nm);
	//}
}

//[Serializable]
//public class NodesModel
//{
//	public int width;
//	public int height;
//	public bool[,,] walls;

//	public NodesModel(GameObject[,] nodes)
//	{
//		height = nodes.GetLength(0);
//		width = nodes.GetLength(1);

//		walls = new bool[height, width, 4];

//		for (int i = 0; i < height; i++)
//		{
//			for (int j = 0; j < width; j++)
//			{
//				for (int k = 0; k < 4; k++)
//				{
//					walls[i, j, k] = nodes[i, j].GetComponent<Node>().walls[k];
//				}
//			}
//		}
//	}
//}