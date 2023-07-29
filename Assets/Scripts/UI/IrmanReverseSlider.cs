using System;
using UnityEngine;
using UnityEngine.UI;

public class IrmanReverseSlider : MonoBehaviour
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
        UpdateView();
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

    public void KurangiNilai() {
        SetValue(value - 1f*valueMultiplier);
    }

    void CheckPoint() {
        // Debug.Log($"Normal Value: {GetNormalValue()}");
        if(GetNormalValue()<(float)pointsValue[1]/maxNilai) {
            DeactivatePoint(1);
        }
        else if(GetNormalValue()<(float)pointsValue[2]/maxNilai) {
            DeactivatePoint(2);
        }
        // else if(GetNormalValue()<(float)pointsValue[2]/maxNilai) {
        //     DeactivatePoint(2);
        // }
    }
    
    public int GetFinalStar() {
        int star;
        if(GetNormalValue()>=(float)pointsValue[2]/maxNilai) {
            star = 3;
        }
        else if(GetNormalValue()>=(float)pointsValue[1]/maxNilai) {
            star = 2;
        }
        // else if(GetNormalValue()>=(float)pointsValue[0]/maxNilai) {
        //     star = 1;
        // }
        else star = 0;
        return star;
    }

    void DeactivatePoint(int _pointvalue) {
        
            Image tempImg = points[_pointvalue].GetChild(0).GetComponent<Image>();
            tempImg.sprite = pointInactive_spr;
            tempImg.rectTransform.localScale = Vector3.zero;
            LeanTween.scale(tempImg.rectTransform, Vector3.one, 0.2f).setIgnoreTimeScale(true);
        
    }

    public Action onValueChanged;

    public void SetOnValueChanged(Action _onChanged) {
        onValueChanged = _onChanged;
    }

    void UpdateView() {
        LeanTween.value(gameObject, UpdateRect, fillRect.anchorMax.x, GetNormalValue(), .2f).setIgnoreTimeScale(true)
        .setOnComplete(CheckPoint);
    }

    void UpdateRect(float _value) {
       // Debug.Log(_value);
        fillRect.anchorMax = new Vector2(_value, fillRect.anchorMax.y);
    }

    public void Reset() {
        SetValue(maxNilai);
        // fillRect.anchorMax = new Vector2(0, fillRect.anchorMax.y);
        points[0].GetChild(0).GetComponent<Image>().sprite = pointActive_spr;
        points[1].GetChild(0).GetComponent<Image>().sprite = pointActive_spr;
        points[2].GetChild(0).GetComponent<Image>().sprite = pointActive_spr;
        // value = minNilai;
    }

    void OnDisable() {
        Reset();
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.PageDown)) {
            KurangiNilai();
        }
        else if(Input.GetKeyUp(KeyCode.Home)) {
            Reset();
        }
    }
}