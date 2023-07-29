using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE {
    MainMenu,
    Latihan,
    Evaluasi
}

public class IrmanSceneManager : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(this);
    }
    public void GoToLatihan() {
        SceneManager.LoadScene((int)SCENE.Latihan);
    }

    public void GoToEvaluasi() {
        SceneManager.LoadScene((int)SCENE.Evaluasi);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene((int)SCENE.MainMenu);
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            int scenecase = SceneManager.GetActiveScene().buildIndex;

            switch ((SCENE)scenecase)
            {
                case SCENE.MainMenu:
                    Application.Quit();
                    break;
                case SCENE.Evaluasi:
                    GoToMainMenu();
                    break;
                case SCENE.Latihan:
                    GoToMainMenu();
                    break;
                default:
                    break;
            }
        }
    }
}