using UnityEngine;
using UnityEngine.UI;

public class ThreeStar : MonoBehaviour
{
    [SerializeField] int value; //0-3
    [SerializeField] Image[] stars_img;

    [SerializeField] Sprite on_spr, off_spr;

    public void SetValue(int _value) {
        value = _value;
        UpdateUI();
    }

    void UpdateUI() {
        for (int i = 0; i < stars_img.Length; i++) {
            stars_img[i].sprite = (i<value)?on_spr:off_spr;
        }
    }
    
}