using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : BaseSystem {
	bool scoreDirty = true;
    bool livesDirty;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        Pool.Instance.AddSystemListener(typeof(LossComponent), this);

        GameController gc = GameController.Instance;
        this.livesDirty = gc.livesMode;
        gc.livesText.gameObject.SetActive(gc.livesMode);
        gc.timerText.gameObject.SetActive(gc.timeMode);
    }

    public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(LossComponent), this);
    }

    public override void Update() {
		GameController gc = GameController.Instance;
        if (scoreDirty || livesDirty) {
            gc.gameOverStreakText.text = string.Format("{0} Streak", gc.maxMatchStreak);

            if (scoreDirty) {
                int score = gc.score;
                int highScore = PlayerPrefs.GetInt("highScore");
                if (score > highScore) {
                    PlayerPrefs.SetInt("highScore", score);
                    highScore = score;
                }

                gc.gameplayScoreText.text = string.Format("Score: {0}", gc.score);
                gc.gameOverScoreText.text = string.Format("{0}", gc.score);
                gc.pauseScoreText.text = string.Format("{0}", gc.score);
                gc.gameOverHighScoreText.text = string.Format("{0}", highScore);
                gc.pauseHighScoreText.text = string.Format("{0}", highScore);
            }

            if (gc.livesMode && livesDirty) {
                gc.livesText.text = string.Format("Lives: {0}", Mathf.Max(gc.numberOfLives, 0));
            }
            scoreDirty = false;
            livesDirty = false;
        }

        if (gc.timeMode) {
            int min = ((int)gc.totalTime) / 60;
            int sec = ((int)gc.totalTime) % 60;
            gc.timerText.text = string.Format("{0:D2}:{1:D2}", Mathf.Max(min, 0), Mathf.Max(sec, 0));
            gc.totalTime -= Time.deltaTime;
        }
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {

		} else if (c is LossComponent) {

        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			GameController gc = GameController.Instance;
            gc.totalTime += gc.bonusTime;
            this.scoreDirty = true;
		} else if (c is LossComponent) {
            this.livesDirty = true;
        }
    }
}
