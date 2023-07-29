using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TraceLineManagerName;

public enum PILIHAN_HURUF {GHAIN, QAF};

public class DrawingController : MonoBehaviour {

    // [SerializeField] GameObject[] linePrefabs;
    // [SerializeField] private GameObject linePrefab;

    private Transform letterParent;

    [SerializeField] int currentHuruf = 0;
    int MAX_HURUF = 2;

    private void Awake() {
        letterParent = transform.Find("Hurufs");
        ResetHuruf();
    }

    void ResetHuruf() {
        int len = letterParent.childCount;
        for (int i = 0; i < len; i++)
        {
            letterParent.GetChild(i).gameObject.SetActive(false);
        }
        letterParent.GetChild(currentHuruf).gameObject.SetActive(true);
    }

    public void NextHuruf() {
        Debug.Log("Next huruf");
        if(currentHuruf<MAX_HURUF-1) currentHuruf++;
        else currentHuruf=0;
        ResetHuruf();
    }

    public void BackToMenu() {
        SceneManager.LoadScene("Menu");
    }
    
    public void ResetDrawing() {
        letterParent.GetChild(currentHuruf).GetComponent<TraceLineManager>().ResetActivity();
    }
}