using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreationController : MonoBehaviour
{
	private MapGeneratorKruskal mapGen;

	public Text selectedBuildingText;

	private ScrollRect savesScrollView;

	void Start()
	{
		mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
		mapGen.GenerateOuterWalls(Utilities.FieldWidth, Utilities.FieldHeight);

		savesScrollView = FindObjectOfType<ScrollRect>();

		Camera.main.transform.position = new Vector3(Utilities.FieldWidth / 2 - 0.5f, -Utilities.FieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
		Camera.main.orthographicSize = Math.Max(Utilities.FieldHeight, Utilities.FieldWidth) + 1;
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		Time.timeScale = 1;
	}

	public void SwitchCanvas(GameObject canvas)
	{
		canvas.SetActive(!canvas.activeSelf);

		Time.timeScale = canvas.activeSelf ? 0 : 1;
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
		FileStream file = File.Create(Application.persistentDataPath + DateTime.Now.ToString() + ".sav");
		bf.Serialize(file, mapGen.nodes);
		file.Close();
	}

	public void LoadAllSaves()
	{
		var fileNames = Directory.GetFiles(Application.persistentDataPath);
		savesScrollView.content.
	}
}
