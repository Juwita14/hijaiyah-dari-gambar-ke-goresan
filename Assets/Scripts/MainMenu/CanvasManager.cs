using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    Main main;

    public void Activate(Main _main) {
        main = _main;
    }
    
    [SerializeField] IrmanCanvas mainMenuCanvas;
    [SerializeField] IrmanCanvas selectModulCanvas;
    [SerializeField] IrmanCanvas selectHurufLatihanCanvas;
    [SerializeField] IrmanCanvas quitPromptCanvas;
    [SerializeField] IrmanCanvas settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        PlayGame();
        GoToMenu(1);
    }

    public void PlayGame() {
        mainMenuCanvas.HideCanvas();
        selectModulCanvas.HideCanvas();
        selectHurufLatihanCanvas.HideCanvas();
    }

    public void GoToMenu(int index) {
        PlayGame();
        if(index==1) {
            mainMenuCanvas.ShowCanvas();
            canQuit = true;
        }
        else if(index==2) {selectModulCanvas.ShowCanvas(); canQuit=false;}
        else if(index==3) {selectHurufLatihanCanvas.ShowCanvas(); canQuit=false;}
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Backspace)) {
            QuitGame();
        }
    }

    bool canQuit = false;

    void QuitGame() {
        if(canQuit) {
            quitPromptCanvas.ShowCanvas();
        }
    }

    public void ShowSetting() {
        settingCanvas.ShowCanvas();
    }

    public void HideSetting() => settingCanvas.HideCanvas();

    //backdoor to train scene
    public void GoToTrainScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
