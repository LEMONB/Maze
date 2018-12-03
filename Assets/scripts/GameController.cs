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
    PlayerMovement playerScript;

    public Button showGenerationButton;
    public Button showControlsButton;
    public GameObject controlsImage;

    void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();
        mapGen.GenerateMaze(Utilities.FieldWidth, Utilities.FieldHeight);
        Camera.main.transform.position = new Vector3(Utilities.FieldWidth / 2 - 0.5f, -Utilities.FieldHeight / 2 + 0.5f, Camera.main.transform.position.z);
        Camera.main.orthographicSize = Math.Max(Utilities.FieldHeight, Utilities.FieldWidth) + 1;

        playerScript = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity).GetComponent<PlayerMovement>();

        if (Utilities.ShowControls)
            controlsImage.SetActive(true);
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
