using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using startPoints;
using UnityEngine.UI;
using System;

namespace IrmanTraceLine
{
    public class IrmanTraceLineManager : MonoBehaviour
    {
        [SerializeField] private IrmanDrawAreaHandler[] drawParts;
        private SpriteRenderer filledLetter;
        private Transform drawLineParent;

        // private float animTime = 0.6f;
        private int index = 0;

        private bool isLetterComplete = false;

        #region
        public GameObject linePrefab;

        private List<GameObject> createdLines;
        private GameObject currentLine;
        private LineRenderer curLineRenderer;
        private List<Vector2> fingerPositions;
        #endregion

        public Action onCompleteLetter;

        private void Awake()
        {
            filledLetter = transform.Find("LetterFill").GetComponent<SpriteRenderer>();
            filledLetter.enabled = false;
            drawLineParent = transform.Find("LineParent");
            
            InitDrawParts();
            DeactivateDrawParts();

            if(createdLines==null) createdLines = new List<GameObject>();
            if(fingerPositions==null) fingerPositions = new List<Vector2>();
        }

        void Start() {
            drawParts[0].gameObject.SetActive(true);
        }

        void InitDrawParts() {
            Transform letterPart = transform.Find("Letter");
            int partCount = letterPart.childCount;
            drawParts = new IrmanDrawAreaHandler[partCount];
            for (int i = 0; i < partCount; i++) {
                drawParts[i] = letterPart.GetChild(i).GetComponent<IrmanDrawAreaHandler>();
                drawParts[i].Activate(this);
                drawParts[i].Init();
            }
        }
        void DeactivateDrawParts() {
            foreach (IrmanDrawAreaHandler drawPart in drawParts) {
                drawPart.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (isLetterComplete == false)
            {
                if (drawParts[index].startPoint.isDrawed == false)
                {
                    if (Input.GetMouseButtonDown(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isClicked == true)
                    {
                        Debug.Log("Creating Line...");
                        CreateLine();
                    }

                    if (Input.GetMouseButton(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isDragging == true)
                    {
                        Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        drawParts[index].startPoint.transform.position = tempFingerPos;
                        if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                        {
                            UpdateLine(tempFingerPos);
                        }
                    }
                }
                else
                {
                    if(index < drawParts.Length - 1) {
                        SwitchDrawPart(index);
                        index++;
                    }
                    else {
                        OnLetterComplete();
                    }
                }
            }
        }

        void SwitchDrawPart(int index)
        {
            drawParts[index].gameObject.SetActive(false);
            drawParts[index + 1].gameObject.SetActive(true);
            SoundController.GetInstance().PlayCorrect();
        }

        void OnLetterComplete() {
            DeactivateDrawParts();
            SoundController.GetInstance().PlayCorrect();
            isLetterComplete = true;
            filledLetter.enabled = true;
            DestoyLines();
            if(onCompleteLetter!=null) onCompleteLetter.Invoke();
        }
        public void ResetActivity()
        {
            index = 0;
            for (int j = 0; j < drawParts.Length; j++)
            {
                drawParts[j].gameObject.SetActive(false);
                drawParts[j].startPoint.transform.position = drawParts[j].startPoint.originalPos;
                drawParts[j].startPoint.ResetBoolVals();
            }
            drawParts[0].gameObject.SetActive(true);
            DestoyLines();
            filledLetter.enabled = false;
            isLetterComplete = false;

        }
        //drawing functions

        #region
        void CreateLine()
        {
            currentLine = Instantiate(linePrefab, new Vector3(0, 0, 0), Quaternion.identity, drawLineParent.transform);
            createdLines.Add(currentLine);
            curLineRenderer = currentLine.GetComponent<LineRenderer>();
            fingerPositions.Clear();
            fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            curLineRenderer.SetPosition(0, fingerPositions[0]);
            curLineRenderer.SetPosition(1, fingerPositions[1]);
        }

        void UpdateLine(Vector2 newFingerPos)
        {
            if (curLineRenderer != null)
            {
                fingerPositions.Add(newFingerPos);
                curLineRenderer.positionCount++;
                curLineRenderer.SetPosition(curLineRenderer.positionCount - 1, newFingerPos);
            }
        }
        void DestoyLines()
        {
            foreach (GameObject obj in createdLines)
            {
                Destroy(obj);
            }
        }
        public void DestoyLine()
        {
            if(currentLine!=null) {
                Debug.Log("destroying line...");
                Destroy(currentLine);
            }
            else {
                Debug.Log("mau destroy line tapi line nya sudah null");
            }
        }
        #endregion
    }

}