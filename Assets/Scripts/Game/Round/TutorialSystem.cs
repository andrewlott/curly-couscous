using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : GameSystem {
    public static bool isTutorialPlaying = false;
    private static int _maxNumberButtons = 5;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(StartTutorialComponent), this);
        Pool.Instance.AddSystemListener(typeof(EndTutorialComponent), this);
        Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(StartTutorialComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(EndTutorialComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
    }

    protected override bool IsPlaying() {
        return isTutorialPlaying;
    }

    protected override void SetIsPlaying(bool b) {
        isTutorialPlaying = b;
    }

    protected override bool ShouldShowAd(int round) {
        return false;
    }

    public override void OnComponentAdded(BaseComponent c) {
        if (c is StartTutorialComponent) {
            ShowGame();
            SetupNextRound();
            SetIsPlaying(true);
            GameObject.Destroy(c);
        } else if (c is EndTutorialComponent) {
            HideGame();
            GameObject.Destroy(RoundSystem.GetExistingMatch());
            SetIsPlaying(false);

            GameObject.Destroy(c);
        } else if (c is MatchComponent) {
            this.OnMatch(c.gameObject);
        } else if (c is LossComponent) {
            this.OnLoss(c.gameObject);
        }
    }

    protected override Color NextColor() {
        GameController gc = (Controller() as GameController);
        List<Color> randomColors = new List<Color> {
            Color.red,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.green,
            Color.grey,
            Color.magenta,
        };
        int index = (gc.round - 1) % randomColors.Count;
        return randomColors[index];
    }

    protected override int NumberOfButtonsUpperbound() {
        return Mathf.Min(_maxNumberButtons, base.NumberOfButtonsUpperbound());
    }

    protected override Color SimilarColor(Color c, int index) {
        if (index >= _maxNumberButtons) {
            return Color.clear;
        }
        GameController gc = (Controller() as GameController);
        List<Color> randomColors = new List<Color> {
            Color.red,
            Color.blue,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.magenta,
            Color.grey,
            //Color.white,
            //Color.black,
        };
        List<Color> usedColors = new List<Color> { c, gc.previousColor };
        foreach (GameObject go in gc.ColorButtons) {
            ColorableComponent cc = go.GetComponent<ColorableComponent>();
            usedColors.Add(cc.color);
        }
        Color ret;
        do {
            int i = Utils.RandomInt(randomColors.Count);
            ret = randomColors[i];
        } while (usedColors.Contains(ret));

        return ret;
    }
}
