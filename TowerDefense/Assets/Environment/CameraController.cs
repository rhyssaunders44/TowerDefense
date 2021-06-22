using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject CameraControllerBody;
    [SerializeField] private int magnification;
    [SerializeField] private int maxZoom = 4;
    [SerializeField] private Vector3 rotate;
    [SerializeField] private int rotationIndex;
    [SerializeField] private float setTime;
    [SerializeField] private float startRot;
    [SerializeField] private float endRot;

    private void Start()
    {
        rotationIndex = 0;
        rotate = new Vector3(0, 45 * rotationIndex, 0);
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && magnification <= maxZoom)
        {
            magnification++;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateTimeSet();
            Mathf.LerpAngle(45 * rotationIndex, 45 * rotationIndex + 1, (Time.time - setTime) / 2 );
            rotationIndex++;
            CameraControllerBody.transform.Rotate(CameraControllerBody.transform.position, Space.World);
        }
    }

    void RotateTimeSet()
    {
        setTime = Time.time;
    }
}
