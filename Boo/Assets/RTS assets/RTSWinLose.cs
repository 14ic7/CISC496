using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RTSWinLose : MonoBehaviour {
	readonly Color WIN_COLOUR = new Color(0.77f, 0.85f, 0.06f); // light green
	readonly Color LOSE_COLOUR = new Color(0.93f, 0.11f, 0.14f); // light red

	CanvasGroup canvasGroup;
	Image background;

	void Update() {
		if (Mathf.Abs(canvasGroup.alpha - 1.0f) > 0.0001f) {
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.8f, 0.5f * Time.deltaTime);
		}
	}

	public void win() {
		start(WIN_COLOUR, "YOU WIN");
	}
	public void lose() {
		start(LOSE_COLOUR, "YOU LOSE");
	}
	void start(Color bgColour, string message) {
		enabled = true;
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;

		transform.GetChild(0).GetComponent<Image>().color = bgColour;
		transform.GetChild(1).GetComponent<Text>().text = message;
		
		Button button = transform.GetChild(2).GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(quitGame);

		button = transform.GetChild(3).GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(playAgain);
	}

	// called by UI button press
	public void quitGame() {
		Application.Quit();
	}

	// called by UI button press
	public void playAgain() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
