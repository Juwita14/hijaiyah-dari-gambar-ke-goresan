using System;
using UnityEngine;
using UnityEngine.UI;

public class IrmanSlider : MonoBehaviour
{
    [SerializeField] RectTransform fillRect;

    public float minNilai=0;
    public float maxNilai=10;
    [SerializeField] private float value=0;
    [SerializeField] private float valueMultiplier=1;

    [SerializeField] Sprite pointActive_spr;
    [SerializeField] Sprite pointInactive_spr;

    [SerializeField] Transform[] points;

    [SerializeField] int[] pointsValue;

    void Awake() {
        Init();
        Activate();
    }

    public void Init(Action _onValueChanged=null) {
        if(_onValueChanged!=null) onValueChanged = _onValueChanged;
    }

    public void Activate() {
        // UpdateView();
        UpdateRect(GetNormalValue());
    }

    public float GetValue() {
        return value;
    }

    public float GetNormalValue() { //0-1
        return value/(maxNilai-minNilai);
    }

    public void SetValue(float _value) {
        if(_value<minNilai) value=minNilai;
        else if(_value>maxNilai) value=maxNilai;
        else value = _value;

        UpdateView();

        if(onValueChanged!=null) onValueChanged.Invoke();
    }

    public void SetValueWithoutTweening(float _value) {
        if(_value<minNilai) value=minNilai;
        else if(_value>maxNilai) value=maxNilai;
        else value = _value;

        UpdateRect(GetNormalValue());
        if(onValueChanged!=null) onValueChanged.Invoke();
    }

    public void AddNilai() {
        SoundController.GetInstance().PlayCharging();
        SetValue(value + 1f*valueMultiplier);
    }

    public void KurangiNilai() {
        SetValue(value - 1f*valueMultiplier);
    }

    void CheckPoint() {
        // Debug.Log($"Normal Value: {GetNormalValue()}");
        if(GetNormalValue()==pointsValue[2]/maxNilai) {
            ActivatePoint(2);
        }
        else if(GetNormalValue()==pointsValue[1]/maxNilai) {
            ActivatePoint(1);
        }
        else if(GetNormalValue()==pointsValue[0]/maxNilai) {
            ActivatePoint(0);
        }
    }
    
    public int GetFinalStar() {
        int star;
        if(GetNormalValue()>pointsValue[2]/maxNilai) {
            star = 3;
        }
        else if(GetNormalValue()>pointsValue[1]/maxNilai) {
            star = 2;
        }
        else if(GetNormalValue()>pointsValue[0]/maxNilai) {
            star = 1;
        }
        else star = 0;
        return star;
    }

    void ActivatePoint(int _pointvalue) {
        Image tempImg = points[_pointvalue].GetChild(0).GetComponent<Image>();
        // tempImg.rectTransform.localScale = Vector3.zero;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(tempImg.rectTransform, Vector3.zero, 0.1f).setEaseInOutQuad().setIgnoreTimeScale(true));
        seq.append(()=>{tempImg.sprite = pointActive_spr;});
        seq.append(LeanTween.scale(tempImg.rectTransform, Vector3.one, 0.1f).setEaseInOutQuad().setIgnoreTimeScale(true));
    }

    void DeactivatePoint(int _pointvalue) {
        Image tempImg = points[_pointvalue].GetChild(0).GetComponent<Image>();
        // tempImg.sprite = pointInactive_spr;
        // tempImg.rectTransform.localScale = Vector3.zero;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(tempImg.rectTransform, Vector3.zero, 0.1f).setEaseInOutQuad().setIgnoreTimeScale(true));
        seq.append(()=>{tempImg.sprite = pointInactive_spr;});
        seq.append(LeanTween.scale(tempImg.rectTransform, Vector3.one, 0.1f).setEaseInOutQuad().setIgnoreTimeScale(true));
    }

    public Action onValueChanged;

    public void SetOnValueChanged(Action _onChanged) {
        onValueChanged = _onChanged;
    }

    void UpdateView() {
        LeanTween.value(gameObject, UpdateRect, fillRect.anchorMax.x, GetNormalValue(), .2f).setEaseInCubic().setIgnoreTimeScale(true)
        .setOnComplete(CheckStatus);
    }

    void UpdateRect(float _value) {
       // Debug.Log(_value);
        fillRect.anchorMax = new Vector2(_value, fillRect.anchorMax.y);
    }

    [SerializeField] SliderType sliderType = SliderType.Normal;

    public void Reset() {
        float nilai=minNilai;
        Sprite sprite =pointInactive_spr;
        if(sliderType==SliderType.Reverse) {nilai=maxNilai;sprite=pointActive_spr;}

        SetValueWithoutTweening(nilai);
        
        points[0].GetChild(0).GetComponent<Image>().sprite = sprite;
        points[1].GetChild(0).GetComponent<Image>().sprite = sprite;
        points[2].GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    void OnDisable() {
        Reset();
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.PageUp)) {
            // SetValue(GetValue()+1f);
        }
        else if(Input.GetKeyUp(KeyCode.Home)) {
            Reset();
        }
    }

    [SerializeField] bool[] starStatus = new bool[3]{false,false,false};
    [SerializeField] bool[] tempStatus = new bool[3]{false,false,false};
    bool[] HitungStatus() {
        for (int i = 0; i < 3; i++)
        {
            if(GetNormalValue()>=pointsValue[i]/maxNilai) tempStatus[i] = true;
            else tempStatus[i] = false;
        }
        return tempStatus;
    }
    void CheckStatus() {
        bool[] checkedStatus = HitungStatus();
        for (int i = 0; i < 3; i++)
        {
            if(checkedStatus[i]!=starStatus[i]) {
                // Debug.Log($"status ke {i} berubah dari {starStatus[i]} ke {checkedStatus[i]}");
                if(checkedStatus[i]==true) {
                    ActivatePoint(i);
                    SoundController.GetInstance().PlayCorrect();
                }
                else {
                    DeactivatePoint(i);
                    SoundController.GetInstance().PlayWrong();
                }

                starStatus[i] = checkedStatus[i];
            }
            else {
                // Debug.Log($"status ke {i} masih sama kok");
            }
        }
    }

}

public enum SliderType {
    Normal,
    Reverse
}