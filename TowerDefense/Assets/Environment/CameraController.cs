using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int magnification;
    [SerializeField] private int maxZoom = 4;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && magnification <= maxZoom)
        {
            magnification++;
        }
    }
}
