using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvas : IrmanCanvas
{
    [SerializeField] Toggle dontDisplayToggle;
    [SerializeField] bool show;
    public bool Show => show;

    void Start() {
        dontDisplayToggle.onValueChanged.AddListener(OnDisplayToggleChanged);
    }

    void OnDisplayToggleChanged(bool value) {
        show=!value;
        // if(value==true) {
        //     PlayerPrefs.SetInt("DONTDISPLAYTUT2",1);
        // }
        // else {
        //     PlayerPrefs.SetInt("DONTDISPLAYTUT2",0);
        // }
    }
}