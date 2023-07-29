using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using IrmanTraceLine;
using System;

namespace IrmanTraceLine
{
    public class IrmanStartPointHandler : MonoBehaviour
    {
        private IrmanDrawAreaHandler drawArea;

        [HideInInspector] public Vector2 originalPos;

        public bool isDrawed;
        public bool isOut = false;
        public bool canDrag = true;
        public bool isDragging;
        public bool isClicked;

        public void Activate(IrmanDrawAreaHandler _drawArea) {
            drawArea = _drawArea;
        }

        private void Awake()
        {
            // LeanTween.scale(traceLine, new Vector3(1.4f, 1, 1), 0.5f).setLoopPingPong();
            originalPos = transform.position;
        }
        public void OnMouseDrag()
        {
            if(isAut) return;
            if(!drawArea.isTitik) {
                if (isOut == false && canDrag == true && isClicked == true)
                {
                    isDragging = true;
                    transform.position += Vector3.zero;
                }
                else if (isOut == true)
                {
                    drawArea.DestroyLine();
                    transform.position = originalPos;
                    isClicked = false;
                    isAut=true;
                }
            }
        }
        bool isAut=false;

        void OnMouseDown()
        {
            Debug.Log("On mouse down");
            isClicked = true;
            
            //LeanTween.pause(traceLine);
            if (isOut == true)
            {
                Debug.Log("Destroy line di pointer down");
                drawArea.DestroyLine();
            }

        }
        public void OnMouseUp()
        {
            isAut=false;
            if(isOut==true) {
                Debug.Log("mouse up pas udah keluar");
                ScoreLatihanManager.GetInstance().AddError(); //onError?.Invoke();
            }
            if(drawArea.isTitik) {
                Debug.Log("ini titik kak");
                isDrawed = true;
                isOut = false;
                canDrag = false;
            }
            if (isDrawed == false)
            {
                isOut = false;
                isClicked = false;
                isDragging = false;
                transform.position = originalPos;
                Debug.Log("Destroy line di pointer up");
                drawArea.DestroyLine();
                drawArea.ResetChecker();
                //LeanTween.resume(traceLine);
            }
            

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if(!drawArea.isTitik) {
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
                

                if(col.GetComponent<IrmanDrawAreaHandler>()) {
                    bool test = drawArea.IsCorrectCheckPoint(col);
                    if(!test) {
                        isClicked = false;
                        isDragging = false;
                        transform.position = originalPos;
                        Debug.Log("Destroy line di triegger enter");
                        drawArea.DestroyLine();
                        ScoreLatihanManager.GetInstance().AddError();//onError?.Invoke();
                    }
                }
            }
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if(!drawArea.isTitik) {
                if (c.gameObject.transform == drawArea.traceArea)
                {
                    isOut = true;
                }
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

        private void OnEnable() {
            ResetBoolVals();
        }
    }
}

