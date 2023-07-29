using UnityEngine;
using UnityEngine.UI;

public class SettingCanvas : IrmanCanvas
{
    [SerializeField] RectTransform bg;
    [SerializeField] RectTransform mainPanel;

    [SerializeField] Slider music_slider, sound_slider;

    protected override void Awake() {
        base.Awake();
        Init();
    }

    void Init() {
        music_slider.onValueChanged.AddListener(OnMusicVolChange);
        sound_slider.onValueChanged.AddListener(OnSoundVolChange);

        music_slider.value = PlayerPrefs.GetFloat("BGMVOL", SoundController.GetInstance().bgmAudio.volume);
        sound_slider.value = PlayerPrefs.GetFloat("SFXVOL", SoundController.GetInstance().sfxAudio.volume);
    }

    void OnMusicVolChange(float _value) {
        SoundController.GetInstance().SetBGMVolume(_value);
        PlayerPrefs.SetFloat("BGMVOL", _value);
    }

    void OnSoundVolChange(float _value) {
        SoundController.GetInstance().SetSFXVolume(_value);
        PlayerPrefs.SetFloat("SFXVOL", _value);
    }

    public override void ShowCanvas() {
        base.ShowCanvas();
        mainPanel.localScale = Vector3.zero;
        bg.LeanAlpha(0f,0f);
        bg.LeanAlpha(.75f, Konstanta.PANEL_TWEEN_DURATION).setEaseInOutBack();
        mainPanel.LeanScale(Vector3.one, Konstanta.PANEL_TWEEN_DURATION).setEaseInOutBack();
    }

    public override void HideCanvas() {
        bg.LeanAlpha(0f, Konstanta.PANEL_TWEEN_DURATION).setEaseInOutBack();
        mainPanel.LeanScale(Vector3.zero, Konstanta.PANEL_TWEEN_DURATION).setEaseInOutBack().setOnComplete(()=>{base.HideCanvas();});
    }
}