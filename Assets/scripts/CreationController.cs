﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreationController : MonoBehaviour
{
	private MapGeneratorKruskal mapGen;

	public Text hintButtonText;

	void Start()
	{
		mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
		mapGen.GenerateOuterWalls(Utilities.FieldWidth, Utilities.FieldHeight);

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

	public void SwitchWallTypeToBuild()
	{
		Utilities.BuildVerticalWall = !Utilities.BuildVerticalWall;
		hintButtonText.text = Utilities.BuildVerticalWall ? "|" : "_";
	}
}
