using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour
{
    bool isDrag;
    Vector2 mouseStartDrag, cameraStartDragPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)){
            mouseStartDrag = Input.mousePosition;
            cameraStartDragPos = transform.position;
            isDrag = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse2)){
            isDrag = false;
        }
        if (isDrag){
            transform.position = (Vector3)cameraStartDragPos + (Vector3)((mouseStartDrag - (Vector2)Input.mousePosition)/100) + Vector3.back*10;
        }
    }
}
