using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : BaseController {
	public Canvas CanvasObject;

	// Hax cuz lazy
	public GameObject background;
	public GameObject playerBox;
	public GameObject targetBox;
	public List<GameObject> colorButtons; // ordered

	public Text scoreText;
	public Text timerText;

	public float totalTime = 61.0f;
	public float bonusTime = 10.0f;

	public float timeScale = 1.0f;

	void Start () {

		// Add systems here
		RoundSystem rs = new RoundSystem();
		AddSystem(rs);
		ColoringSystem cs = new ColoringSystem();
		AddSystem(cs);
		TouchSystem ts = new TouchSystem();
		AddSystem(ts);

		AnimationSystem ans = new AnimationSystem();
		AddSystem(ans);
		UISystem uis = new UISystem();
		AddSystem(uis);
		DestroySystem ds = new DestroySystem();
		AddSystem(ds);


		Enable();
		StartGame();
		ExtraSetup();
	}

	private void ExtraSetup() {
		GameObject debug = GameObject.Find("Debug");
		if (debug != null) {
			#if UNITY_EDITOR
			debug.SetActive(true);
			#else
			debug.SetActive(false);
			#endif
		}
	}
		
	public void Restart() {
		Disable();
		Systems.Clear();
		SceneManager.LoadScene("Main");
	}

	public override void OnUpdate() {
		if (Time.timeScale != this.timeScale) {
			Time.timeScale = Mathf.Max(this.timeScale, 0.0f);
		}
	}

	public void StartGame() {
		
	}
		
	public void Pause() {
		PauseComponent pc = gameObject.GetComponent<PauseComponent>();
		if (pc != null) {
			GameObject.Destroy(pc);
		} else {
			gameObject.AddComponent<PauseComponent>();
		}
	}

	public void OnBack() {

	}
}
