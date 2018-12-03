using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{

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
}
