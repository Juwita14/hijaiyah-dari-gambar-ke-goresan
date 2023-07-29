using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILatihan : MonoBehaviour
{
    private static UILatihan instance;

    public static UILatihan GetInstance() {
        return instance;
    }

    IrmanCanvas canvasComplete;
    ThreeStar starComplete;

    IrmanCanvas canvasPause;
    IrmanCanvas canvasTutorial;

    void Awake() {
        instance = this;

        canvasComplete = transform.Find("CanvasComplete").GetComponent<IrmanCanvas>();
        starComplete = canvasComplete.transform.Find("panel").Find("star_star").GetComponent<ThreeStar>();

        canvasPause = transform.Find("CanvasPause").GetComponent<IrmanCanvas>();
        canvasTutorial = transform.Find("CanvasTutorial").GetComponent<IrmanCanvas>();
    }

    void Start()
    {
        ShowTutorial();
    }

    void OnEnable() {
        ShowTutorial();
    }

    public void ResetUI() {
        HideComplete();
        HidePause();
    }

    public void ShowComplete(int _star) {
        starComplete.SetValue(_star);
        // canvasComplete.enabled = true;
        canvasComplete.ShowCanvas();
    }

    public void HideComplete() {
        // canvasComplete.enabled = false;
        canvasComplete.HideCanvas();
    }

    public void ShowPause() {
        canvasPause.ShowCanvas();
    }

    public void HidePause() {
        canvasPause.HideCanvas();
    }

    public void ShowTutorial() {
        // if(PlayerPrefs.GetInt("DONTDISPLAYTUT1",0)==0) {
        if(canvasTutorial.GetComponent<TutorialCanvas>().Show) {
            canvasTutorial.ShowCanvas();
        }
        else {
            
        }
    }

    public void HideTutorial() {
        canvasTutorial.HideCanvas();
    }
}
