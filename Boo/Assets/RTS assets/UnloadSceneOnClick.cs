using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnloadSceneOnClick : MonoBehaviour
{
	private Button button;

	// Use this for initialization
	void Start()
	{
		button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(Unload);

		Debug.Log(button);
	}
	
	public void Unload()
	{
		Debug.Log("unload " + gameObject.scene);
		SceneManager.UnloadSceneAsync(gameObject.scene);
	}
}
