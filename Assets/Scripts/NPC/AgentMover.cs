using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AgentMover : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _Agent;

    public event Action<float> OnSpeedChanged;

    private bool IsThrowing = false;

    [SerializeField]
    private float _jumpDuration = 0.8f;

    public UnityEvent OnLand, OnStartJump;

    private void Start()
    {
        _Agent.autoTraverseOffMeshLink = false;
    }


    public void SetDestination(Vector3 destination)
    {

        _Agent.destination = destination;
    }

    private void Update()
    {
        OnSpeedChanged?.Invoke(
            Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));

        if (IsThrowing)
        {
            FaceTarget(_Agent.currentOffMeshLinkData.endPos);
        }
    }


    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation 
            = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation 
            = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

}
