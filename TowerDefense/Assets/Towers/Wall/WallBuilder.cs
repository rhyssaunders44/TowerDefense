using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField] private GameObject ParentWall;
    [SerializeField] private GameObject ChildWall;
    [SerializeField] private float maxDistance = 7;
    [SerializeField] private Vector3[] buildDirection;
    [SerializeField] private int rayCount = 8;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Ray guideRay;
    void Start()
    {
        GameObject temp = new GameObject();
        temp.transform.parent = transform;

        buildDirection = new Vector3[rayCount];

        for (int i = 0; i < buildDirection.Length; i++)
        {
            temp.transform.rotation = Quaternion.AngleAxis((360 / rayCount) * i, Vector3.up);
            buildDirection[i] = temp.transform.forward;

            guideRay = new Ray(transform.position, buildDirection[i]);

            if (Physics.Raycast(guideRay, out RaycastHit hit, maxDistance))
            {
                Quaternion hitDirection = new Quaternion(360 / rayCount * i, 360 / rayCount * i, 0, 0);

                Instantiate(ChildWall, ParentWall.transform.position, hitDirection);
            }
        }
        Destroy(temp);
    }
}
