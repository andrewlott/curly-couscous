using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : BaseController {
    private static GameController _instance;

    public Camera mainCamera;
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
    public GameObject livesContainer;
    //public Text livesText;
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
    public Color previousColor;
    public Color currentColor;

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
    public int round = 1;
    public int easyDifficulty = 1;
    public int mediumDifficulty = 5;
    public int hardDifficulty = 10;
    public int intenseDifficulty = 15;
	public bool initializeGame = false;
    public bool isPlaying = false;

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
		GameSystem gs = new GameSystem();
		AddSystem(gs);
		TutorialSystem tus = new TutorialSystem();
		AddSystem(tus);

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

    private void ScaleCamera() {
        float sceneWidth = 5.5f;
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        mainCamera.orthographicSize = desiredHalfHeight;
    }

    private void ExtraSetup() {
        ScaleCamera();
		GameObject debug = GameObject.Find("Debug");
        if (debug != null) {
#if UNITY_EDITOR
            debug.SetActive(true);
#else
			debug.SetActive(false);
#endif
        }

        ShowMainMenu();
        this.mainMenuHighScoreText.text = string.Format("{0}", PlayerPrefs.GetInt("highScore"));
        previousColor = Utils.RandomColor();
        background.GetComponent<SpriteRenderer>().color = previousColor;
    }

    private void ShowMainMenu() {
        HideCanvas(gameplayCanvas);
        ShowCanvas(mainMenuCanvas);
        PlayParticleSystem("bgParticlesMenus", true);
    }

    private void ShowGame() {
        HideCanvas(mainMenuCanvas);
        HideCanvas(gameOverCanvas);
        ShowCanvas(gameplayCanvas);
    }

    public void ShowGameOver() {
        HideCanvas(gameplayCanvas);
        ShowCanvas(gameOverCanvas);
        GameSystem.isPlaying = false;
    }

    public void Restart() {
        this.round = 1;
        this.Lives = this.initialLives;
        this.Score = 0;
        this.difficulty = 1;
		this.matchStreak = 0;
		this.missStreak = 0;
		this.totalTime = this.initialTotalTime;
		this.numberOfButtons = this.initialNumberOfButtons;
		this.initializeGame = true;
    }

    public override void OnUpdate() {
        if (Time.timeScale != this.timeScale) {
            Time.timeScale = Mathf.Max(this.timeScale, 0.0f);
        }
    }

    public void OnBack() {
        GameObject.Destroy(Pool.Instance.ComponentForType(typeof(AdComponent)));

        this.gameObject.AddComponent<EndGameComponent>();
        this.gameObject.AddComponent<EndTutorialComponent>();
        ShowMainMenu();
    }

    public void NewGame() {
        Restart();
        ShowGame();
        this.gameObject.AddComponent<StartGameComponent>();
    }

    public void NewTutorial() {
        Restart();
        ShowGame();
        this.gameObject.AddComponent<StartTutorialComponent>();
    }

    public void Pause() {
        PauseComponent pc = gameObject.GetComponent<PauseComponent>();
        if (pc != null) {
            GameObject.Destroy(pc);
        } else {
            gameObject.AddComponent<PauseComponent>();
        }
    }

    public void ShowCanvas(GameObject g) {
        if (!g.activeInHierarchy) {
            g.SetActive(true);
        }
        g.GetComponent<Animator>().SetBool("screenOn", true);
    }

    public void HideCanvas(GameObject g) {
        if (g.activeInHierarchy) {
            g.GetComponent<Animator>().SetBool("screenOn", false);
        }
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

    public void PlayParticleForDifficulty() {
        if (!gameplayCanvas.activeInHierarchy) {
            return;
        }
        if (Lives == 1) {
            PlayParticleSystem("bgParticlesOneLifeLeft", true);
        } else if (difficulty == easyDifficulty) {
            PlayParticleSystem("bgParticlesEasy", true);
        } else if (difficulty == mediumDifficulty) {
            PlayParticleSystem("bgParticlesMedium", true);
        } else if (difficulty == hardDifficulty) {
            PlayParticleSystem("bgParticlesHard", true);
        } else if (difficulty == intenseDifficulty) {
            PlayParticleSystem("bgParticlesIntense", true);
        }
    }

    public void PlayParticleSystem(string name, bool disableOthers) {
        List<string> allNames = new List<string> {
            "bgParticlesMenus",
            "bgParticlesEasy",
            "bgParticlesMedium",
            "bgParticlesHard",
            "bgParticlesIntense",
            "bgParticlesOneLifeLeft"
        };
        ParticleSystem p = GameObject.Find(name).GetComponent<ParticleSystem>();
        if (!p.isPlaying) {
            p.Play();
        }

        if (disableOthers) {
            foreach (string s in allNames) {
                if (s == name) {
                    continue;
                }
                ParticleSystem pp = GameObject.Find(s).GetComponent<ParticleSystem>();
                if (pp.isPlaying) {
                    pp.Stop();
                }
            }
        }
    }
}
