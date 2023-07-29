using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DrawingDatasetMain : MonoBehaviour 
{
    [SerializeField] Camera renderCam;
    [SerializeField] Canvas successCanvas;

    private void Start() {
        drawing = GetComponent<IrmanDraw>();
        UpdateTextHuruf();
        UpdateRectDimension();
        UpdateCamProperties();
    }

    void UpdateCamProperties() {
        //Camera.main.orthographicSize =(Screen.width>Screen.height) ? 5.65f: 5.65f*Screen.height/Screen.width;
    }

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

    void ShowSuccessCanvas() {
        successCanvas.enabled = true;
        Invoke("HideSuccessCanvas",1f);
    }

    void HideSuccessCanvas() {
        successCanvas.enabled = false;
    }

    IrmanDraw drawing;

    public Vector2 texDim = new Vector2(256,256);
    public bool enableScaling = true;
    public Vector2 texFinalDim = new Vector2(32,32);

    public bool useBilinear = true;

    public Transform lineParent;

    void MoveLine(int x=0, int y=0) {
        Vector3 addition = new Vector3(x*0.01f,y*0.01f,0);

        int len = lineParent.childCount;
        for (int i = 0; i < len; i++)
        {
            LineRenderer line = lineParent.GetChild(i).GetComponent<LineRenderer>();
            int linelen = line.positionCount;
            for (int j = 0; j < linelen; j++)
            {
                line.SetPosition(j, line.GetPosition(j) + addition); 
            }
        }
    }

    POINT RotatePoint(POINT p, float angle=0, float cx=0, float cy=0)
    {
        float sinTeta = Mathf.Sin(angle);
        float cosTeta = Mathf.Cos(angle);

        // translate point back to origin:
        p.x -= cx;
        p.y -= cy;

        // rotate point
        float xnew = p.x * cosTeta - p.y * sinTeta;
        float ynew = p.x * sinTeta + p.y * cosTeta;

        // translate point back:
        p.x = xnew + cx;
        p.y = ynew + cy;
        return p;
    }

    POINT ResizePoint(POINT p, float scale=1, float cx=0, float cy=0)
    {
        // translate point back to origin:
        p.x -= cx;
        p.y -= cy;

        // rotate point
        float xnew = scale * p.x;
        float ynew = scale * p.y;

        // translate point back:
        p.x = xnew + cx;
        p.y = ynew + cy;
        return p;
    }

    public struct POINT
    {
        public POINT(float _x, float _y) {
            x = _x;
            y = _y;
        }
        public float x;
        public float y;
    }

    void RotateLine(float angle) {
        angle *= Mathf.Deg2Rad;
        int len = lineParent.childCount;
        for (int i = 0; i < len; i++)
        {
            LineRenderer line = lineParent.GetChild(i).GetComponent<LineRenderer>();
            int linelen = line.positionCount;
            for (int j = 0; j < linelen; j++)
            {
                POINT poin = new POINT(0,0);
                poin.x = line.GetPosition(j).x;
                poin.y = line.GetPosition(j).y;
                POINT px = RotatePoint(poin, angle);
                line.SetPosition(j, new Vector3(px.x, px.y, 0)); 
            }
        }
    }

    void ResizeLine(float scale=1) {
        int len = lineParent.childCount;
        for (int i = 0; i < len; i++)
        {
            LineRenderer line = lineParent.GetChild(i).GetComponent<LineRenderer>();
            int linelen = line.positionCount;
            POINT poin = new POINT(0,0);
            for (int j = 0; j < linelen; j++)
            {
                poin.x = line.GetPosition(j).x;
                poin.y = line.GetPosition(j).y;
                POINT px = ResizePoint(poin, scale);
                line.SetPosition(j, new Vector3(px.x, px.y, 0)); 
            }
        }
    }

    public int[] lineMove = new int[2] {2,2};
    public float rotationAngle = 0;

    public float scaleFactor = 1f;

    public void GenerateVariasi() {
        var seq = LeanTween.sequence();
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(2);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);     SaveTexture(3);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);    SaveTexture(4);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);    SaveTexture(5);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);    SaveTexture(6);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);    SaveTexture(7);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(8);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(9);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);   SaveTexture(1);});

        seq.append(.1f);seq.append(()=>{RotateLine(5);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(12);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);    SaveTexture(13);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(14);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(15);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(16);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(17);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(18);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(19);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);  SaveTexture(11);});

        seq.append(.1f);seq.append(()=>{RotateLine(-10);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(22);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);    SaveTexture(23);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(24);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(25);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(26);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(27);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(28);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(29);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);  SaveTexture(21);});

        seq.append(.1f);seq.append(()=>{RotateLine(5);});
        seq.append(.1f);seq.append(()=>{ResizeLine(1.2f);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(32);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);     SaveTexture(33);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);    SaveTexture(34);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);    SaveTexture(35);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);    SaveTexture(36);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);    SaveTexture(37);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(38);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);     SaveTexture(39);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);   SaveTexture(31);});

        seq.append(.1f);seq.append(()=>{RotateLine(5);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(42);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);    SaveTexture(43);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(44);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(45);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(46);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(47);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(48);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(49);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);  SaveTexture(41);});

        seq.append(.1f);seq.append(()=>{RotateLine(-10);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(52);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,0);    SaveTexture(53);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(54);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,-30);   SaveTexture(55);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(56);});
        seq.append(.1f);seq.append(()=>{MoveLine(-30,0);   SaveTexture(57);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(58);});
        seq.append(.1f);seq.append(()=>{MoveLine(0,30);    SaveTexture(59);});
        seq.append(.1f);seq.append(()=>{MoveLine(30,-30);  SaveTexture(51);});
        seq.append(.1f);seq.append(()=>{ShowSuccessCanvas();drawing.DeleteDrawing();});
    }

    public void SaveTexture(int count) {
        /*
        Texture2D tempTex;
        if(enableScaling) {
            tempTex = GetSampleDrawing(new int[2] {(int)texDim.x,(int)texDim.y});
            if(useBilinear) TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
            else TextureScale.Point(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
        }
        else {
            tempTex = GetSampleDrawing(new int[2] {(int)texDim.x,(int)texDim.y});
        }
        SaveTextureToFile(tempTex, count);
        Debug.Log("Texture Saved!");
        */
        // ShowSuccessCanvas();
        // drawing.DeleteDrawing();

        Texture2D tempTex = GetSampleDrawing2();
        TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
        SaveTextureToFile(tempTex, count);
    }

    Texture2D GetSampleDrawing(int[] dim)
    {
        Texture2D tex = new Texture2D(dim[0], dim[1], TextureFormat.RGB24, false);
        RenderTexture rt = new RenderTexture(dim[0], dim[1], 24);
        renderCam.targetTexture = rt;
        renderCam.Render();
        RenderTexture.active = rt;
        Rect rectReadPixels = new Rect(0, 0, dim[0], dim[1]);
        tex.ReadPixels(rectReadPixels, 0, 0);
        tex.Apply();

        renderCam.targetTexture = null;

        return tex;
    }

    public Vector2 rectdim = new Vector2(0,0);
    public Vector2 readpixeldest = new Vector2(0,0);

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

    [SerializeField] int curHuruf = 0;
    public Text hurufText;

    void UpdateTextHuruf() {
        hurufText.text = Konstanta.GetRealHuruf(curHuruf).ToUpper();
    }

    public void NextHuruf() {
        if(curHuruf<Konstanta.REAL_OUTPUT_SIZE-1) curHuruf++;
        else curHuruf = 0;
        UpdateTextHuruf();
    }

    public void PrevHuruf() {
        if(curHuruf>0) curHuruf--;
        else curHuruf = Konstanta.REAL_OUTPUT_SIZE-1;
        UpdateTextHuruf();
    }

    void SaveTextureToFile(Texture2D _tex, int count, string _path=null) {
        if(_path==null) _path = Application.persistentDataPath;
        string time =  System.DateTime.Now.ToString("ddMMyyHHmmss");
        string fileName = Konstanta.GetRealHuruf(curHuruf) + time.ToString() + count.ToString();

        if(!Directory.Exists(_path + "/FOLDER")) Directory.CreateDirectory(_path + "/FOLDER");

        var pngData = _tex.EncodeToPNG();
        if (pngData != null )
            File.WriteAllBytes(_path + "/FOLDER/" + fileName + ".png", pngData);
        else
            Debug.Log("Could not convert " + fileName + " to png. Skipping saving texture");
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Space)) {
            SaveTexture(32423);
        }
        else if(Input.GetKeyUp(KeyCode.RightAlt)) {
            if(Screen.width > Screen.height) {
                Texture2D tempTex = GetSampleDrawing2();
                TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
                SaveTextureToFile(tempTex, 12313123);
            }
            else {
                Texture2D tempTex = GetSampleDrawing2();
                TextureScale.Bilinear(tempTex, (int)texFinalDim.x, (int)texFinalDim.y);
                SaveTextureToFile(tempTex, 12313123);
            }
        }
    }

    private void OnApplicationQuit() {
        // ZipFiles();
    }

    void ZipFiles() {
        string dir = Application.persistentDataPath + "/FOLDER";
        string dest = Application.persistentDataPath + "/thanks" + System.DateTime.Now.ToString("ddMMyyHHmmss") + ".zip";

        // System.IO.Compression.ZipFile.CreateFromDirectory(dir,dest);
    }
}