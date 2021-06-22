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
    [SerializeField] private Collider inRangeTrigger;
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
        targetOptions = new List<GameObject>();
        powered = false;
        Physics.IgnoreLayerCollision(2 , 9);
        rateOfFire = 1;
        range = 15f;
        damage = 1;
        powerCost = 5;
        upgradeTeir = 1;
        targetAquired = false;

        needsPowerCheck = true;
        powerCheck();
    }

    public void Update()
    {
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

        if (targetOptions.Count == 0)
            return;

        TargetAquisition(0);
        targetPos = targetOptions[targetID].transform;
        distToTarget = Vector3.Distance(targetOptions[targetID].transform.position, TurretBody.transform.position);
        TurretOperator.transform.LookAt(targetPos);

        if (distToTarget <= range + 2)
        {
            float yAngle = TurretOperator.transform.localEulerAngles.y - TurretBody.transform.localEulerAngles.y;
            float xAngle = TurretOperator.transform.localEulerAngles.x - Barrel.transform.localEulerAngles.x;

            if (yAngle > 1f)
                yAngle = 1f;

            TurretBody.transform.Rotate(Vector3.up, yAngle);
            Barrel.transform.Rotate(Vector3.right, xAngle);
            
           if(Physics.Raycast(BulletSpawner.transform.position, BulletSpawner.transform.forward, out RaycastHit hit, range, mask))
           {
                Fire(upgradeTeir);
                targetAquired = true;
           }
        }

        if (distToTarget > range)
        {
            targetAquired = false;
        }
    }

    public void TargetAquisition(int sortIndex)
    {
        if(targetOptions.Count != 0)
        {
            targetOptions.RemoveAll(GameObject => GameObject == null);
        }

        targetID = 0;
    }

    private void Fire(int bulletDamage)
    {
        bulletDamage = damage;

        if (!reloading && targetAquired && (BuildManagerScript.heat < BuildManagerScript.heatMax) && powered)
        {
            Instantiate(Bullet, BulletSpawner.transform.position, BulletSpawner.transform.rotation);
            BuildManagerScript.heat = BuildManagerScript.heat + damage;
            reloadStart = Time.time;
            reloadEnd = reloadStart + rateOfFire;
        }

        if(Time.time < reloadEnd)
        {
            reloading = true;
        }

        if (Time.time >= reloadEnd)
        {
            reloading = false;
        }
    }

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

    public void Upgrade(int upgradecost)
    {
        int x = 1;

        if (BuildManagerScript.currentCash <= upgradecost)
        {
            range = range + x;
            damage = damage + x;
            powerCost = powerCost + x;
            BuildManagerScript.currentCash = BuildManagerScript.currentCash - upgradecost;
        }
        else
        {
            //no
        }
    }
}
