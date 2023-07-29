using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using startPoints;
using UnityEngine.UI;

namespace TraceLineManagerName
{
    public class TraceLineManager : MonoBehaviour
    {
        [SerializeField] private DrawAreaHandler[] drawParts;
        private Image filledLetterImg;
        private Transform drawLineParent;

        public AudioSource audioData;

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

        private void Awake()
        {
            filledLetterImg = transform.Find("LetterFill").GetComponent<Image>();
            filledLetterImg.enabled = false;
            drawLineParent = transform.Find("LineParent");
            Transform letterPart = transform.Find("Letter");
            int partCount = letterPart.childCount;

            drawParts = new DrawAreaHandler[partCount];
            for (int i = 0; i < partCount; i++) {
                drawParts[i] = letterPart.GetChild(i).GetComponent<DrawAreaHandler>();
                drawParts[i].Activate(this);
            }

            foreach (DrawAreaHandler drawPart in drawParts) {
                drawPart.gameObject.SetActive(false);
            }
            drawParts[0].gameObject.SetActive(true);

            if(createdLines==null) createdLines = new List<GameObject>();
            if(fingerPositions==null) fingerPositions = new List<Vector2>();
        }
        void Update()
        {
            if (isLetterComplete == false)
            {
                if (index != drawParts.Length - 1)
                {
                    if (drawParts[index].startPoint.isDrawed == false)
                    {
                        if (Input.GetMouseButtonDown(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isClicked == true)
                        {
                            CreateLine();
                        }

                        if (Input.GetMouseButton(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isDragging == true)
                        {
                            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                            {
                                UpdateLine(tempFingerPos);
                            }
                        }
                    }
                    else
                    {

                        SwitchDrawPart(index);
                        index++;
                    }
                }
                else if (index == drawParts.Length - 1)
                {
                    if (drawParts[index].startPoint.isDrawed == false)
                    {
                        if (Input.GetMouseButtonDown(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isClicked == true)
                        {
                            CreateLine();
                        }

                        if (Input.GetMouseButton(0) && drawParts[index].startPoint.canDrag == true && drawParts[index].startPoint.isOut == false && drawParts[index].startPoint.isDragging == true)
                        {
                            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                            {
                                UpdateLine(tempFingerPos);
                            }
                        }
                    }
                    else
                    {
                        audioData.Play();
                        isLetterComplete = true;
                        filledLetterImg.enabled = true;
                        DestoyLines();
                    }
                }

            }

        }
        public void SwitchDrawPart(int index)
        {

            drawParts[index].gameObject.SetActive(false);
            drawParts[index + 1].gameObject.SetActive(true);
        }
        public void ResetActivity()
        {
            index = 0;
            for (int j = 0; j < drawParts.Length; j++)
            {
                drawParts[j].gameObject.SetActive(false);
                drawParts[j].startPoint.startRectTransform.anchoredPosition = drawParts[j].startPoint.originalPos;
                drawParts[j].startPoint.ResetBoolVals();
            }
            drawParts[0].gameObject.SetActive(true);
            DestoyLines();
            filledLetterImg.enabled = false;
            isLetterComplete = false;

        }
        //drawing functions

        #region
        public void CreateLine()
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

        public void UpdateLine(Vector2 newFingerPos)
        {
            if (curLineRenderer != null)
            {
                fingerPositions.Add(newFingerPos);
                curLineRenderer.positionCount++;
                curLineRenderer.SetPosition(curLineRenderer.positionCount - 1, newFingerPos);
            }
        }
        public void DestoyLines()
        {
            foreach (GameObject obj in createdLines)
            {
                Destroy(obj);
            }
        }
        public void DestoyLine()
        {
            Debug.Log("destroying line...");
            Destroy(currentLine);
        }
        #endregion
    }

}