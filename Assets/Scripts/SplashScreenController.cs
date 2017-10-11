using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {
	public Text highScoreText;

	void Start () {
		highScoreText.text = string.Format("High Score: {0}", PlayerPrefs.GetInt("highScore"));
	}

	public void EnterGame() {
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}
}
