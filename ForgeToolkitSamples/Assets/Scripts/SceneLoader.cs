using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public Camera loaderSceneCamera;

	string loadedScene;


	public void LoadScene(string sceneName)
	{
		//loaderSceneCamera.enabled = false;
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		loadedScene = sceneName;
	}

	public void GoBack()
	{
		SceneManager.UnloadSceneAsync(loadedScene);
		loaderSceneCamera.enabled = true;
	}
}
