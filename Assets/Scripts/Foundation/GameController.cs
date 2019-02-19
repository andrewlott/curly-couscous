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

	void Start () {

		// Add systems here
		RoundSystem rs = new RoundSystem();
		AddSystem(rs);
		ColoringSystem cs = new ColoringSystem();
		AddSystem(cs);
		TouchSystem ts = new TouchSystem();
		AddSystem(ts);

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
}
