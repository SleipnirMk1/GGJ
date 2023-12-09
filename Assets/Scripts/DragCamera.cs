using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour
{
    bool isDrag;
    Camera camera;
    Vector2 mouseStartDrag, cameraStartDragPos, panning;
    [SerializeField] Vector2 limitX, limitY;
    float scrollVal;

    void Start(){
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera panning
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            mouseStartDrag = Input.mousePosition;
            cameraStartDragPos = transform.position;
            isDrag = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            isDrag = false;
        }
        if (isDrag)
        {
            transform.position = (Vector3)cameraStartDragPos + (Vector3)((mouseStartDrag - (Vector2)Input.mousePosition) / 100) + Vector3.back * 10;
        }

        // WASD panning
        if (Input.GetKeyDown(KeyCode.W))
        {
            panning += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            panning += Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            panning += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            panning += Vector2.right;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            panning -= Vector2.up;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            panning -= Vector2.left;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            panning -= Vector2.down;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            panning -= Vector2.right;
        }
        transform.position += (Vector3)panning * 10 * Time.deltaTime;

        // Set camera bound
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,limitX.x,limitX.y),Mathf.Clamp(transform.position.y,limitY.x,limitY.y),-10);

        // Zoom in-out
        scrollVal += Input.mouseScrollDelta.y;
        scrollVal = Mathf.Clamp(scrollVal,5,10);
        camera.orthographicSize = scrollVal;
        //camera.orthographicSize = Mathf.SmoothStep(5,10,scrollVal);
    }
}
