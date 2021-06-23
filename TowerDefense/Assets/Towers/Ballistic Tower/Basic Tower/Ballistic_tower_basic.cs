using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistic_tower_basic : MonoBehaviour
{
    [SerializeField] private int targetID;
    [SerializeField] private GameObject BulletSpawner;
    [SerializeField] private GameObject TurretBody;
    [SerializeField] private GameObject Barrel;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject TurretOperator;
    [SerializeField] private SphereCollider inRangeTrigger;
    [SerializeField] private Collider Floor;
    [SerializeField] private GameObject mouseSelectEnabler;


    [SerializeField] private int powerCost;
    public static bool powered;
    [SerializeField] private Light powerLight;
    [SerializeField] private LayerMask powerMask;
    [SerializeField] private GameObject PowerBox;
    [SerializeField] private LayerMask mask;
    public static bool needsPowerCheck;
    [SerializeField] private int upgradeTeir;

    [SerializeField] private float rateOfFire;
    [SerializeField] private bool targetAquired;
    [SerializeField] private bool reloading;
    [SerializeField] private float reloadStart;
    [SerializeField] private float reloadEnd;
    [SerializeField] private float distToTarget;
    [SerializeField] private float range;
    public Transform targetPos;
    public static int damage;
    public static List<GameObject> targetOptions;

    void Start()
    {
        //KNOWN ISSUES
        //if one tower is unpowered all will not fire
        //an out of range exception if a target dies while another turret is targeting it

        targetOptions = new List<GameObject>();
        powered = false;
        Physics.IgnoreLayerCollision(2 , 9);
        rateOfFire = 1;
        damage = 1;
        powerCost = 5;
        upgradeTeir = 1;
        targetAquired = false;
        range = 10;

        needsPowerCheck = true;
        powerCheck();
    }

    public void Update()
    {
        //an attempt to get the power system to work
        
        if (needsPowerCheck)
        {
            powerCheck();
            if (powered)
            {
                powerLight.color = Color.green;
                PowerBox.SetActive(true);
            }
            
            if(!powered)
            {
                powerLight.color = Color.red;
            }
        }
        
        //dont bother running the following garbage if there are no potential targets
        if (targetOptions.Count == 0)
            return;

        TargetAquisition(0);

        //checks if the target is in range
        targetPos = targetOptions[targetID].transform;
        distToTarget = Vector3.Distance(targetOptions[targetID].transform.position, TurretBody.transform.position);
        TurretOperator.transform.LookAt(targetPos);

        if (distToTarget <= range + 2 && powered)
        {
            //my attempt at getting a turret yaw action
            float yAngle = TurretOperator.transform.localEulerAngles.y - TurretBody.transform.localEulerAngles.y;
            float xAngle = TurretOperator.transform.localEulerAngles.x - Barrel.transform.localEulerAngles.x;

            //seems to only rotate from 0-360 degrees and will not take shortest route to look at target if it has to go through 0

            //magic speed of rotation cap
            if (yAngle > 1f)
                yAngle = 1f;

            //rotate the rotatableparts based on the above
            TurretBody.transform.Rotate(Vector3.up, yAngle);
            Barrel.transform.Rotate(Vector3.right, xAngle);
            
           //if its looking at the target, fire
           if(Physics.Raycast(BulletSpawner.transform.position, BulletSpawner.transform.forward, out RaycastHit hit, range, mask))
           {
                Fire(upgradeTeir);
                targetAquired = true;
           }
        }

        //if the target is out of range, go to the next target
        // theoretically the target should have been removed from the list, and then cleaned up by target aquisition
        // that does not seem to be the case, but target the next target in the target list
        if (distToTarget > range)
        {
            targetAquired = false;
            targetID++;
            if(targetID > targetOptions.Count)
            {
                targetID = 0;
            }
        }
    }

    //removes all dead items from the list of potential target
    //since it runs every frame (ew) it only cares if there are potential targets
    public void TargetAquisition(int sortIndex)
    {
        if(targetOptions.Count != 0)
        {
            targetOptions.RemoveAll(GameObject => GameObject == null);
        }

        targetID = sortIndex;

    }

    //build based on the idea that bullets could do different damages based on upgrade level
    private void Fire(int bulletDamage)
    {
        bulletDamage = damage;

        //if the heat bar is capped out the turret cannot fire
        if (!reloading && targetAquired && (BuildManagerScript.heat < BuildManagerScript.heatMax) && powered)
        {
            Instantiate(Bullet, BulletSpawner.transform.position, BulletSpawner.transform.rotation);
            BuildManagerScript.heat = BuildManagerScript.heat + damage;
            reloadStart = Time.time;
            reloadEnd = reloadStart + rateOfFire;
        }

        //since this is called every frame it has to check if it is allowed to fire every frame (which is gross)
        if(Time.time < reloadEnd)
        {
            reloading = true;
        }

        if (Time.time >= reloadEnd)
        {
            reloading = false;
        }
    }
    /// <summary>
    /// checks if the building is powered by firing a ray in each direction, if it hits a powerbox, the building is powered
    /// </summary>
    public void powerCheck()
    {
        Vector3[] direction = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };

        for (int i = 0; i < direction.Length; i++)
        {
            if (Physics.Raycast(mouseSelectEnabler.transform.position, direction[i], out RaycastHit hit, 2, powerMask) && (hit.collider.gameObject.tag == "PowerBox"))
            {
                powered = true;
                powerLight.color = Color.green;
                PowerBox.SetActive(true);
            }
        }
        needsPowerCheck = false;
    }


    //     this section is unimplemented due to time constraints

    //    public void Upgrade(int upgradecost)
    //    {
    //        int x = 1;

    //        if (BuildManagerScript.currentCash <= upgradecost)
    //        {
    //            range = range + x;
    //            damage = damage + x;
    //            powerCost = powerCost + x;
    //            BuildManagerScript.currentCash = BuildManagerScript.currentCash - upgradecost;
    //        }
    //        else
    //        {
    //            //no
    //        }
    //    }
}
