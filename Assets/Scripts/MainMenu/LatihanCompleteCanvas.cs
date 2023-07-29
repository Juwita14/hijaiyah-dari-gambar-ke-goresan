using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LatihanCompleteCanvas : IrmanCanvas
{
    [SerializeField] RectTransform bg;
    [SerializeField] RectTransform mainPanel;

    protected override void Awake() {
        base.Awake();
        Init();
    }

    void Init() {

    }

    [SerializeField] float delayShow = 0.0f;
    [SerializeField] UnityEvent actionOnStartShow;
    [SerializeField] LeanTweenType easingShow = LeanTweenType.easeOutBack;
    [SerializeField] LeanTweenType easingHide = LeanTweenType.easeInBack;

    public override void ShowCanvas() {
        base.ShowCanvas();
        mainPanel.localScale = Vector3.zero;
        bg.LeanAlpha(0f,0f);
        bg.LeanAlpha(.75f, Konstanta.PANEL_TWEEN_DURATION).setEase(easingShow).setDelay(delayShow).setOnStart(()=>{actionOnStartShow?.Invoke();});
        mainPanel.LeanScale(Vector3.one, Konstanta.PANEL_TWEEN_DURATION).setEase(easingShow).setDelay(delayShow);
    }

    public override void HideCanvas() {
        bg.LeanAlpha(0f, Konstanta.PANEL_TWEEN_DURATION).setEase(easingHide);
        mainPanel.LeanScale(Vector3.zero, Konstanta.PANEL_TWEEN_DURATION).setEase(easingHide).setOnComplete(()=>{base.HideCanvas();});
    }
}