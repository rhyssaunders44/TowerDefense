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
    [SerializeField] private GameObject buildAura;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask powerMask;
    [SerializeField] private GameObject powerBox;
    public static bool needsWallpower;

    void Start()
    {
        Vector3 buildDist;
        Vector3 baseRot = new Vector3(90, 0, 0);
        direction = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };

        for (int i = 0; i < direction.Length; i++)
        {
            if (Physics.Raycast(transform.position, direction[i], out hit, maxDistance, mask))
            {
                buildDist = ParentWall.transform.position + direction[i];
                NewWall = Instantiate(ChildWall, buildDist , Quaternion.identity, ParentWall.transform);
                NewWall.transform.LookAt(hit.point);
                NewWall.transform.Rotate(baseRot, Space.Self);
            }

        }

        needsWallpower = true;
        WallPowerCheck();
    }

    public void Update()
    {
        if (needsWallpower)
        {
            WallPowerCheck();
        }
    }

    private void WallPowerCheck()
    {
        direction = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };

        for (int i = 0; i < direction.Length; i++)
        {
            if (Physics.Raycast(transform.position, direction[i], out RaycastHit powerHit, 2, powerMask))
            {
                powerBox.SetActive(true);
                //WallPowerCheck();
            }
        }
        needsWallpower = false;
    }
}

