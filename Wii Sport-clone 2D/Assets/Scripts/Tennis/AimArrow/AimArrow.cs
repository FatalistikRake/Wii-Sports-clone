using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    public static Vector3 MousePosition { get; private set; }

    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y));
        transform.LookAt(MousePosition, Vector3.forward);
    }
}
