using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManagerScript : MonoBehaviour
{
    public GameObject[] Towers;
    public GameObject[] buildGhost;

    public static int buildIndex;
    [SerializeField] private Ray mouseRay;
    public GameObject buildingSpawner;
    public static GameObject TempBuilding;
    [SerializeField] private GameObject RealisedBuilding;

    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask mask;

    [Header("Tower variables")]
    public static int power;
    public static float heat;
    public static float heatMax;


    [Header("Money related stuff")]
    public static int currentCash;
    [SerializeField] private int[] baseBuildingCost;


    [Header("Placing Conditions")]
    public static bool obstructed;
    [SerializeField] private bool placing;
    public static bool finalBuilding;
    public static int passBuildIndex;


    public void Awake()
    {
        currentCash = 150;
        placing = false;
        obstructed = false;
    }

    void Start()
    {
        heatMax = 12;
        finalBuilding = false;
        baseBuildingCost = new int[] { 5, 25, 20, 15 };
    }

    void Update()
    {
        //mouse fires a ray which hits the floor to 
        mouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit, float.MaxValue, mask))
        {
            buildingSpawner.transform.position = new Vector3(Mathf.Round(hit.point.x / 2) * 2, hit.point.y + 0.5f, Mathf.Round(hit.point.z / 2) * 2);

            if(placing)
            {
                TempBuilding.transform.position = buildingSpawner.transform.position;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    placing = false;
                    Destroy(TempBuilding);
                }
            }
        }

        if (Input.GetMouseButton(0) && placing && !obstructed)
        {
             Build(passBuildIndex);
             Destroy(TempBuilding);
        }

    }

    private void FixedUpdate()
    { 

        //base heat dissapation rate
        // later can be augmented with a heat sink tower by making heat = heat - heatDissapation rate instead of - a magic number
        if (heat > 0)
            heat = heat - 0.01f;


        //stops you waiting around for heat to dissapate at the end of a wave
        if(WaveManager.Wave.Count == 0)
        {
            heat = heat - heat;
        }

    }

    public void BuildGhost(int buildIndex)
    {
        finalBuilding = false;
        passBuildIndex = buildIndex;

        if (baseBuildingCost[buildIndex] <= currentCash)
            UiManager.outOfMoney = false;
        else
            UiManager.outOfMoney = true;

        if (!placing) 
        {
            if(UiManager.outOfMoney)
            {
                UiManager.changing = true;
            }

            TempBuilding = Instantiate(buildGhost[passBuildIndex], buildingSpawner.transform);
            placing = true;
        }
    }

    public void Build(int buildIndex)
    {
        finalBuilding = true;
        passBuildIndex = buildIndex;
        UiManager.changing = true;

        if (baseBuildingCost[passBuildIndex] <= currentCash)
        {
            placing = false;

            RealisedBuilding = Instantiate(Towers[buildIndex], buildingSpawner.transform.position, Quaternion.identity);
            currentCash = currentCash - baseBuildingCost[passBuildIndex];

            Ballistic_tower_basic.needsPowerCheck = true;
            WallBuilder.needsWallpower = true;
            HeatSink.needsContact = true;
        }
        else
        {
            placing = false;
        }    
    }
}
