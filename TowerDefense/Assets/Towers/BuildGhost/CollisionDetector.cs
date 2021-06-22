using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private Renderer ghostRenderer;
    [SerializeField] private Material[] ghostCol;
    [SerializeField] private Collider[] Building;
    [SerializeField] private Collider Ghost;
    [SerializeField] private Collider Bubble;
    [SerializeField] private GameObject selectEnabler;
    [SerializeField] private int buildLevel;
    [SerializeField] private bool locked;
    [SerializeField] private bool foundationBuilding;
    [SerializeField] public int passBuildingIndex;

    public void Start()
    {
        SetBuildIndex();

        if (BuildManagerScript.passBuildIndex != 0)
            Bubble = BuildManagerScript.TempBuilding.GetComponentInChildren<SphereCollider>();

        if (BuildManagerScript.finalBuilding)
        {
            locked = true;
            ghostRenderer.material = ghostCol[2];
        }

        if(!BuildManagerScript.finalBuilding)
        {
            selectEnabler.SetActive(false);
            locked = false;

            if (foundationBuilding && !UiManager.outOfMoney)
            {
                ghostRenderer.material = ghostCol[0];
                BuildManagerScript.obstructed = false;
            }
            if(!foundationBuilding || UiManager.outOfMoney)
            {
                ghostRenderer.material = ghostCol[1];
                BuildManagerScript.obstructed = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!locked && (collision.gameObject.tag == "Aura" || collision.gameObject.tag == "turretAura"))
        {
            collisionEnter(1);
        }

        if (!locked && collision.gameObject.tag == "Aura" && collision.gameObject.tag == "turretAura")
        {
            collisionEnter(2);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!locked && (collision.gameObject.tag == "Aura" || collision.gameObject.tag == "turretAura"))
        {
            collisionExit(1);
        }

        if (!locked && collision.gameObject.tag == "Aura" && collision.gameObject.tag == "turretAura")
        {
            collisionExit(2);
        }
    }

    public void SetBuildIndex()
    {
        passBuildingIndex = BuildManagerScript.passBuildIndex;

        if(passBuildingIndex == 0)
        {
            foundationBuilding = true;
        }
        else
        {
            foundationBuilding = false;
        }
    }

    public void collisionExit(int collisionInt)
    {
        buildLevel -= collisionInt;

        if (foundationBuilding && buildLevel == 0 && !UiManager.outOfMoney)
        {
            ghostRenderer.material = ghostCol[0];
            BuildManagerScript.obstructed = false;
        }

        if (!foundationBuilding && buildLevel != 1 || UiManager.outOfMoney)
        {
            ghostRenderer.material = ghostCol[1];
            BuildManagerScript.obstructed = true;
        }
    }

    public void collisionEnter(int collisionInt)
    {
        buildLevel += collisionInt;

        if (foundationBuilding && buildLevel != 0 || UiManager.outOfMoney)
        {
            ghostRenderer.material = ghostCol[1];
            BuildManagerScript.obstructed = true;
        }

        if (!foundationBuilding && buildLevel == 1 && !UiManager.outOfMoney)
        {
            ghostRenderer.material = ghostCol[0];
            BuildManagerScript.obstructed = false;
        }
    }
}
