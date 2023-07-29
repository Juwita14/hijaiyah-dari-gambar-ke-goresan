using UnityEngine;

public class DatasetRegister : MonoBehaviour
{
    private static DatasetRegister instance;

    public static DatasetRegister GetInstance() {
        return instance;
    }

    void Awake() {
        instance = this;
    }

    string[] hurufs = { "alif","ba","ta","tsa","jim","ha'","kha","dal","dzal","ra","za",
                                            "sin","syin","shad","dhad","tha","zha","ain","ghain","fa","qaf",
                                            "kaf","lam","mim","nun","waw","ha","lamalif","hamzah","ya"};

    string GetPath(int huruf, int nomor) {
        string nomorUrut = nomor.ToString();//"";
        // if(nomor<10) nomorUrut = "0" + nomor.ToString();
        // else nomorUrut = nomor.ToString();

        string hurufPath = "new32pxDataset/" + hurufs[huruf] + "/" + hurufs[huruf] + "_" +nomorUrut;
        return hurufPath;
    }

    public Texture2D GetTexture(int huruf, int nomor) {
        string path = GetPath(huruf, nomor);
        Debug.Log("loading:" + path);
        Texture2D loadedTex = Resources.Load<Texture2D>(path);
        return loadedTex;
    }

    public string GetHuruf(int no) {
        return hurufs[no];
    }
}