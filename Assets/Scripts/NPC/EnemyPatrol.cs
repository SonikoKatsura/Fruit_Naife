using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyPatrol: MonoBehaviour
{
    [SerializeField] Transform destination, throwarea, player;
    [SerializeField] float throwduration, pickduration;
   
   [SerializeField]
    private NavMeshAgent _Agent;

    public event Action<float> OnSpeedChanged;

    private int basketcounter;

    private bool _IsPicking = false;
    private bool _IsThrowing = false;
    private Mesh _NavMesh;
    public UnityEvent OnPick, OnThrow;

    [SerializeField]
    GameObject[] baskets ;

    void Start()
    {
        NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
        Mesh mesh = new Mesh();
        _NavMesh = mesh;
        _NavMesh.vertices = triangles.vertices;
        _NavMesh.triangles = triangles.indices;


        baskets = GameObject.FindGameObjectsWithTag("Basket");

        StartCoroutine(Patrol());
    }

    void Update()
    {

    }



    IEnumerator Patrol()
    {
        GetComponent<Animator>().SetFloat("vel", 2);
        destination = GetRandomBasketPosition();
        GetComponent<NavMeshAgent>().SetDestination(destination.position);
        while (true)
        {
            OnSpeedChanged?.Invoke(Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));

            if (Vector3.Distance(transform.position, destination.position) < 0.5f)
            {
                _Agent.isStopped = true;
                GetComponent<Animator>().SetFloat("velocity", 0);
                StopCoroutine(Patrol());
                StartCoroutine(PickUp());
  
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Throw()
    {
        _IsThrowing = true;
        if (_IsThrowing)
        {
            GetComponent<Animator>().SetTrigger("throwing");
            FaceTarget(player.position);


            yield return new WaitForSeconds(throwduration);
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator PickUp()
    {
        _IsPicking = true;
        while (_IsPicking)
        {
            GetComponent<Animator>().SetTrigger("pick");
            yield return new WaitForSeconds(pickduration);
            basketcounter++;
            if (basketcounter > 2)
            {
                StopCoroutine(PickUp());
                StartCoroutine(GoThrow());
            }
            else
            {
                StopCoroutine(PickUp());
                StartCoroutine(Patrol());
            }
        }
    }
    IEnumerator GoThrow()
    {
         Vector3 targetPosition = throwarea.position + Random.insideUnitSphere * 2f;
         GetComponent<NavMeshAgent>().SetDestination(targetPosition);
         while (true)
         {
            OnSpeedChanged?.Invoke(Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));
            if (Vector3.Distance(transform.position, destination.position) < 0.2f)
            {
                _Agent.isStopped = true;
                GetComponent<Animator>().SetFloat("velocity", 0);
                StopCoroutine(Patrol());
                StartCoroutine(Throw());

            }

        }
        yield return new WaitForEndOfFrame();
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation
            = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation
            = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private Transform GetRandomBasketPosition()
    {
        int index = Random.Range(0, baskets.Length);
        return baskets[index].transform;
    }

}
