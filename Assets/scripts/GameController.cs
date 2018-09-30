using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    MapGeneratorKruskal mapGen;
    public GameObject playerPrefab;

    public Text widthInput;
    public Text heightInput;
    public Button setSizeButton;
    public Toggle showConstrToggle;


    // GameObject playerGO;

    void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
        mapGen.GenerateMaze(Utilities.fieldWidth, Utilities.fieldHeight);
        Camera.main.transform.position = new Vector3(Utilities.fieldWidth / 2 - 0.5f, -Utilities.fieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
        Camera.main.orthographicSize = Math.Max(Utilities.fieldHeight, Utilities.fieldWidth) + 1;

        showConstrToggle.isOn = Utilities.ShowConstruction;
        // playerGO = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);
        Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);
    }

    public void FinishGame()
    {
        if (widthInput.text != "" && heightInput.text != "")
        {
            int width = int.Parse(widthInput.text);
            int height = int.Parse(heightInput.text);
            if (width >= 3 && width <= 50 && height >= 3 && height <= 50 && height >= width)
            {
                Utilities.fieldWidth = width;
                Utilities.fieldHeight = height;
            }
        }

        Utilities.ShowConstruction = showConstrToggle.isOn;

        SceneManager.LoadScene("main");
        // Destroy(playerGO);
        // playerGO = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);
    }
}
