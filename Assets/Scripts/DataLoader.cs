using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour
{
    static DataLoader instance;
    public static DataLoader GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start() {
        InitBarracuda();
        // LoadData();
    }

    public int selectedHuruf;

    public GameSaving gameSaving;

    public LevelHuruf[] gameData;

    public LevelHuruf GetLevel(int index) {
        return gameSaving.levelhuruf[index];
    }

    void CreateData() {
        gameSaving.levelhuruf = new LevelHuruf[Konstanta.REAL_OUTPUT_SIZE];
        for (int i = 0; i < Konstanta.REAL_OUTPUT_SIZE; i++)
        {
            LevelHuruf huruf = new LevelHuruf(i, Konstanta.GetRealHuruf(i), false, 0);
            gameSaving.levelhuruf[i] = huruf;
        }
        gameSaving.levelhuruf[0].isPlayed = true;
    }

    void LoadData() {
        if(IsSaveDataAvailable()) {
            string savedData = GetSavedData();
            gameSaving = JsonUtility.FromJson<GameSaving>(savedData);
        }
        else {
            CreateData();
            SaveGame();
        }
        if(gameSaving!=null) SceneManager.LoadScene(1);
    }

    void SaveGame() {
        string savedData2 = JsonUtility.ToJson(gameSaving);
        Debug.Log(savedData2);
        SaveFile(savedData2);
    }

    void SaveFile(string _data) {
        string destination = "";

        #if UNITY_ANDROID
            destination = Application.persistentDataPath + "/save.data";
        #endif
        #if UNITY_EDITOR || UNITY_STANDALONE
            destination = Application.dataPath + "/save.data"; 
        #endif
        StreamWriter file;
        if(File.Exists(destination)) {
            file = new StreamWriter(destination, false);
            file.WriteLine(_data);
            file.Close();
            Debug.Log("Data teroverwrite");
        }
        else {
            file = File.CreateText(destination);
            file.WriteLine(_data);
            file.Close();
            Debug.Log("Data tersimpan.");
        }
    }

    bool IsSaveDataAvailable() {
        string destination = "";

        #if UNITY_ANDROID
            destination = Application.persistentDataPath + "/save.data";
        #endif
        #if UNITY_EDITOR || UNITY_STANDALONE
            destination = Application.dataPath + "/save.data"; 
        #endif

        if(File.Exists(destination)) {
            return true;
        }
        else {
            return false;
        }
    }

    string GetSavedData() {
        string destination = "";

        #if UNITY_ANDROID
            destination = Application.persistentDataPath + "/save.data";
        #endif
        #if UNITY_EDITOR || UNITY_STANDALONE
            destination = Application.dataPath + "/save.data"; 
        #endif

        StreamReader file;
        string ret;
        if(File.Exists(destination)) {
            file = File.OpenText(destination);
            ret = file.ReadToEnd();
            file.Close();
        }
        else {
            Debug.Log("File tidak ada.");
            ret = "";
        }
        return ret;
    }

    private void OnApplicationQuit() {
        Debug.Log("Application quit");
        SaveGame();
    }

    public IWorker worker;
    [Header("Model Stuff")]
    public NNModel modelFile;
    public Texture2D testTexture;
    const int IMAGE_SIZE = 32;
	const string INPUT_NAME = "conv2d_1_input";
	const string OUTPUT_NAME = "dense_2/Softmax";
    void InitBarracuda() {
        var model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        StartCoroutine(TestModelRoutine(testTexture));
    }

    IEnumerator TestModelRoutine(Texture2D tex) {
        Tensor tensor = XUtils.TransformInput3(tex);

		var inputs = new Dictionary<string, Tensor> {
			{ INPUT_NAME, tensor }
		};

		DataLoader.GetInstance().worker.Execute(inputs);
		Tensor outputTensor = DataLoader.GetInstance().worker.PeekOutput(OUTPUT_NAME);

        //dispose tensors
        tensor.Dispose();
		outputTensor.Dispose();
		yield return null;
        LoadData();
	}

    public Noedify.Net net;
    void InitNet() {
        net = new Noedify.Net();
        net.LoadModel("savedModel1");//, Application.streamingAssetsPath);
    }
}

[System.Serializable]
public class LevelHuruf {

    public LevelHuruf(int _no, string _huruf, bool _isplayed, int _skor) {
        no = _no;
        huruf = _huruf;
        isPlayed = _isplayed;
        skor = _skor;
    }
    public int no;
    public string huruf;
    public bool isPlayed;
    public int skor;
}

[System.Serializable]
public class GameSaving {
    public LevelHuruf[] levelhuruf;
}