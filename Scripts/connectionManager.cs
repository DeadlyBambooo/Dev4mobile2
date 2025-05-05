using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    public LineRenderer lineRen;
    public LayerMask targetLayerMask;
    private Camera _mainCamera;
    private bool _isDrawing;

    public List<GameObject> connectedObjects = new List<GameObject>();
    public List<Vector3> drawPositions = new List<Vector3>();

    public List<GameObject> resultLevel1;
    public bool _levelOneDone = false;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = GetRayOnMousePosition();
            if (Physics.Raycast(ray, out var raycastHit, 100f, targetLayerMask))
            {
                _isDrawing = true;
                connectedObjects.Add(raycastHit.transform.gameObject);
                lineRen.gameObject.SetActive(true);
            }
        }

        if (Input.GetMouseButton(0) && _isDrawing)
        {
            var ray = GetRayOnMousePosition();
            if (Physics.Raycast(ray, out var raycastHit, 200f, targetLayerMask))
            {
                var targetObject = raycastHit.transform.gameObject;

                if (!connectedObjects.Contains(targetObject))
                {
                    connectedObjects.Add(targetObject);
                }
            }

            DrawLine();
        }

        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            _isDrawing = false;
            connectedObjects.Clear();
            DeactivateDrawing();
        }

       if(AreListsEqualInOrder(connectedObjects, resultLevel1))
       {
            _levelOneDone = true;
            Debug.Log("Hello World");
            SceneManager.LoadSceneAsync(2);
        }
       


    }

    private Ray GetRayOnMousePosition()
    {
        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    private void DrawLine()
    {
        drawPositions.Clear();

        if (connectedObjects.Count > 0)
        {
            foreach (var targetObject in connectedObjects)
            {
                drawPositions.Add(targetObject.transform.position);
            }

            var inputDrawPosition = GetMouseWorldInputPosition();
            drawPositions.Add(inputDrawPosition);

            lineRen.positionCount = drawPositions.Count;
            lineRen.SetPositions(drawPositions.ToArray());
        }
    }

   private Vector3 GetMouseWorldInputPosition()
    {
        var targetInputPos = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        return targetInputPos;
    }


    private void DeactivateDrawing()
    {
        lineRen.positionCount = 0;
        drawPositions.Clear();
        lineRen.gameObject.SetActive(false);
    }

    private bool AreListsEqualInOrder(List<GameObject> list1, List<GameObject> list2)
    {
        if (list1.Count != list2.Count) return false;

        for (int i = 0; i < list1.Count; i++)
        {
        if (list1[i] != list2[i]) return false; // Compare order and content
        }

    return true;
    }
}


