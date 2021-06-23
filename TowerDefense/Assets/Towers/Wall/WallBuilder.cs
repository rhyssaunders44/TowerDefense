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

        //fires a ray in the nsew directions, if it hits a wall, build a wallpiece
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


    //i wanted it to update so you could build the buildings in any order and connect a generator and realise it would be powered,
    // but that was getting too intensive since it would be recursive and without and exit for the recursion it'll blow up
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

