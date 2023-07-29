using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TraceLineManagerName;

namespace startPoints
{
    public class StartPointHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private DrawAreaHandler drawArea;
        //for point handle
        private Canvas canvas;
        [HideInInspector] public RectTransform startRectTransform;
        [HideInInspector] public Vector2 originalPos;

        [HideInInspector] public bool isDrawed;
        [HideInInspector] public bool isOut = false;
        [HideInInspector] public bool canDrag = true;
        [HideInInspector] public bool isDragging;
        [HideInInspector] public bool isClicked;

        public void Activate(DrawAreaHandler _drawArea) {
            drawArea = _drawArea;
        }

        private void Awake()
        {
            // LeanTween.scale(traceLine, new Vector3(1.4f, 1, 1), 0.5f).setLoopPingPong();
            originalPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
            startRectTransform = GetComponent<RectTransform>();
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if(!drawArea.isTitik) {
                if (isOut == false && canDrag == true && isClicked == true)
                {
                    isDragging = true;
                    startRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                }
                else if (isOut == true)
                {
                    drawArea.DestroyLine();
                    startRectTransform.anchoredPosition = originalPos;
                    isOut = false;
                    isClicked = false;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isClicked = true;
            
            //LeanTween.pause(traceLine);
            if (isOut == true)
            {
                Debug.Log("Destroy line di pointer down");
                drawArea.DestroyLine();
            }

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if(drawArea.isTitik) {
                Debug.Log("ini titik kak");
                isDrawed = true;
                isOut = false;
                canDrag = false;
            }
            if (isDrawed == false)
            {
                isClicked = false;
                isDragging = false;
                startRectTransform.anchoredPosition = originalPos;
                Debug.Log("Destroy line di pointer up");
                drawArea.DestroyLine();
                drawArea.ResetChecker();
                //LeanTween.resume(traceLine);
            }
            

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.transform == drawArea.traceArea)
            {
                canDrag = true;
            }
            if (col.gameObject.transform == drawArea.endPoint)
            {
                Debug.Log("triggerenterkak");
                isDrawed = true;
                isOut = false;
                canDrag = false;
            }
            

            if(col.GetComponent<DrawAreaHandler>()) {
                bool test = drawArea.IsCorrectCheckPoint(col);
                if(!test) {
                    isClicked = false;
                    isDragging = false;
                    startRectTransform.anchoredPosition = originalPos;
                    Debug.Log("Destroy line di triegger enter");
                    drawArea.DestroyLine();
                }
            }
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject.transform == drawArea.traceArea)
            {
                isOut = true;
            }
        }
        public void ResetBoolVals()
        {
            isDrawed = false;
            isOut = false;
            canDrag = true;
            isDragging = false;
            isClicked = false;
            //LeanTween.resume(traceLine);
        }
    }
}

