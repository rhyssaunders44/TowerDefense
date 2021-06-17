using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManagerScript : MonoBehaviour
{
    public GameObject[] Towers;
    public GameObject[] buildGhost;
    public int buildIndex;
    Vector2 mouseFollow;
    public GameObject buildingSpawner;
    public GameObject currentBuilding;
    public Material matCol;

    [Header("Money related stuff")]
    public static int currentCash;
    public int[] baseBuildingCost;


    [Header("Placing Conditions")]
    bool obstructed;
    bool placing;


    void Start()
    {
        placing = false;
    }

    void Update()
    {
        //spawning object follows mouse
        mouseFollow = new Vector3(Input.mousePosition.x, 1, Input.mousePosition.z);
        buildingSpawner.transform.position = mouseFollow;

        if(placing == true)
        {

            currentBuilding.transform.position = mouseFollow;
            if(obstructed == true)
            {
                //turn red
            }
            else
            {
                //turn Green
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && placing == true)
        {
            Destroy(currentBuilding);
            placing = false;
        }
    }

    public void BuildGhost(int buildIndex)
    {
        currentBuilding =  Instantiate(buildGhost[buildIndex], buildingSpawner.transform);
        matCol = currentBuilding.GetComponent<Material>();
        //set alpha of material to "ghost"
        placing = true;
    }

    public void Build(int buildIndex)
    {
        if(baseBuildingCost[buildIndex] <= currentCash)
        Instantiate(Towers[buildIndex], buildGhost[buildIndex].transform.position, Quaternion.identity);
        
    }

    public void BuildSelect(int buildSelectIndex)
    {
        buildIndex = buildSelectIndex;
    }
}
