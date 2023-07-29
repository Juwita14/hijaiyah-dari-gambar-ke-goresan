using UnityEngine;
using UnityEngine.UI;

public class IrmanCanvas : MonoBehaviour
{
    Canvas canvas;
    GraphicRaycaster raycaster;

    protected virtual void Awake() {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    public virtual void ShowCanvas() {
        gameObject.SetActive(true);
        canvas.enabled = true;
        raycaster.enabled = true;
    }

    public virtual void HideCanvas() {
        gameObject.SetActive(false);
        canvas.enabled = false;
        raycaster.enabled = false;
    }
}