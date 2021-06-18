using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField] private GameObject ParentWall;
    [SerializeField] private GameObject ChildWall;
    [SerializeField] private GameObject NewWall;
    [SerializeField] private float maxDistance = 2;
    [SerializeField] private RaycastHit hit;
    [SerializeField] private Vector3[] direction;

    void Start()
    {
        Vector3 buildDist;
        Vector3 baseRot = new Vector3(90, 0, 0);
        direction = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };

        for (int i = 0; i < direction.Length; i++)
        {
            if (Physics.Raycast(ParentWall.transform.position, direction[i], out hit, maxDistance))
            {
                buildDist = ((hit.point - ParentWall.transform.position) / 2) + ParentWall.transform.position;
                NewWall = Instantiate(ChildWall, buildDist, Quaternion.identity);
                NewWall.transform.LookAt(hit.point);
                NewWall.transform.Rotate(baseRot, Space.Self);
            }
        }
    }
}
