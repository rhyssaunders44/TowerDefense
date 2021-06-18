using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManagerScript : MonoBehaviour
{
    public GameObject[] Towers;
    public GameObject[] buildGhost;
    public int buildIndex;
    [SerializeField] private Ray mouseRay;
    public GameObject buildingSpawner;
    public GameObject currentBuilding;
    public Material matCol;
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask mask;

    [Header("Money related stuff")]
    public static int currentCash;
    [SerializeField] private int[] baseBuildingCost;


    [Header("Placing Conditions")]
    private bool obstructed;
    public static bool placing;

    [Header("Tilemap")]
    [SerializeField] private Tilemap buildGrid;

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
            // to fixed to tilemap
            buildingSpawner.transform.position = new Vector3(Mathf.Round(hit.point.x), hit.point.y + 1, Mathf.Round(hit.point.z));
        }

        if(placing == true)
        {
            if (Physics.Raycast(mouseRay, out hit, float.MaxValue, mask))
            {
                currentBuilding.transform.position = new Vector3(Mathf.Round(hit.point.x), hit.point.y + 1, Mathf.Round(hit.point.z));
            }

            HitCheck(hit);
        }
        else

        if (Input.GetKeyDown(KeyCode.Escape) && placing == true)
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

    private void HitCheck(RaycastHit hit)
    {
        if (buildIndex == 0 && (hit.collider.tag == "Wall" || hit.collider.tag == "Building"))
        {
            obstructed = true;
        }
        else
        {
            obstructed = false;
        }
    }

    public void BuildGhost(int buildIndex)
    {
        if (placing == false) 
        {
            currentBuilding = Instantiate(buildGhost[buildIndex], buildingSpawner.transform);

            //set alpha of material to "ghost"
            //draw a range based on the tower's range
            matCol = currentBuilding.GetComponent<Material>();
            placing = true;
        }

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
