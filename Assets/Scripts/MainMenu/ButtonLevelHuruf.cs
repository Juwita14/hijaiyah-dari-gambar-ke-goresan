using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonLevelHuruf : MonoBehaviour
{
    [SerializeField] Sprite[] starSprites;
    TMP_Text hurufText;
    Image starImage;
    Button button;

    void Awake() {
        Init();
    }

    void Init() {
        hurufText = transform.Find("huruf_img").Find("huruf_txt").GetComponent<TMP_Text>();
        starImage = transform.Find("stars_img").GetComponent<Image>();
        button = GetComponent<Button>();
    }

    int indexHuruf;

    public void Activate(int index) {
        indexHuruf = index;
        if(hurufText==null) Init();
        starImage.sprite = starSprites[DataLoader.GetInstance().gameSaving.levelhuruf[index].skor];
        bool isPlayed = DataLoader.GetInstance().gameSaving.levelhuruf[index].isPlayed;
        hurufText.text = Konstanta.GetArabicHuruf(index);
        GetComponent<Button>().interactable = isPlayed;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClick);
    }

    public void ReActivateData(int index) {
        starImage.sprite = starSprites[DataLoader.GetInstance().gameSaving.levelhuruf[index].skor];
        bool isPlayed = DataLoader.GetInstance().gameSaving.levelhuruf[index].isPlayed;
        hurufText.text = Konstanta.GetArabicHuruf(index);
        GetComponent<Button>().interactable = isPlayed;
    }

    public void OnButtonClick() {
        DataLoader.GetInstance().selectedHuruf = indexHuruf;
        Debug.Log($"button clicked: set select huruf menjadi {DataLoader.GetInstance().selectedHuruf}");
        Main.GetInstance().PlayLatihan();
    }
}