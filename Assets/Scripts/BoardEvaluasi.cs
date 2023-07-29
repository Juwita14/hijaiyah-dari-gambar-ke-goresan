using UnityEngine;
using UnityEngine.EventSystems;

public class BoardEvaluasi : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] IrmanDraw drawer;

    public void OnPointerDown(PointerEventData eventData)
    {
        drawer.onDownBoard = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drawer.onDownBoard = false;
    }
}