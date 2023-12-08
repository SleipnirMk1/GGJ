using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour
{
    bool isDrag;
    Vector2 mouseStartDrag, cameraStartDragPos, panning;

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
    }
}
