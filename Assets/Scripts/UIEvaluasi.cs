using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEvaluasi : MonoBehaviour
{
    private static UIEvaluasi instance;

    public static UIEvaluasi GetInstance() {
        return instance;
    }

    EvaluationMain main;

    public void Activate(EvaluationMain _main) {
        main = _main;
    }

    IrmanCanvas scoreCanvas;
    TMP_Text score_txt;
    ThreeStar score_star;
    Canvas resultPopupCanvas;
    GameObject benarPanel, salahPanel;
    Canvas instructionCanvas;
    IrmanCanvas pauseCanvas;
    IrmanCanvas tutorialCanvas;

    void Awake() {
        instance = this;

        scoreCanvas = transform.Find("CanvasScore").GetComponent<IrmanCanvas>();
        score_txt = scoreCanvas.transform.Find("panel").Find("score_txt").GetComponent<TMP_Text>();
        score_star = scoreCanvas.transform.Find("panel").Find("score_star").GetComponent<ThreeStar>();

        resultPopupCanvas = transform.Find("CanvasBenarSalah").GetComponent<Canvas>();
        benarPanel = resultPopupCanvas.transform.GetChild(1).gameObject;
        salahPanel = resultPopupCanvas.transform.GetChild(2).gameObject;

        instructionCanvas = transform.Find("CanvasInstruction").GetComponent<Canvas>();

        pauseCanvas = transform.Find("CanvasPause").GetComponent<IrmanCanvas>();
        tutorialCanvas = transform.Find("TutorialCanvas").GetComponent<IrmanCanvas>();
    }

    private void InitBenarSalahPanel()
    {
        resultPopupCanvas.enabled = false;
        benarPanel.SetActive(false);
        salahPanel.SetActive(false);
    }

    void Start()
    {
        InitBenarSalahPanel();
        ShowTutorial();
    }

    void OnEnable() {
        ShowTutorial();
        HidePause();
    }

    void Update()
    {
        
    }

    public void ShowScore(int _score, int _star) {
        main.OnNotifyPause();
        score_txt.text = _score.ToString();
        score_star.SetValue(_star);
        scoreCanvas.ShowCanvas();
    }

    public void HideScore() {
        scoreCanvas.HideCanvas();
    }

    public void ShowBenarSalah(bool _benar) {
        main.OnNotifyPause();
        if(_benar) benarPanel.SetActive(true);
        else salahPanel.SetActive(true);
        resultPopupCanvas.enabled = true;
        Invoke("HideBenarSalah", 1f);
    }

    void HideBenarSalah() {
        main.OnNotifyResume();
        InitBenarSalahPanel();
    }

    public void ShowInstruction() {
        instructionCanvas.enabled = true;
    }

    public void HideInstruction() {
        instructionCanvas.enabled = false;
    }

    public void ShowPause() {
        pauseCanvas.ShowCanvas();
        main.OnNotifyPause();
    }

    public void HidePause() {
        pauseCanvas.HideCanvas();
        main.OnNotifyResume();
    }

    public void ShowTutorial() {
        // if(PlayerPrefs.GetInt("DONTDISPLAYTUT2",0)==0) {
        if(tutorialCanvas.GetComponent<TutorialCanvas>().Show) {
            tutorialCanvas.ShowCanvas();
        }
        else {
            ShowInstruction();
        }
    }

    public void HideTutorial() {
        tutorialCanvas.HideCanvas();
    }
}
