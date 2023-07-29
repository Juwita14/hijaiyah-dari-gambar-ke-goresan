using UnityEngine;

public class ScoreLatihanManager : MonoBehaviour {
    static ScoreLatihanManager instance;
    public static ScoreLatihanManager GetInstance() {
        return instance;
    }

    [SerializeField] IrmanSlider sliderScore;

    void Awake() {
        instance = this;
    }

    [SerializeField] int currentError = 0;

    public void AddError() {
        currentError++;
        sliderScore.KurangiNilai();
    }

    public void ResetError() {
        currentError = 0;
        sliderScore.Reset();
    }

    public int GetError() {
        return currentError;
    }
}