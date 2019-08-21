﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : BaseSystem {
    private int _internalScore;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        Pool.Instance.AddSystemListener(typeof(LossComponent), this);
        Pool.Instance.AddSystemListener(typeof(EndGameComponent), this);


        GameController gc = GameController.Instance;
        gc.livesContainer.gameObject.SetActive(gc.livesMode);
        gc.timerText.gameObject.SetActive(gc.timeMode);
    }

    public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(LossComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(EndGameComponent), this);
    }

    public override void Update() {
		GameController gc = GameController.Instance;
        if (gc.scoreDirty || gc.livesDirty) {
            if (gc.matchStreak > 1) {
                AnimationComponent.Animate(
                    gc.streakText.GetComponentInParent<Animator>().gameObject,
                    "isStreak",
                    false,
                    null,
                    "anim_streaktext"
                );

                gc.streakText.text = string.Format("{0} STREAK!", gc.matchStreak);
            }
            gc.gameOverStreakText.text = string.Format("{0} Streak", gc.maxMatchStreak);

            if (gc.scoreDirty) {
                int score = gc.Score;
                int highScore = PlayerPrefs.GetInt("highScore");
                if (score > highScore) {
                    PlayerPrefs.SetInt("highScore", score);
                    highScore = score;
                }
				if (gc.Score == 0) {
					this._internalScore = 0;
				}
			    gc.HandleCoroutine(LerpToScore());

                gc.pauseScoreText.text = string.Format("{0}", gc.Score);
                gc.pauseHighScoreText.text = string.Format("{0}", highScore);

                gc.mainMenuHighScoreText.text = string.Format("{0}", highScore);
            }

            if (gc.livesMode && gc.livesDirty) {
                for (int i = 0; i < gc.livesContainer.transform.childCount; i++) {
                    gc.livesContainer.transform.GetChild(i).gameObject.SetActive(i < gc.Lives);
                }
            }
            gc.scoreDirty = false;
            gc.livesDirty = false;
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

        } else if (c is EndGameComponent) {
            GameController gc = GameController.Instance;
            gc.gameOverScoreText.text = string.Format("{0}", gc.Score);
            gc.gameOverHighScoreText.text = string.Format("{0}", PlayerPrefs.GetInt("highScore"));
        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			GameController gc = GameController.Instance;
            gc.totalTime += gc.bonusTime;
		}
    }

    private IEnumerator LerpToScore() {
        GameController gc = (Controller() as GameController);
        while (this._internalScore != gc.Score) {
            gc.gameplayScoreText.text = string.Format("Score: {0}", this._internalScore);
            this._internalScore += 10;
            yield return new WaitForSeconds(0.0f);
        }
        gc.gameplayScoreText.text = string.Format("Score: {0}", gc.Score);

    }
}
