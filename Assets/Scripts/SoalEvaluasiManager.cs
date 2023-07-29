using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class SoalEvaluasiManager : MonoBehaviour {

    private void Start() {
        // GenerateSoal();
    }

    public int JUMLAH_SOAL = 30;
    private List<int> soals = new List<int>();

    public int GetSoal(int index) {
        return soals[index];
    }

    public int GetSoalCount() {
        return soals.Count;
    }

    public int GetSoal() {
        return soals[0];
    }

    public void GenerateSoal() {
        soals.Clear();
        for (int i = 0; i < Konstanta.REAL_OUTPUT_SIZE; i++) {
            soals.Add(i);
        }

        soals.Shuffle();
        soals.Shuffle();
        soals.Shuffle();

        PrintSoal();
    }

    public void SkipSoal() {
		soals.Add(soals[0]);
		soals.RemoveAt(0);
		PrintSoal();
	}

    public void NextSoal() {
        if(soals.Count>0) soals.RemoveAt(0);
        else Debug.LogError("Soal habis");
		PrintSoal();
	}

    void PrintSoal() {
        string toPrint = "";
        for (int i = 0; i < soals.Count; i++) {
            toPrint += soals[i] + ",";
        }
        Debug.Log(toPrint);
    }



}