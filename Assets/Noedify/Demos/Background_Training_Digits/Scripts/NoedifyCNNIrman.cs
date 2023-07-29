using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoedifyCNNIrman : MonoBehaviour
{
    /*
        This example script will import a handwritten digit dataset and train 
        a 2D convolutional model during runtime using parallel processing.
    */

    public float trainingRate = 0.4f;

    public int no_epochs = 10;
    public int batch_size = 2;

    public Noedify_Solver.CostFunction costFunction;

    public Noedify_Debugger debugger;
    public Transform costPlotOrigin;

    public TestPredictions_BackgroundTraining predictionTester;

    public UnityEngine.UI.Toggle solverMethodToggle;

    [Header("IRMAN training images")]

    public Texture2D[] IRMAN_images0,IRMAN_images1,IRMAN_images2,IRMAN_images3,IRMAN_images4,IRMAN_images5,
    IRMAN_images6,IRMAN_images7,IRMAN_images8,IRMAN_images9,IRMAN_images10;

    Noedify.Net net;
    Noedify_Solver solver;

    int no_labels = 30; // number of output labels

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    void Awake() {
        JUMLAH_HURUF = Konstanta.REAL_OUTPUT_SIZE;
        // LoadDataset();
        //CopyArrayToIRMAN();
    }

    int JUMLAH_DATA_TRAIN = 12;
    int JUMLAH_HURUF = 30;

    public List<Texture2D[]> LoadedDatasets = new List<Texture2D[]>();

    private void LoadDataset() {
        for (int i = 0; i < JUMLAH_HURUF; i++) {
            Texture2D[] texs = new Texture2D[JUMLAH_DATA_TRAIN];
            for (int j = 0; j < JUMLAH_DATA_TRAIN; j++) {
                texs[j] = DatasetRegister.GetInstance().GetTexture(i,j+1);
            }
            LoadedDatasets.Add(texs);
            Debug.Log("Successfully load huruf : " + DatasetRegister.GetInstance().GetHuruf(i));
        }
    }

    void CopyArrayToIRMAN () {
        int len = LoadedDatasets[0].Length;
        IRMAN_images0 = new Texture2D[len];
        IRMAN_images1 = new Texture2D[len];
        IRMAN_images2 = new Texture2D[len];
        IRMAN_images3 = new Texture2D[len];
        IRMAN_images4 = new Texture2D[len];
        IRMAN_images5 = new Texture2D[len];
        IRMAN_images6 = new Texture2D[len];
        IRMAN_images7 = new Texture2D[len];
        IRMAN_images8 = new Texture2D[len];
        IRMAN_images9 = new Texture2D[len];
        IRMAN_images10 = new Texture2D[len];
        LoadedDatasets[0].CopyTo(IRMAN_images0, 0);
        LoadedDatasets[1].CopyTo(IRMAN_images1, 0);
        LoadedDatasets[2].CopyTo(IRMAN_images2, 0);
        LoadedDatasets[3].CopyTo(IRMAN_images3, 0);
        LoadedDatasets[4].CopyTo(IRMAN_images4, 0);
        LoadedDatasets[5].CopyTo(IRMAN_images5, 0);
        LoadedDatasets[6].CopyTo(IRMAN_images6, 0);
        LoadedDatasets[7].CopyTo(IRMAN_images7, 0);
        LoadedDatasets[8].CopyTo(IRMAN_images8, 0);
        LoadedDatasets[9].CopyTo(IRMAN_images9, 0);
        LoadedDatasets[10].CopyTo(IRMAN_images10, 0);
    }

    void Start()
    {
        LoadDataset();
        CopyArrayToIRMAN();
        BuildModel();
        //BuildConvModel();
    }

    void BuildConvModel() {
        net = new Noedify.Net();
        
        Noedify.Layer inputLayer = new Noedify.Layer(
            Noedify.LayerType.Input2D,
            new int[2] { 32, 32 },
            1,
            "input layer");
        net.AddLayer(inputLayer);

        Noedify.Layer hiddenLayer0 = new Noedify.Layer(Noedify.LayerType.Convolutional2D, 
            inputLayer,
            new int[2] { 4, 4 }, // filter size
            new int[2] { 2, 2 }, // stride
            8, // # of filters
            new int[2], //#padding
            Noedify.ActivationFunction.ReLU, "convolutional 1"
            );
        net.AddLayer(hiddenLayer0);

        #region backup
        Noedify.Layer poolingLayer0 = new Noedify.Layer(Noedify.LayerType.Pool2D, 
            hiddenLayer0, 
            new int[2] { 2, 2 }, //shape 
            new int[2] { 1, 1 }, //stride
            new int[2], //padding
            Noedify.PoolingType.Max, "MaxPooling");
        #endregion

        Noedify.Layer hiddenLayer1 = new Noedify.Layer(
            Noedify.LayerType.FullyConnected, 
            100, 
            Noedify.ActivationFunction.Sigmoid, 
            "fully connected 1");
        net.AddLayer(hiddenLayer1);

        Noedify.Layer outputLayer = new Noedify.Layer(Noedify.LayerType.Output, 
            no_labels, // layer size
            Noedify.ActivationFunction.Sigmoid, "output layer" );
        net.AddLayer(outputLayer);

        net.BuildNetwork();
    }

    void BuildModel()
    {
        net = new Noedify.Net();

        /* Input layer */
        
        Noedify.Layer inputLayer = new Noedify.Layer(
            Noedify.LayerType.Input2D, // layer type
            new int[2] { 32, 32 }, // input size
            1, // # of channels
            "input layer" // layer name
            );
        net.AddLayer(inputLayer);

        // Hidden layer 2
        Noedify.Layer hiddenLayer1 = new Noedify.Layer(
            Noedify.LayerType.FullyConnected, // layer type
            200,//100,
            Noedify.ActivationFunction.Sigmoid,
            "fully connected 1" // layer name
            );
        net.AddLayer(hiddenLayer1);

        // Output layer 
        Noedify.Layer outputLayer = new Noedify.Layer(
            Noedify.LayerType.Output, // layer type
            no_labels, // layer size
            Noedify.ActivationFunction.SoftMax, // activation function
            "output layer" // layer name
            );
        net.AddLayer(outputLayer);

        net.BuildNetwork();
    }

    public void TrainModel()
    {
        List<float[,,]> trainingData = new List<float[,,]>();
        List<float[]> outputData = new List<float[]>();

        // List<Texture2D[]> IRMAN_images = new List<Texture2D[]>();

        // IRMAN_images.Add(IRMAN_images0);
        // IRMAN_images.Add(IRMAN_images1);
        // IRMAN_images.Add(IRMAN_images2);
        // IRMAN_images.Add(IRMAN_images3);
        // IRMAN_images.Add(IRMAN_images4);
        // IRMAN_images.Add(IRMAN_images5);
        // IRMAN_images.Add(IRMAN_images6);
        // IRMAN_images.Add(IRMAN_images7);
        // IRMAN_images.Add(IRMAN_images8);
        // IRMAN_images.Add(IRMAN_images9);
        // IRMAN_images.Add(IRMAN_images10);

        // Debug.Log(trainingData);
        // Debug.Log(outputData);
        // Debug.Log(IRMAN_images[0]);
        if(LoadedDatasets==null){
            Debug.Log("datasetnull");
        }
        Debug.Log("countdiluar:"+LoadedDatasets.Count);
        Debug.Log("countdiluarlenhuruf:"+LoadedDatasets[0].Length);

        //Noedify_Utils.ImportImageData(ref trainingData, ref outputData, IRMAN_images, true);
        XUtils.ImportImageData(ref trainingData, ref outputData, LoadedDatasets, true);
        debugger.net = net;

        

        Noedify_Solver.SolverMethod solverMethod = Noedify_Solver.SolverMethod.MainThread;
        if (solverMethodToggle != null)
            if (solverMethodToggle.isOn)
                solverMethod = Noedify_Solver.SolverMethod.Background;

        if (solver == null)
            solver = Noedify.CreateSolver();
        solver.debug = new Noedify_Solver.DebugReport();
        sw.Start();
        solver.costThreshold = 0.01f; // Add a cost threshold to prematurely end training when a suitably low error is achieved
        //solver.suppressMessages = true; // suppress training messages from appearing in editor the console
        solver.TrainNetwork(net, trainingData, outputData, no_epochs, batch_size, trainingRate, costFunction, solverMethod, null, 4);
        float[] cost = solver.cost_report;
        StartCoroutine(PlotCostWhenComplete(solver, cost));
    }

    IEnumerator PlotCostWhenComplete(Noedify_Solver solver, float[] cost)
    {
        while (solver.trainingInProgress)
        {
            yield return null;
        }
        sw.Stop();
        print("Elapsed: " + sw.ElapsedMilliseconds + " ms");
        debugger.PlotCost(cost, new float[2] { 1.0f / no_epochs * 2.5f, 5 }, costPlotOrigin);
        TestImages();
    }

    void TestImages() {
        for (int n = 0; n < predictionTester.sampleImagePlanes.Length; n++) {
            float[,,] testInputImage = new float[1,1,1];
            Noedify_Utils.ImportImageData(ref testInputImage, predictionTester.sampleImageRandomSet[n], true);
            solver.Evaluate(net, testInputImage, Noedify_Solver.SolverMethod.MainThread);
            int prediction = Noedify_Utils.ConvertOneHotToInt(solver.prediction);
            predictionTester.CNN_predictionText[n].text = Konstanta.GetRealHuruf(prediction);//prediction.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.S)) {
            net.SaveModel("savedModel1");
        }
        else if(Input.GetKeyUp(KeyCode.T)) {
            TestImages();
        }
    }

}
