using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPredictions_BackgroundTraining : MonoBehaviour
{

    public Texture2D[] sampleImageSet;

    public List<Texture2D> newSampleImageSet;

    public GameObject[] sampleImagePlanes;
    public Text[] FC_predictionText;
    public Text[] CNN_predictionText;

    public List<Texture2D> sampleImageRandomSet;

    int[] sampleRange;

    // Start is called before the first frame update
    void Start()
    {
        JUMLAH_HURUF = Konstanta.REAL_OUTPUT_SIZE;
        LoadSampleImageSet();
        sampleRange = new int[newSampleImageSet.Count];
        sampleImageRandomSet = new List<Texture2D>();
        
        for (int i = 0; i < sampleRange.Length; i++)
            sampleRange[i] = i;

        RefreshSample();

    }

    int JUMLAH_HURUF = 11;

    private void LoadSampleImageSet()
    {
        newSampleImageSet = new List<Texture2D>();
        
        for (int i = 0; i < JUMLAH_HURUF; i++) {
            for (int j=13; j<14; j++) {
                newSampleImageSet.Add(DatasetRegister.GetInstance().GetTexture(i,j+1));
            }
        }
    }

    void RefreshSample() {
        Noedify_Utils.Shuffle(sampleRange);
        sampleImageRandomSet.Clear();

        for (int i = 0; i < sampleImagePlanes.Length; i++)
            sampleImageRandomSet.Add(newSampleImageSet[sampleRange[i]]);

        for (int i=0; i < sampleImagePlanes.Length; i++)
        {
            sampleImagePlanes[i].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", sampleImageRandomSet[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.R)) {
            RefreshSample();
        }
    }
}
