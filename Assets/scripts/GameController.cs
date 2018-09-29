using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    MapGeneratorKruskal mapGen;
    public GameObject playerPrefab;


    GameObject playerGO;

    void Start()
    {
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();

        playerGO = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);
    }

    void Update()
    {

    }

    public void FinishGame()
    {
        SceneManager.LoadScene("main");
        // Destroy(playerGO);
        // playerGO = Instantiate(playerPrefab, mapGen.nodes[0, 0].transform.position, Quaternion.identity);
    }
}
