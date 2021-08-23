using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshScript : MonoBehaviour
{
    NavMeshAgent _Navmesh;
    void Start()
    {
        _Navmesh = GetComponent<NavMeshAgent>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Navmesh.SetDestination(GameManager.Instance._player.position);   
    }
}
