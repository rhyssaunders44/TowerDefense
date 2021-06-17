using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManagerScript : MonoBehaviour
{
    public GameObject[] Towers;
    public GameObject[] buildGhost;
    public int buildIndex;
    Ray mouseRay;
    public GameObject buildingSpawner;
    public GameObject currentBuilding;
    public Material matCol;
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask mask;

    [Header("Money related stuff")]
    public static int currentCash;
    public int[] baseBuildingCost;


    [Header("Placing Conditions")]
    bool obstructed;
    public static bool placing;

    public void Awake()
    {
        currentCash = 100;
        placing = false;
        obstructed = false;
    }

    void Start()
    {
        baseBuildingCost = new int[] { 5, 25 };
    }

    void Update()
    {
        //spawning object follows mouse
        mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit, float.MaxValue, mask))
        {
            buildingSpawner.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
        }

        if(placing == true)
        {
            if (Physics.Raycast(mouseRay, out hit, float.MaxValue, mask))
            {
                currentBuilding.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            }
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

        if(Input.GetMouseButton(0) && placing == true && obstructed == false)
        {
            Build(buildIndex);
            Destroy(currentBuilding);
        }
    }

    public void BuildGhost(int buildIndex)
    {
        currentBuilding = Instantiate(buildGhost[buildIndex], buildingSpawner.transform);

        //set alpha of material to "ghost"
        matCol = currentBuilding.GetComponent<Material>();
        placing = true;
    }

    public void Build(int buildIndex)
    {
        if(baseBuildingCost[buildIndex] <= currentCash)
        {
            Instantiate(Towers[buildIndex], buildingSpawner.transform.position, Quaternion.identity);
            placing = false;
            currentCash = currentCash - baseBuildingCost[buildIndex];
        }
        else
        {
            //you dont have enough money to buy this
        }    
    }

    public int BuildSelect(int buildSelectIndex)
    {
        buildIndex = buildSelectIndex;
        return buildIndex;
    }
}
