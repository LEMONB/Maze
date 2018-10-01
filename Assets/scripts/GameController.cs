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

    public Button showGenerationButton;
    public Button showControlsButton;
    public GameObject controlsImage;

    void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
        mapGen.GenerateMaze(Utilities.fieldWidth, Utilities.fieldHeight);
        Camera.main.transform.position = new Vector3(Utilities.fieldWidth / 2 - 0.5f, -Utilities.fieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
        Camera.main.orthographicSize = Math.Max(Utilities.fieldHeight, Utilities.fieldWidth) + 1;

        Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);

        if (Utilities.ShowControls)
            ShowControls();
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
        if (Utilities.ShowControls)
        {
            controlsImage.SetActive(true);
        }
        else
        {
            controlsImage.SetActive(false);
        }
    }
}
