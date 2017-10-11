using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public int initialTime = 60;
	public int bonusTime = 10;
	public float allowableColorDifference = 0.1f;

	// Outlets
	public Text timerText;
	public Text scoreText;
	public Image userColorObject;
	public Image targetColorObject;
	public Image previousColorObject;

	private float _startTime;
	private int _accruedTime = 0;
	private int _numberOfColors = 0;
	private bool _timerPaused = false;
	private int _score = 0;

	void Start () {
		_startTime = Time.time + 0.99f;
		UpdateScoreText();

		StartCoroutine(ResetCoroutine());
	}
	
	void Update () {
		UpdateTimerText();
		if (CheckLose()) {
			Lose();
		}
	}

	#region Game

	private IEnumerator ResetCoroutine(float delay = 0.0f) {
		yield return new WaitForSeconds(delay);

		Reset();
	}

	private void Reset() {
		ResetUserColor();
		SetPreviousColor();
		GenerateTargetColor();
	}
		
	private void Win() {
		_accruedTime += bonusTime;
		AddToScore();
		StartCoroutine(ResetCoroutine(0.5f));
	}

	private void Lose() {
		_timerPaused = true;
		StartCoroutine(LoadSplashScreenCoroutine(1.0f));
	}

	private IEnumerator LoadSplashScreenCoroutine(float delay = 0.0f) {
		yield return new WaitForSeconds(delay);

		LoadSplashScreen();
	}

	private void LoadSplashScreen() {
		SceneManager.LoadScene("SplashScene", LoadSceneMode.Single);
	}

	private bool CheckWin() {
		return ColorDifference() <= allowableColorDifference;
	}

	private bool CheckLose() {
		return RemainingTime() <= 0.0f;
	}
	#endregion

	#region Color
	private void ResetUserColor() {
		userColorObject.color = Color.white;
		_numberOfColors = 0;
	}

	private void SetPreviousColor() {
		previousColorObject.color = targetColorObject.color;
	}

	private void GenerateTargetColor() {
		float randomR = Random.Range(0.0f, 1.0f);
		float randomG = Random.Range(0.0f, 1.0f);
		float randomB = Random.Range(0.0f, 1.0f);

		targetColorObject.color = new Color(randomR, randomG, randomB);
	}

	private float ColorDifference() {
		float diffR = Mathf.Abs(userColorObject.color.r - targetColorObject.color.r);
		float diffG = Mathf.Abs(userColorObject.color.g - targetColorObject.color.g);
		float diffB = Mathf.Abs(userColorObject.color.b - targetColorObject.color.b);

		return (diffR + diffG + diffB) / 3.0f;
	}

	#endregion

	#region Score

	private void AddToScore(int amount = 1) {
		_score += amount;
		int highScore = PlayerPrefs.GetInt("highScore");
		if (_score > highScore) {
			PlayerPrefs.SetInt("highScore", _score);
		}
		UpdateScoreText();
	}

	private void UpdateScoreText() {
		scoreText.text = string.Format("Score: {0}", _score);
	}

	#endregion

	#region Time

	private void UpdateTimerText() {
		if (_timerPaused) {
			return;
		}
		int remainingTime = RemainingTime();
		int minutes = remainingTime / 60;
		int seconds = remainingTime % 60;
		timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	private int RemainingTime() {
		return (int)(_startTime + initialTime + _accruedTime - Time.time);
	}

	#endregion

	#region Color Button Input

	public void ColorButtonPressed(Image image) {
		if (_timerPaused) {
			return;
		}

		Color c = image.color;

		Color currentColor = userColorObject.color;

		Color newColor = (c + (currentColor * _numberOfColors)) / (_numberOfColors + 1);

		userColorObject.color = newColor;
		_numberOfColors++;

		if (CheckWin()) {
			Win();
		}
	}

	public void EraserButtonPressed() {
		if (_timerPaused) {
			return;
		}

		ResetUserColor();
	}

	public void BackButtonPressed() {
		LoadSplashScreen();
	}

	#endregion
}
