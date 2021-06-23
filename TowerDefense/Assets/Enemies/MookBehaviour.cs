using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MookBehaviour : MonoBehaviour
{
    [SerializeField] public Transform objective;
    [SerializeField] public float hitPoints;
    [SerializeField] private float maxHitpoints;

    private void Start()
    {
        maxHitpoints = 3;
        hitPoints = maxHitpoints;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = objective.transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Objective")
        {
            UiManager.baseHp = UiManager.baseHp - 1;
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Bullet")
        {
            TakeDamage(Ballistic_tower_basic.damage);
            Destroy(collision.collider.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints = hitPoints - damage;

        if (hitPoints <= 0)
        {
            BuildManagerScript.currentCash = BuildManagerScript.currentCash + 5;
            WaveManager.Wave.Remove(this.gameObject);
            Ballistic_tower_basic.targetOptions.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TargetingBubble")
        {
            Ballistic_tower_basic.targetOptions.Add(this.gameObject);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TargetingBubble")
        {
            Ballistic_tower_basic.targetOptions.Remove(this.gameObject);
        }
    }
}
