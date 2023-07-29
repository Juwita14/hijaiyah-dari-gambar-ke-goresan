using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.Collections;
using Unity.Barracuda;
using System.Linq;
using System.Text;
using System;
using UnityEngine.Monetization;

// TAMBAHAN

using System.Drawing;
using Color = UnityEngine.Color;
using UnityEditor.Compilation;
using System.Reflection;
using UnityEngine.SocialPlatforms;

public class EvaluationMain : MonoBehaviour {

    [SerializeField] TMP_Text predictionText;
    [SerializeField] TMP_Text soalNum;
    [SerializeField] TMP_Text soalText;
    SoalEvaluasiManager soalManager;
    [SerializeField] IrmanDraw drawer;
    public bool IsPlaying = false;
    [SerializeField] UIEvaluasi uIEvaluasi;

    void Awake() {
        uIEvaluasi = transform.Find("UIEvaluasi").GetComponent<UIEvaluasi>();
        uIEvaluasi.Activate(this);
        drawer = transform.Find("Draw").GetComponent<IrmanDraw>();
        drawer.Activate(this);
    }

    private void Start() {
        
        UpdateRectDimension();
    }

    void GameStart() {
        IsPlaying = true;
        StartTimer();
        OnNotifyResume();
    }

    void GenerateSoal() {
        soalManager = GetComponent<SoalEvaluasiManager>();
        soalManager.GenerateSoal();
        
        
        UpdateUISoal();
    }

    public void OnNotifyPause() {
        drawer.HideDrawing();
    }
    public void OnNotifyResume() {
        drawer.ShowDrawing();
    }

    #region timer

    [SerializeField] TMP_Text timer_txt;

    [SerializeField] float timerTime = 60f;
    int timerTweenId = 0;

    bool isTimerRunning = false;

    void StartTimer() {
        if(isTimerRunning) {
            return;
        }
        isTimerRunning = true;
        timerTweenId = LeanTween.value(gameObject, timerTime, 0f, timerTime).setIgnoreTimeScale(true)
        .setOnStart(()=>{OnTimerStart();})
        .setOnUpdate((x)=>{OnTimerUpdate(x);})
        .setOnComplete(()=>{OnTimerComplete();}).id;
    }

    void StopTimer() {
        LeanTween.pause(timerTweenId);
        isTimerRunning = false;
    }

    void ResumeTimer() {
        LeanTween.resume(timerTweenId);
        isTimerRunning = true;
    }

    void OnTimerUpdate(float time) {
        UpdateTimerUI(time);
    }

    void OnTimerComplete() {
        Debug.Log($"Timer Complete!");
        isTimerRunning = false;
        IsPlaying = false;
        EndSoal();
    }

    void OnTimerStart() {
        Debug.Log($"Timer Start!");
    }

    public void OnGamePause() {
        StopTimer();
        uIEvaluasi.ShowPause();
    }
    public void OnGameResume() {
        ResumeTimer();
        uIEvaluasi.HidePause();
    }

    void UpdateTimerUI(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer_txt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion timer

    #region soal

    int currentSoal = 1;

    void NextSoal() {
        if(soalManager.GetSoalCount() > 1) soalManager.NextSoal();
        else {
            EndSoal();
            StopTimer();
        }

        UpdateUISoal();
    }

    public void SkipSoal() {
        soalManager.SkipSoal();
        UpdateUISoal();
    }

    void UpdateUISoal() {
        string text = Konstanta.GetRealHuruf(soalManager.GetSoal());
        text = text[0].ToString().ToUpper() + text.Substring(1);
        soalText.text = text;
        Debug.Log($"update soal menjadi:{text}");
    }

    void CheckingJawaban(int prediction) {
        Debug.Log($"prediksi:{prediction} -> soal:{soalManager.GetSoal()}");
        if(prediction==soalManager.GetSoal()) {
            OnCorrectAnswer();
        }
        else {
            OnWrongAnswer();
        }
    }

    void OnCorrectAnswer() {
        AddBenar();
        NextSoal();
        SoundController.GetInstance().PlayCorrect();
        uIEvaluasi.ShowBenarSalah(true);
        sliderNilai.AddNilai();
        GetComponent<IrmanDraw>().DeleteDrawing();
    }

    void OnWrongAnswer() {
        // OnNotifyPause();
        uIEvaluasi.ShowBenarSalah(false);
        SoundController.GetInstance().PlayWrong();
        KurangiBenar();
        sliderNilai.KurangiNilai();

    }

    void ShowDrawing() {
        drawer.ShowDrawing();
    }

    void EndSoal() {
        uIEvaluasi.ShowScore(/*Mathf.CeilToInt(sliderNilai.GetNormalValue() * 100f)*/GetBenar()*10, sliderNilai.GetFinalStar());
    }

    #endregion soal

    #region Nilai

    [SerializeField] IrmanSlider sliderNilai;

    [SerializeField] private int currentBenar = 0;

    void AddBenar() {
        currentBenar += 1;
    }

    void KurangiBenar() {
        currentBenar -= 1;
    }

    int GetBenar() {
        return currentBenar;
    }

    void ResetBenar() {
        currentBenar = 0;
    }

    #endregion Nilai

    #region sampling-gambar

    [SerializeField] Camera mainCam;
    [SerializeField] Camera renderCam;

    public Vector2 rectdim = new Vector2(0,0);
    public Vector2 readpixeldest = new Vector2(0,0);

    public Vector2 texDim = new Vector2(128,128);
    public bool enableScaling = true;
    public Vector2 texFinalDim = new Vector2(64,64);
    public bool useBilinear = true;
    void UpdateRectDimension() {
        if(Screen.width>Screen.height) {
            rectdim.x = ((texDim[0]*(float)Screen.width/Screen.height)-texDim[0])/2;
            rectdim.y = 0;
        }
        else {
            rectdim.x = 0;
            rectdim.y = ((texDim[0]*(float)Screen.height/Screen.width)-texDim[0])/2;
        }
    }

    Texture2D GetSampleDrawing2(int[] dim=null) {
        UpdateRectDimension();
        bool isLandscape = Screen.width > Screen.height? true: false;

        if(dim==null) {
            if(isLandscape)
                dim = new int[2] {(int)texDim.y * Screen.width/Screen.height,(int)texDim.y};
            else
                dim = new int[2] {(int)texDim.x,(int)texDim.x * Screen.height/Screen.width};
        }

        Debug.Log($"Get sample drawing dg ukuran {dim[0]}x{dim[1]}");
        
        Texture2D tex = new Texture2D((int)texDim.x, (int)texDim.x, TextureFormat.RGB24, false);
        RenderTexture rt = new RenderTexture(dim[0], dim[1], 24);
        renderCam.targetTexture = rt;
        renderCam.Render();
        RenderTexture.active = rt;
        
        Rect rectReadPixels = new Rect(rectdim.x, rectdim.y, (int)texDim.x, (int)texDim.y);
        tex.ReadPixels(rectReadPixels, (int)readpixeldest.x, (int)readpixeldest.y);
        tex.Apply();

        renderCam.targetTexture = null;

        return tex;
    }
    public void ExportCsv(List<DataPointTime> genericList, string fileName)
    {
        var sb = new StringBuilder();
        //var basePath = AppDomain.CurrentDomain.BaseDirectory;
        //var finalPath = Path.Combine(basePath, fileName + ".csv");
        var finalPath = Path.Combine(fileName + ".csv");
        var header = "";
        if (!File.Exists(finalPath))
        {
            var file = File.Create(finalPath);
            file.Close();
            header = "x,y,time";
            sb.AppendLine(header);
            TextWriter sw = new StreamWriter(finalPath, true);
            sw.Write(sb.ToString());
            sw.Close();
        }
        foreach (DataPointTime obj in genericList)
        {
            sb = new StringBuilder();
            var line = "";
            line += obj.x + ","+ obj.y + ","+ obj.time;
            
            sb.AppendLine(line);
            TextWriter sw = new StreamWriter(finalPath, true);
            sw.Write(sb.ToString());
            sw.Close();
        }
    }

    void SaveTextureToFile(Texture2D _tex, int count, string _path=null) {
        if(_path==null) _path = Application.persistentDataPath;
        string time =  System.DateTime.Now.ToString("ddMMyyHHmmss");
        string fileName = Konstanta.GetRealHuruf(soalManager.GetSoal(currentSoal-1))+ time.ToString() + count.ToString();

        if(!Directory.Exists(_path + "/FOLDER")) Directory.CreateDirectory(_path + "/FOLDER");

        var pngData = _tex.EncodeToPNG();
        
        if (pngData != null)
        {
            File.WriteAllBytes(_path + "/FOLDER/" + fileName + ".png", pngData);
            Debug.Log(_path + "/FOLDER/" + fileName + ".png");
            Debug.Log(drawer.datas.Count);
            ExportCsv(drawer.datas, _path + "/FOLDER/" + fileName);
        }
        else
            Debug.Log("Could not convert " + fileName + " to png. Skipping saving texture");

        string path = "C:\\Users\\Peni Nata\\AppData\\LocalLow\\Irman\\Hijaiyah\\FOLDER\\ain04062309003412313123.png";
        Chain(_tex, path);
    }

    void Chain(Texture2D _tex, string path){
        Texture2D tex = null;
        byte[] fileData;

        fileData = File.ReadAllBytes(path);
        tex = new Texture2D(64, 64);
        tex.LoadImage(fileData);

        int start_x = 0;
        int start_y = 0;

        bool exit_loop = false;

        int w = tex.width;
        int h = tex.height;
        UnityEngine.Color32[] cols = tex.GetPixels32();
        UnityEngine.Color32 white = new UnityEngine.Color32(255, 255, 255, 255);
        UnityEngine.Color32 black = new UnityEngine.Color32(0, 0, 0, 255);

        // cari start point
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int index = y * w + x;
                
                if (cols[index].Equals(white))
                {
                    Debug.Log(x + " , " + y + " = " + index);
                    Debug.Log("white");
                    start_x = x;
                    start_y = y;
                    exit_loop = true;
                    break;
                } 
            }
            if(exit_loop == true) 
            {
                break;
            }
        }

        // bobot directions
        int[] directions = { 0, 1, 2,
                                 7,    3,
                                 6, 5, 4 };

        int[] change_j = {-1, 0, 1,
                              -1,    1,
                              -1, 0, 1};

        int[] change_i = {-1, -1, -1,
                               0,      0,
                               1, 1, 1 };

        
        List<int> chain = new List<int>();
        List<DataPointTime> border = new List<DataPointTime>();

        int curr_point_x = start_x;
        int curr_point_y = start_y;

        int direction = 0;

        for (direction = 0; direction < directions.Length; direction++)
        {
            int idx = direction;
            int new_point_x = start_x + change_i[idx];
            int new_point_y = start_y + change_j[idx];
            int index = new_point_y * w + new_point_x;
            //Debug.Log(cols[index]);
            if (cols[index].Equals(black)) 
            {
                border.Add(new DataPointTime(new_point_x, new_point_y, 0));
                chain.Add(direction);
                curr_point_x = new_point_x;
                curr_point_y = new_point_y;
                Debug.Log(border);
                Debug.Log(direction);
                Debug.Log(curr_point_x + ", " + curr_point_y);
                break;
            }
        }

        int count = 0;

        while ((curr_point_x != start_x) && (curr_point_y != start_y))
        {
            int b_direction = (direction + 5) % 8;
            Debug.Log("b_direction " + b_direction);
            
            var dirs_1 = Enumerable.Range(b_direction, 8 - b_direction).ToList();
            var dirs_2 = Enumerable.Range(0, b_direction).ToList();
            
            dirs_1.AddRange(dirs_2);
            for (int i = 0; i < dirs_1.Count; i++)
            {
                Debug.Log(dirs_1[i]);
            }
            break;
            /*
            for(direction = 0; direction < dirs_1.lenght; direction++){
                int idx = direction;
                int new_point_x = start_x + change_i[idx];
                int new_point_y = start_y + change_j[idx];
                int index = new_point_y * w + new_point_x;
                //Debug.Log(cols[index]);
                if (cols[index].Equals(black))
                {
                    border.Add(new DataPointTime(new_point_x, new_point_y, 0));
                    chain.Add(direction);
                    curr_point_x = new_point_x;
                    curr_point_y = new_point_y;
                    Debug.Log(border);
                    Debug.Log(direction);
                    Debug.Log(curr_point_x + ", " + curr_point_y);
                    break;
                }
            }

            if(count.Equals(1000))
                break;
            count += 1;*/
        }
    }

    #endregion sampling-gambar

    bool usingNoedify=false;
    bool usingTF=false;

    public void TestJawaban()
    {
        Texture2D tempTex = GetSampleDrawing2();
        TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
        SaveTextureToFile(tempTex, 12313123);
        int prediction=0;
        float confidence=0;
        /*if(usingTF){
            ArrayList result = Evaluate(tempTex);
            prediction = (int)result[0];
            confidence = (float)result[1];
        }*/

        prediction = Konstanta.ConvertPredictionToRealIndex(prediction);
        predictionText.text = $"{Konstanta.GetRealHuruf(prediction)} {confidence}";
        CheckingJawaban(prediction);
    }

    public void TestJawaban99() {
        Texture2D tempTex = GetSampleDrawing2();
        //TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
        
        SaveTextureToFile(tempTex, 12313123);

        


        StartCoroutine(RunModelRoutine(tempTex));
    }    

    const int IMAGE_SIZE = 32;
	const string INPUT_NAME = "conv2d_1_input";
	const string OUTPUT_NAME = "dense_2/Softmax";

    IEnumerator RunModelRoutine(Texture2D tex) {
        Tensor tensor = XUtils.TransformInput3(tex);

		var inputs = new Dictionary<string, Tensor> {
			{ INPUT_NAME, tensor }
		};

		DataLoader.GetInstance().worker.Execute(inputs);
		Tensor outputTensor = DataLoader.GetInstance().worker.PeekOutput(OUTPUT_NAME);

		//get largest output
		List<float> temp = outputTensor.ToReadOnlyArray().ToList();
		float max = temp.Max();
		int index = temp.IndexOf(max);

        //set UI text
        int index2 = Konstanta.ConvertPredictionToRealIndex(index);
        predictionText.text = $"{Konstanta.GetRealHuruf(index2)} {max}";

        CheckingJawaban(index2);

        //dispose tensors
        tensor.Dispose();
		outputTensor.Dispose();
		yield return null;
	}

    public void RestartEvaluasi() {
        Reset();
        uIEvaluasi.ShowTutorial();
        //Invoke("GameStart", 2.0f);
        soalManager.GenerateSoal();
        UpdateUISoal();
    }

    public void Reset() {
        uIEvaluasi.HideScore();
        ResetBenar();
        StopTimer();
        sliderNilai.Reset();
        UpdateTimerUI(0);
        GetComponent<IrmanDraw>().DeleteDrawing();
    }

    #region TF

    /*

    public TextAsset graphModel;

    private static int img_width = 32;
	private static int img_height = 32;
	private float[,,,] inputImg = new float[1,img_width,img_height,1];

    TFGraph graph;
    void InitTF() {
        graph = new TFGraph();
		graph.Import (graphModel.bytes);
    }
    ArrayList Evaluate (Texture2D input) {
		
		// Get raw pixel values from texture, format for inputImg array
		for (int i = 0; i < img_width; i++) {
			for (int j = 0; j < img_height; j++) {
				inputImg [0, img_width - i - 1, j, 0] = input.GetPixel(j, i).r;
			}
		}

		// Create the TensorFlow model
		
		var session = new TFSession (graph);
		var runner = session.GetRunner ();

		// Set up the input tensor and input
		runner.AddInput (graph ["conv2d_1_input"] [0], inputImg);
		// Set up the output tensor
		runner.Fetch (graph ["dense_2/Softmax"] [0]);

		// Run the model
		float[,] recurrent_tensor = runner.Run () [0].GetValue () as float[,];

		// Find the answer the model is most confident in
		float highest_val = 0;
		int highest_ind = -1;
		float sum = 0;
		float currTime = Time.time;

		for (int j = 0; j < Konstanta.REAL_OUTPUT_SIZE; j++) {
			float confidence = recurrent_tensor [0, j];
			if (highest_ind > -1) {
				if (recurrent_tensor [0, j] > highest_val) {
					highest_val = confidence;
					highest_ind = j;
				}
			} else {
				highest_val = confidence;
				highest_ind = j;
			}

			// sum should total 1 in the end
			sum += confidence;
		}

        return new ArrayList(){highest_ind, highest_val};
		// label.text = $"Answer: {arabic_characters[highest_ind]} ({highest_ind}) \n Confidence: {highest_val} \n Latency: {(Time.time - currTime) * 1000000}us";
	}

    */

    #endregion TF

    void OnDisable() {
        Reset();
    }

    void OnEnable() {
        Debug.Log("On enable evaluasi");
        // LeanTween.cancelAll();
        GenerateSoal();
        
    }

    public void Play() {
        //Invoke("GameStart", 2.0f);
        GameStart();
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.RightControl)) {
            OnCorrectAnswer();
        }
        else if(Input.GetKeyUp(KeyCode.S)) {
            StartTimer();
        }
    }
}