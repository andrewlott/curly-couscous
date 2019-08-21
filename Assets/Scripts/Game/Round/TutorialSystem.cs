using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : GameSystem {
    public static bool isTutorialPlaying = false;

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
        return randomColors[gc.round - 1];
    }

    protected override Color SimilarColor(Color c) {
        GameController gc = (Controller() as GameController);
        List<Color> randomColors = new List<Color> {
            Color.red,
            Color.blue,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.magenta,
            //Color.grey,
            //Color.white,
            //Color.black,
        };
        List<Color> usedColors = new List<Color> { c, gc.previousColor };
        //foreach (GameObject go in gc.ColorButtons) {
        //    ColorableComponent cc = go.GetComponent<ColorableComponent>();
        //    usedColors.Add(cc.color);
        //}
        Color ret;
        do {
            int index = Utils.RandomInt(randomColors.Count);
            ret = randomColors[index];
        } while (usedColors.Contains(ret));

        return ret;
    }
}
