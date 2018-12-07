using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
	protected MapGeneratorKruskal mapGen;

	public GameObject savedFileButton;
	public GameObject contentGO;

	protected virtual void Start()
	{
		mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();

		Camera.main.transform.position = new Vector3(Utilities.FieldWidth / 2 - 0.5f, -Utilities.FieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
		Camera.main.orthographicSize = Math.Max(Utilities.FieldHeight, Utilities.FieldWidth) + 1;

		savedFileButton = Resources.Load("SavedFileButton") as GameObject;
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

	public void LoadAllSavesToScrollView()
	{
		contentGO = GameObject.Find("Content");
		contentGO.transform.DetachChildren();

		var fileNames = Directory.GetFiles(Application.persistentDataPath).Select(Path.GetFileName).ToArray();
		foreach (var item in fileNames)
		{
			GameObject button = Instantiate(savedFileButton);
			button.GetComponentInChildren<Text>().text = item;
			button.transform.SetParent(contentGO.transform);
			button.GetComponent<Button>().onClick.AddListener(() => CreateMapFromFile(item));
			button.GetComponent<Button>().onClick.AddListener(() => SwitchCanvas(GameObject.Find("SavesToLoadCanvas")));
			button.GetComponent<Button>().onClick.AddListener(() => SwitchCanvas(GameObject.Find("PauseCanvas")));
		}
	}

	public void CreateMapFromFile(string fileName)
	{
		FileStream file = File.Open(Application.persistentDataPath + @"\" + fileName, FileMode.Open);
		BinaryFormatter bf = new BinaryFormatter();

		NodesModel nm = (NodesModel)bf.Deserialize(file);

		file.Close();

		mapGen.GenerateMaze(nm);
	}
}

[Serializable]
public class NodesModel
{
	public int width;
	public int height;
	public bool[,,] walls;

	public NodesModel(GameObject[,] nodes)
	{
		height = nodes.GetLength(0);
		width = nodes.GetLength(1);

		walls = new bool[height, width, 4];

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					walls[i, j, k] = nodes[i, j].GetComponent<Node>().walls[k];
				}
			}
		}
	}
}