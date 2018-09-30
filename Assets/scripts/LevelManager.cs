using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject contextCanvas;
    GameObject contextCanvasTemp;
    GameObject currentCanvas;

    public GameObject connectionCanvas;

    private void Start()
    {
        currentCanvas = mainMenuCanvas;

        ChangeCanvas(mainMenuCanvas);

        //GameObject[] buttons = FindObjectsOfType<GameObject>();

        //foreach (GameObject buttonGO in buttons)
        //{
        //	Debug.Log(buttonGO.name);
        //	if (buttonGO.GetComponent<Button>() != null)
        //	{
        //		if (buttonGO.GetComponent<Text>() != null)
        //		{
        //			buttonGO.GetComponent<Text>().color = allowedColors[Random.Range(0, 3)];
        //		}
        //		else if (buttonGO.GetComponent<Image>() != null)
        //		{
        //			buttonGO.GetComponent<Image>().color = allowedColors[Random.Range(0, 3)];
        //		}
        //	}
        //}

        //PlayerPrefs.DeleteAll();

        // UpdatePoints();

        // if (Utilities.triedToConnectToGooglePS == false)        //needed for not trying to connect when we get back to menu from any other scene
        // {
        // 	PlayGamesPlatform.Activate();
        // 	ConnectToGooglePlayServices();
        // 	Utilities.triedToConnectToGooglePS = true;
        // }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentCanvas == mainMenuCanvas)
            {
                ContextCanvasCall("quit");
            }
            else
            {
                ChangeCanvas(mainMenuCanvas);
            }
        }
    }

    public void ChangeCanvas(GameObject newCanvas)
    {
        currentCanvas.SetActive(false);
        newCanvas.SetActive(true);

        currentCanvas = newCanvas;
    }

    public void ContextCanvasCall(string typeOfContext)
    {
        switch (typeOfContext)
        {
            case "connection":
                {
                    contextCanvasTemp = Instantiate(connectionCanvas);
                    GameObject.Find("Yes").GetComponent<Button>().onClick.AddListener(() => ContextOption(true, 2));
                    break;
                }
            case "settings":
                {
                    contextCanvasTemp = Instantiate(contextCanvas);
                    GameObject.Find("Yes").GetComponent<Button>().onClick.AddListener(() => ContextOption(true, 1));
                    break;
                }
            default:
                {
                    contextCanvasTemp = Instantiate(contextCanvas);
                    GameObject.Find("Yes").GetComponent<Button>().onClick.AddListener(() => ContextOption(true, 0));
                    break;
                }
        }

        GameObject.Find("No").GetComponent<Button>().onClick.AddListener(() => ContextOption(false, 0));
    }

    public void ContextOption(bool option, int currMenuType)
    {
        switch (currMenuType)
        {
            case 0: //mainMenu
                {
                    if (option == true)
                    {
                        Application.Quit();
                    }
                    break;
                }
            case 1: //settings
                {
                    if (option == true)
                    {
                        PlayerPrefs.DeleteAll();
                        // UpdatePoints();
                    }
                    break;
                }
            // case 2: //connection
            // 	{
            // 		if (option == true)
            // 		{
            // 			ConnectToGooglePlayServices();
            // 		}
            // 		break;
            // 	}
            default:
                break;
        }

        Destroy(contextCanvasTemp);
    }

    // public GameObject summaryPoints;

    // public void UpdatePoints()
    // {
    // 	summaryPoints.GetComponent<Text>().text = PlayerPrefs.GetFloat("SummaryPoints", 0f).ToString("F0");
    // }

    // public void ChangeMode(int newMode)
    // {
    // 	Utilities.ChangeMode(newMode);
    // }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // public void RateMyApp()
    // {
    // 	Application.OpenURL("market://details?id=com.LemonGames.Slider");
    // }

    // public void ConnectToGooglePlayServices()
    // {
    // 	if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
    // 	{
    // 		Social.localUser.Authenticate((bool succes) =>
    // 		{
    // 			if (succes)
    // 			{
    // 				//((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.BOTTOM);
    // 				Utilities.UnlockAchievement(SliderResources.achievement_hi);
    // 			}
    // 		});
    // 	}
    // }

    public void ShowLeaderboardsUI()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            ContextCanvasCall("connection");
        }
    }

    public void ShowAchievementsUI()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            ContextCanvasCall("connection");
        }
    }
}
