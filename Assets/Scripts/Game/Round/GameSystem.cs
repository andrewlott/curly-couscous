using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : BaseSystem {
    public static bool isPlaying = false;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(StartGameComponent), this);
        Pool.Instance.AddSystemListener(typeof(EndGameComponent), this);
        Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(StartGameComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(EndGameComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
    }

    protected virtual bool IsPlaying() {
        return isPlaying;
    }

    public override void Update() {
        GameController gc = (Controller() as GameController);
        if (!IsPlaying()) {
            return;
        }

        if (GetExistingMatch() != null) {
            return;
        }

        List<BaseComponent> matchableComponents = Pool.Instance.ComponentsForType(typeof(MatchableComponent));
        MatchableComponent target = null;
        bool hasMatch = true;
        foreach (MatchableComponent mac in matchableComponents) {
            ColorableComponent cac = mac.GetComponent<ColorableComponent>();

            if (target == null) {
                target = mac;
                continue;
            }

            Color targetColor = target.GetComponent<ColorableComponent>().color;
            if (targetColor != cac.color) {
                hasMatch = false;
            }
        }

        if (target) {
            if (hasMatch) {
                BaseObject.AddComponent<MatchComponent>();
            }
        }
    }

	public override void OnComponentAdded(BaseComponent c) {
		if (c is StartGameComponent) {
            StartGameComponent sgc = c as StartGameComponent;
            if (sgc.go != null) {
                ShowIfInactive(sgc.go);
            } else {
                ShowGame();
            }
            SetupNextRound();
            isPlaying = true;

            GameObject.Destroy(c);
        } else if (c is EndGameComponent) {
            EndGameComponent egc = c as EndGameComponent;
            if (egc.go != null) {
                HideIfActive(egc.go);
            } else {
                HideGame();
            }
            GameObject.Destroy(RoundSystem.GetExistingMatch());
            isPlaying = false;

            GameObject.Destroy(c);
        } else if (c is MatchComponent) {
            this.OnMatch(c.gameObject);
        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
	}

    protected void ShowGame() {
        GameController gc = GameController.Instance;
        ShowIfInactive(gc.Target);
        ShowIfInactive(gc.Player);
        foreach (GameObject go in gc.ColorButtons) {
            // TODO: Only set active the relevant color buttons
            //ShowIfInactive(go);
        }
    }

    protected void ShowIfInactive(GameObject g) {
        if (!g.activeInHierarchy) {
            g.SetActive(true);
        }
        Controller().HandleWaitAndDo(Utils.RandomFloat(0.0f), () => {
            g.GetComponent<Animator>().SetBool("isOn", true);
        });
    }



    protected void HideGame() {
        GameController gc = GameController.Instance;
        Color currentColor = gc.Target.GetComponent<ColorableComponent>().color;
        HideIfActive(gc.Target);
        HideIfActive(gc.Player);
        foreach (GameObject go in gc.ColorButtons) {
            HideIfActive(go);
        }
    }

    protected void HideIfActive(GameObject g) {
        if (g.activeInHierarchy) {
            Controller().HandleWaitAndDo(Utils.RandomFloat(0.0f), () => {
                g.GetComponent<Animator>().SetBool("isOn", false);
            });
        }
    }

    public static bool HasMatch() {
        List<BaseComponent> matchableComponents = Pool.Instance.ComponentsForType(typeof(MatchableComponent));
        MatchableComponent target = null;
        bool hasMatch = true;
        foreach (MatchableComponent mac in matchableComponents) {
            ColorableComponent cac = mac.GetComponent<ColorableComponent>();

            if (target == null) {
                target = mac;
                continue;
            }

            Color targetColor = target.GetComponent<ColorableComponent>().color;
            if (targetColor != cac.color) {
                hasMatch = false;
            }
        }

        return hasMatch;
    }

    public static MatchComponent GetExistingMatch() {
        return Pool.Instance.ComponentForType(typeof(MatchComponent)) as MatchComponent;
    }

    protected void OnMatch(GameObject g) {
        if (IsPlaying()) {
			SetupNextRound();
		}
	}

    protected void SetupNextRound() {
		GameController gc = (Controller() as GameController);
		SetupRound(gc.round);
		gc.round++;

	}

    protected void SetupRound(int round) {
        GameController gc = GameController.Instance;
        if (round > 1) {
            gc.previousColor = gc.currentColor;
        }
        gc.currentColor = NextColor();
        gc.Target.GetComponent<ColorableComponent>().color = gc.currentColor;
        gc.Player.GetComponent<ColorableComponent>().color = gc.previousColor;

        int index = 0;
        int lowerBound = 0;
        int upperBound = Mathf.Min(gc.numberOfButtons, gc.ColorButtons.Count);
        int targetIndex = lowerBound + Utils.RandomInt(upperBound - lowerBound);
        Debug.Log(string.Format("Color is at location {0}", targetIndex));
        foreach (GameObject go in gc.ColorButtons) {
            Color buttonColor = Color.clear;
            if (index == targetIndex) {
                buttonColor = gc.currentColor;
            } else if (index >= lowerBound && index < upperBound) {
                buttonColor = SimilarColor(gc.currentColor);
            }

            if (buttonColor != Color.clear) {
                ColorableComponent cac = go.GetComponent<ColorableComponent>();
                cac.color = buttonColor;
                ShowIfInactive(go);
            } else {
                HideIfActive(go);
            }
            index++;
        }

        if (gc.firstAdLevel == round) {
            gc.gameObject.AddComponent<AdComponent>();
        }
        gc.lastMatchTime = Time.time;
        Debug.Log(string.Format("Difficulty {0}", gc.difficulty));
        gc.PlayParticleForDifficulty();
    }

    protected virtual Color NextColor() {
        return Utils.RandomColor();
    }

    protected virtual Color SimilarColor(Color c) {
        GameController gc = (Controller() as GameController);
        float offset = 1.0f / gc.difficulty; // Magic equation that feels nice

        float r = Mathf.Max(Mathf.Min(c.r - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        float g = Mathf.Max(Mathf.Min(c.g - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        float b = Mathf.Max(Mathf.Min(c.b - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        return new Color(r, g, b);
    }
}
