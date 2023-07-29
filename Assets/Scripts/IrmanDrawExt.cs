using UnityEngine;

public class IrmanDrawExt : MonoBehaviour {

    public Camera mainCamera;
    public GameObject brush;
    public Transform drawParent;

    LineRenderer lineRenderer;

    Vector2 lastPos;

    [SerializeField] float MIN_DISTANCE = 1f;

    bool isDrawable = false;

    void Draw() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            if(isDrawable) {
                CreateBrush();
            }
        }   
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            if(isDrawable) {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if(Vector2.Distance(mousePos, lastPos) > MIN_DISTANCE) {
                //if(mousePos != lastPos) {
                    AddPoint(mousePos);
                    lastPos = mousePos;
                }
            }
        }
        else
        {
            lineRenderer = null;
        }
    }

    void CreateBrush() {
        GameObject brushInstance = Instantiate(brush);
        brushInstance.transform.SetParent(drawParent);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, mousePos);
        lineRenderer.SetPosition(1, mousePos);
        // Debug.Log("BrushCreated");
    }

    void AddPoint(Vector2 pointPos) {
        lineRenderer.positionCount++;
        int posIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(posIndex, pointPos);
        // Debug.Log("AddPoint");
    }

    public void DeleteDrawing() {
        for (int i=0; i < drawParent.childCount; i++)
                Destroy(drawParent.GetChild(i).gameObject);
    }

    private void Update() {
        Draw();
    }
}