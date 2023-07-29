using System;
using System.Collections.Generic;
using UnityEngine;

public class IrmanDraw : MonoBehaviour {

    public Camera mainCamera;
    public GameObject brush;
    public Transform drawParent;
    public Transform drawArea;

    EvaluationMain evalMain;
    public DateTime timeCurrent;
    public List<DataPointTime> datas;


    public void Activate(EvaluationMain _main) {
        evalMain = _main;
    }

    LineRenderer lineRenderer;

    Vector2 lastPos;

    [SerializeField] float MIN_DISTANCE = 1f;

    
    void Draw() {
        Vector3 mPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mPos;
        
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) && onDownBoard)
        {
            timeCurrent=DateTime.Now;
            datas = new List<DataPointTime>();
            CreateBrush();
            isDrawing = true;
        }   
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0)) && onDownBoard && isDrawing)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if(Vector2.Distance(mousePos, lastPos) > MIN_DISTANCE) {
            //if(mousePos != lastPos) {
                AddPoint(mousePos);
                lastPos = mousePos;
            }
        }
        else
        {
            lineRenderer = null;
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0)) {
            isDrawing = false;
            canDraw = false;
        }
    }

    void CreateBrush() {
        Debug.Log("Touch");
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.SetParent(drawParent);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, mousePos);
        lineRenderer.SetPosition(1, mousePos);
        lastPos = mousePos;
        // Debug.Log("BrushCreated");
        Debug.Log(mousePos);
    }

    void AddPoint(Vector2 pointPos) {
        lineRenderer.positionCount++;
        int posIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(posIndex, pointPos);
        //deltaTime posisi x dan y
        //Debug.Log(posIndex);
        //Debug.Log(Time.deltaTime);
        double time = (DateTime.Now - timeCurrent).TotalMilliseconds;
        Debug.Log(time);
        Debug.Log(pointPos);
        datas.Add(new DataPointTime(pointPos.x, pointPos.y, time));
    }

    public void DeleteDrawing() {
        for (int i=0; i < drawParent.childCount; i++)
                Destroy(drawParent.GetChild(i).gameObject);
    }

    private void Update() {
        
        if(evalMain.IsPlaying) Draw();
    }

    private void OnTriggerStay2D(Collider2D col) {
        if(col.transform == drawArea) {
            canDraw = true;
        }
    }

    public void HideDrawing() {
        drawParent.gameObject.SetActive(false);
    }

    public void ShowDrawing() {
        drawParent.gameObject.SetActive(true);
    }

    [SerializeField] bool canDraw = false;
    [SerializeField] bool isDrawing = false;

    private void OnTriggerExit2D(Collider2D col) {
        if(col.transform == drawArea) {
            canDraw = false;
            isDrawing = false;
        }
    }

    public bool onEnterBoard = false;
    public bool onDownBoard = false;
}