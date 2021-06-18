using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField] private GameObject ParentWall;
    [SerializeField] private GameObject ChildWall;
    [SerializeField] private float maxDistance = 7;
    [SerializeField] private Vector3 buildDirection;
    [SerializeField] private int rayCount = 8;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Ray guideRay;
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            buildDirection = new Vector3(0, 0, 0);
            guideRay = new Ray(ParentWall.transform.position, buildDirection);

            if(Physics.Raycast(guideRay, out RaycastHit hit, maxDistance))
            {
                Vector3 buildPoint = guideRay.GetPoint((ParentWall.transform.position.x + hit.point.x) / 2);

                Instantiate(ChildWall, buildPoint, Quaternion.LookRotation(hit.point));
            }
        }
    }
}
