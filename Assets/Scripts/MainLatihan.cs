using IrmanTraceLine;
using UnityEngine;

public class MainLatihan : MonoBehaviour
{
    [SerializeField] Transform hurufTransform;
    [SerializeField] DrawingLatihanController controller;

    [SerializeField] Camera latihanCamera;

    [SerializeField] UILatihan uILatihan;

    private void Awake() {
        Debug.Log($"{Screen.width}x{Screen.height}");
        float defaultResolution = (float)9/16;
        float curResolution = (float)Screen.width/Screen.height;
        float curScale = hurufTransform.localScale.x;
        
        float mult = defaultResolution/curResolution;
        float curCamSize = latihanCamera.orthographicSize;
        // latihanCamera.orthographicSize = 5.65f * ((float)Screen.height/Screen.width)/((float)16/9);
        // Debug.Log($"curscale={curScale} curres={curResolution} mult={mult}");
        // Debug.Log($"{curScale*mult} {(float)curScale/mult}");
        hurufTransform.localScale = new Vector3(curScale/mult, curScale/mult, curScale/mult);

        controller.Activate(this);
    }

    public void Mulai() {
        controller.ResetHuruf();
    }

    public void RestartLevel() {
        uILatihan.ResetUI();
        controller.ResetDrawing();
        ScoreLatihanManager.GetInstance().ResetError();
        uILatihan.ShowTutorial();
        Mulai();
    }

    public void NextLevel() {
        if(DataLoader.GetInstance().selectedHuruf<Konstanta.REAL_OUTPUT_SIZE-1)
            DataLoader.GetInstance().selectedHuruf++;
        else
            DataLoader.GetInstance().selectedHuruf = 0;
        RestartLevel();
    }

    public void ShowScore(int _score) {
        uILatihan.ShowComplete(_score);
    }

    public void Exit() {
        uILatihan.ResetUI();
        controller.ResetDrawing();
    }
}