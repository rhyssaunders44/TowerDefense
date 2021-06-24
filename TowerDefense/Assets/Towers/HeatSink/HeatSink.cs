using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSink : MonoBehaviour
{
    [SerializeField] private Material heatMaterial;
    [SerializeField] private Vector3[] direction;
    [SerializeField] private GameObject powerBox;
    public static bool needsContact;
    [SerializeField] private LayerMask powerMask;
    void Start()
    {
        needsContact = true;
        BuildManagerScript.heatMax = BuildManagerScript.heatMax + 2;
    }

    private void Update()
    {
        if (needsContact)
        {
            WallPowerCheck();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BuildManagerScript.heat > 0)
        BuildManagerScript.heat = BuildManagerScript.heat - 0.01f;
    }

    private void WallPowerCheck()
    {
        direction = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };

        for (int i = 0; i < direction.Length; i++)
        {
            if (Physics.Raycast(transform.position, direction[i], out RaycastHit powerHit, 2, powerMask))
            {
                powerBox.SetActive(true);
            }
        }
        needsContact = false;
    }
}
