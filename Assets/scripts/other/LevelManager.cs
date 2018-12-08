using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		Camera.main.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(Camera.main.aspect * Camera.main.orthographicSize * 2, Camera.main.orthographicSize * 2);
		currentCanvas = mainMenuCanvas;

		ChangeCanvas(mainMenuCanvas);

		if (PlayerPrefs.GetInt("FirstLaunch", 0) == 0)
		{
			Utilities.ShowControls = true;
			PlayerPrefs.SetInt("FirstLaunch", 1);
		}
		else
		{
			Utilities.ShowControls = false;
		}

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

						DirectoryInfo dI = new DirectoryInfo(Application.persistentDataPath);
						FileInfo[] fileInfos = dI.GetFiles();
						foreach (var item in fileInfos)
						{
							item.Delete();
						}
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

	//public void ChangeMode(int newMode)
	//{
	//    switch (newMode)
	//    {
	//        case 0:
	//            Utilities.FieldHeight = 10;
	//            Utilities.FieldWidth = 10;
	//            break;
	//        case 1:
	//            Utilities.FieldHeight = 20;
	//            Utilities.FieldWidth = 20;
	//            break;
	//        case 2:
	//            Utilities.FieldHeight = 30;
	//            Utilities.FieldWidth = 30;
	//            break;
	//        default:
	//            break;
	//    }
	//}

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
