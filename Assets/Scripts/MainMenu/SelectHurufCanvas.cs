using UnityEngine;
using UnityEngine.UI;

public class SelectHurufCanvas : IrmanCanvas
{
    [SerializeField] Transform hurufs;

    protected override void Awake() {
        base.Awake();
        Init();
    }

    void Init() {
        int count = hurufs.childCount;
        for (int i = 0; i < count; i++)
        {
            hurufs.GetChild(i).GetComponent<ButtonLevelHuruf>().Activate(i);
        }
    }

    void OnEnable() {
        Debug.Log("onenable select huruf");
        int count = hurufs.childCount;
        for (int i = 0; i < count; i++)
        {
            hurufs.GetChild(i).GetComponent<ButtonLevelHuruf>().ReActivateData(i);
        }
    }
}