using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerWorldSpaceCanvas : MonoBehaviour
{    void Update()
    {
        Camera mainCamera = Camera.main;
        transform.LookAt(transform.position + mainCamera.transform.rotation * -Vector3.back, mainCamera.transform.rotation * Vector3.up);
    }
}
