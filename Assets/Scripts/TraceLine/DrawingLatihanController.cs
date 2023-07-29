using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IrmanTraceLine
{
public class DrawingLatihanController : MonoBehaviour {

    MainLatihan main;
    public void Activate(MainLatihan _main) {
        main = _main;
    }

    [SerializeField] private Transform letterParent;

    [SerializeField] int currentHuruf = 0;
    int MAX_HURUF = 2;

    [SerializeField] private TMPro.TMP_Text soal_txt;

    [SerializeField] int currentError = 0;

    private void Awake() {
        MAX_HURUF = letterParent.childCount;
        Debug.Log("Awake sblm ganti huruf");
        ResetHuruf();
        
    }

    public void ResetHuruf() {
        int len = letterParent.childCount;
        for (int i = 0; i < len; i++)
        {
            letterParent.GetChild(i).GetComponent<IrmanTraceLineManager>().onCompleteLetter = null;
            letterParent.GetChild(i).GetComponent<IrmanTraceLineManager>().onCompleteLetter += OnCompleteLetter;
            letterParent.GetChild(i).gameObject.SetActive(false);
        }
        currentHuruf = DataLoader.GetInstance().selectedHuruf;
        Debug.Log($"ganti huruf ke index: {currentHuruf}");
        letterParent.GetChild(currentHuruf).gameObject.SetActive(true);
        // Debug.Log($"{DataLoader.GetInstance().selectedHuruf}");
        UpdateUISoal();
    }

    public void OnCompleteLetter() {
        int totalStar = HitungSkor();//
        main.ShowScore(totalStar);
        int curlvl = DataLoader.GetInstance().selectedHuruf;
        if(curlvl<Konstanta.REAL_OUTPUT_SIZE-1) DataLoader.GetInstance().GetLevel(curlvl+1).isPlayed = true;
        if(totalStar>DataLoader.GetInstance().GetLevel(curlvl).skor)
            DataLoader.GetInstance().GetLevel(curlvl).skor = totalStar;
    }

    int HitungSkor() {
        int err = ScoreLatihanManager.GetInstance().GetError();
        if(err<=3) return 3;
        else if(err<=5) return 2;
        else return 1;
    }

    public void NextHuruf() {
        Debug.Log("Next huruf");
        if(currentHuruf<MAX_HURUF-1) currentHuruf++;
        else currentHuruf=0;
        ResetHuruf();
    }

    public void UpdateUISoal() {
        string text = Konstanta.GetRealHuruf(currentHuruf);
        text = text[0].ToString().ToUpper() + text.Substring(1);
        soal_txt.text = text;
    }
    
    public void ResetDrawing() {
        letterParent.GetChild(currentHuruf).GetComponent<IrmanTraceLineManager>().ResetActivity();
    }
}

}