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
    [SerializeField] Vector3 min, max;
    Vector3 destination;
    [SerializeField] float playerDetectionDistance, playerAttackDistance;
    Transform player;
    [SerializeField] float visionAngle;
   
   [SerializeField]
    private NavMeshAgent _Agent;

    public event Action<float> OnSpeedChanged;

    private bool _IsThrowing = false;
    private Mesh _NavMesh;
    [SerializeField]
    private float _jumpDuration = 0.8f;
    public event Action<Vector3> DestinationGet;
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


        baskets = GameObject.FindGameObjectsWithTag("Baskets");

        StartCoroutine(Patrol());
        StartCoroutine(Alert());
    }

    void Update()
    {
       
    }

  

IEnumerator Patrol()
    {
        GetComponent<NavMeshAgent>().SetDestination(destination);
        while (true)
        {
            OnSpeedChanged?.Invoke(
                   Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));

            if (_IsThrowing)
            {
                FaceTarget(_Agent.currentOffMeshLinkData.endPos);
            }
            if (Vector3.Distance(transform.position, destination) < 1.5f)
            {
                GetComponent<Animator>().SetFloat("velocity", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
  
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Alert()
    {
        while (true)
        {
            OnSpeedChanged?.Invoke(
                   Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));

            if (_IsThrowing)
            {
                FaceTarget(_Agent.currentOffMeshLinkData.endPos);
            }
            if (Vector3.Distance(transform.position, player.position) < playerDetectionDistance)
            {
                Vector3 vectorPlayer = player.position - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward) < visionAngle)
                {
                    Debug.Log("Personaje detectado");
                    StopCoroutine(Patrol());
                    StartCoroutine(Attack());
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            OnSpeedChanged?.Invoke(
                    Mathf.Clamp01(_Agent.velocity.magnitude / _Agent.speed));
            if (_IsThrowing)
            {
                FaceTarget(_Agent.currentOffMeshLinkData.endPos);
            }
            if (Vector3.Distance(transform.position, player.position) > playerDetectionDistance)
            {
                StartCoroutine(Patrol());
                StartCoroutine(Alert());
                break;
            }
            if (Vector3.Distance(transform.position, player.position) < playerAttackDistance)
            {
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                GetComponent<Animator>().SetBool("attack", true);
                yield return new WaitForSeconds(3);
                Debug.Log("Ataque");
            }
            else
            {
                GetComponent<NavMeshAgent>().SetDestination(player.position);
                GetComponent<Animator>().SetBool("attack", false);
            }
            yield return new WaitForEndOfFrame();
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
