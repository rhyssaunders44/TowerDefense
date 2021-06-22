using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathChecker : MonoBehaviour
{
    [SerializeField] private Transform Objective;
    [SerializeField] private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = Objective.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(agent.pathStatus);
    }
}
