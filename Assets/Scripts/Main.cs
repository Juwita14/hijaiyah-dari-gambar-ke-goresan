using UnityEngine;

public class Main : MonoBehaviour
{
    static Main instance;
    public static Main GetInstance() {
        return instance;
    }

    [SerializeField] CanvasManager mainMenu;
    [SerializeField] public MainLatihan mainLatihan;
    [SerializeField] EvaluationMain mainEvaluasi;

    [SerializeField] Camera menuCamera;
    [SerializeField] Camera latihanCamera;
    [SerializeField] Camera evaluasiCamera;

    private void Awake() {
        instance = this;
        mainMenu.Activate(this);
    }

    private void Start() {
        SoundController.GetInstance().PlayBGM(BGMType.Menu);
        GoToMenu();
    }

    public void PlayLatihan() {
        SoundController.GetInstance().PlayBGM(BGMType.Gameplay);
        mainMenu.PlayGame();
        mainLatihan.gameObject.SetActive(true);
        mainLatihan.Mulai();
        SwitchCamera(2);
    }

    public void PlayEvaluasi() {
        SoundController.GetInstance().PlayBGM(BGMType.Gameplay);
        mainMenu.PlayGame();
        mainEvaluasi.gameObject.SetActive(true);
        // mainEvaluasi.Play();
        SwitchCamera(3);
    }

    public void GoToMenu() {
        SoundController.GetInstance().PlayBGM(BGMType.Menu);
        mainMenu.GoToMenu(1);
        mainLatihan.gameObject.SetActive(false);
        mainEvaluasi.gameObject.SetActive(false);
        SwitchCamera(1);
    }

    public void GoToSelectHuruf() {
        SoundController.GetInstance().PlayBGM(BGMType.Menu);
        mainMenu.GoToMenu(3);
        mainLatihan.gameObject.SetActive(false);
        mainEvaluasi.gameObject.SetActive(false);
        SwitchCamera(1);
    }

    public void SwitchCamera(int cam) {
        menuCamera.enabled = false;
        latihanCamera.enabled = false;
        evaluasiCamera.enabled = false;
        switch (cam)
        {
            case 1: menuCamera.enabled=true;break;
            case 2: latihanCamera.enabled=true;break;
            case 3: evaluasiCamera.enabled=true;break;
            default:break;
        }
    }

    public void QuitGame() {
        Application.Quit(1);
    }
}