using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : BaseController {
    private static GameController _instance;

    // Modes
    public bool livesMode;
    // TODO: Add time system
    public bool timeMode;

    public GameObject gameplayCanvas;

    // Hax cuz lazy
    public GameObject gameOverCanvas;
    public GameObject background;
    public GameObject mainMenuCanvas;
    public GameObject pauseCanvas;

    private GameObject _player;
    private GameObject _target;
    private List<GameObject> _colorButtons; // ordered

    // Gameover Canvas
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;
    public Text gameOverStreakText;

    // Gameplay Canvas
    public Text gameplayScoreText;
    public Text timerText;
    public Text livesText;
    public Text streakText;

    // Main menu
    public Text mainMenuHighScoreText;

    // Pause Canvas
    public Text pauseScoreText;
    public Text pauseHighScoreText;

    // Gameplay elements
    [SerializeField]
    private int _score = 0;
    public int Score {
        get { return _score; }
        set {
            _score = value;
            scoreDirty = true;
        }
    }
    public bool scoreDirty = true;
    public int initialLives;
    private int _lives;
    public int Lives {
        get { return _lives; }
        set {
            _lives = value;
            livesDirty = true;
        }
    }
    public bool livesDirty = true;
    public int matchStreakForStock = 10;
    public int matchStreak;
    public int maxMatchStreak;
    public int missStreak;
    public float initialTotalTime = 61.0f;
    public float totalTime;
    public float bonusTime = 10.0f;
	public int initialNumberOfButtons = 3;
    public int numberOfButtons;
    public int firstAdLevel = 3;
    public int difficulty = 1;
    public int round = 0;
	public bool initializeGame = true;

    public float timeScale = 1.0f;
    public float lastMatchTime;

    public static GameController Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return _instance;
        }
    }

    void Start() {
        // Add systems here
        RoundSystem rs = new RoundSystem();
        AddSystem(rs);
        LivesSystem ls = new LivesSystem();
        AddSystem(ls);
        StreakSystem ss = new StreakSystem();
        AddSystem(ss);
        ScoreSystem scs = new ScoreSystem();
        AddSystem(scs);
        ColoringSystem cs = new ColoringSystem();
        AddSystem(cs);
        TouchSystem ts = new TouchSystem();
        AddSystem(ts);

        AnimationSystem ans = new AnimationSystem();
        AddSystem(ans);
        UISystem uis = new UISystem();
        AddSystem(uis);
        PauseSystem ps = new PauseSystem();
        AddSystem(ps);
        DestroySystem ds = new DestroySystem();
        AddSystem(ds);

        AdSystem ads = new AdSystem();
        AddSystem(ads);

        Enable();
		this.initializeGame = true;
        ExtraSetup();
    }

    private void ExtraSetup() {
        this.mainMenuHighScoreText.text = string.Format("{0}", PlayerPrefs.GetInt("highScore"));
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
        this.round = 1;
        this.Lives = this.initialLives;
        this.Score = 0;
        this.difficulty = 1;
		this.numberOfButtons = this.initialNumberOfButtons;
        Disable();
    }

    public override void OnUpdate() {
        if (Time.timeScale != this.timeScale) {
            Time.timeScale = Mathf.Max(this.timeScale, 0.0f);
        }
    }

    public void StartGame() {
        Enable();
        AnimateCubes(true, null);
    }

    public void NewGame() {
        Restart();
        StartGame();
        mainMenuCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameplayCanvas.SetActive(true);
        /*
        AnimationComponent.Animate(
            mainMenuCanvas.gameObject,
            "main menu_off",
            false,
            null,
           "anim_mainmenu_off"
        );
        */
    }

    public void EndGame() {
        AnimateCubes(false, EndGameCallback);
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
        AnimateCubes(false, NewGameCallback);
        /*
        AnimationComponent.Animate(
            mainMenuCanvas.gameObject,
            "main menu on",
            false,
            null,
            "anim_mainmenu_on"
        );
        */
        //SceneManager.LoadScene("SplashScene", LoadSceneMode.Single);
    }

    private void AnimateCubes(bool show, AnimationComponent.CallbackFunction callback) {
        string trigger = "isOn" ;
        string callbackState = show ? "anim_appear" : "anim_idle";
        AnimationComponent.CallbackFunction doOnceCallback = null;
        if (!show) {
            doOnceCallback = callback;
        }

        AnimationComponent.Animate(this.Player, trigger, show, null, callbackState);
        AnimationComponent.Animate(this.Target, trigger, show, doOnceCallback, callbackState);
        for (int i = 0; i < this.numberOfButtons; i++) {
            AnimationComponent.Animate(this.ColorButtons[i], trigger, show, null, callbackState);
        }
    }

	public void NewGameCallback(GameObject g) {
		mainMenuCanvas.gameObject.SetActive(true);
		gameplayCanvas.SetActive(false);
	}

	public void EndGameCallback(GameObject g) {
        gameOverCanvas.gameObject.SetActive(true);
        gameplayCanvas.SetActive(false);
    }

    // Properties
    public List<GameObject> ColorButtons {
        get {
            if (this._colorButtons == null) {
                this._colorButtons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button Cube"));
                this._colorButtons.Sort(CompareGameObjectByName);
            }
            return this._colorButtons;
        }
    }

    private static int CompareGameObjectByName(GameObject a, GameObject b) {
        return string.Compare(a.name, b.name, System.StringComparison.Ordinal);
    }

    public GameObject Player {
        get {
            if (this._player == null) {
                this._player = GameObject.FindGameObjectWithTag("Player Cube");
            }
            return this._player;
        }
    }

    public GameObject Target {
        get {
            if (this._target == null) {
                this._target = GameObject.FindGameObjectWithTag("Target Cube");
            }
            return this._target;
        }
    }
}
