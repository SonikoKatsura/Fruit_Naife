using System;
using System.Collections;
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

    void Start()
    {
        NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
        Mesh mesh = new Mesh();
        _NavMesh = mesh;
        _NavMesh.vertices = triangles.vertices;
        _NavMesh.triangles = triangles.indices;
        

        player = GameObject.FindGameObjectWithTag("Player").transform;
        RandomDestination();
        StartCoroutine(Patrol());
        StartCoroutine(Alert());
    }

    void Update()
    {
       
    }

    void RandomDestination()
    {
        destination = GetRandomPointOnMesh(_NavMesh);
        GetComponent<NavMeshAgent>().SetDestination(destination);
        GetComponent<Animator>().SetFloat("vel", 2);
        DestinationGet?.Invoke(destination);
    }
    Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        //so everything above this point wants to be factored out

        float randomsample = Random.value * total;

        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1) Debug.LogError("triIndex should never be -1");

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        //generate random barycentric coordinates

        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        //and then turn them back to a Vector3
        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
        Debug.Log(pointOnMesh);
        return pointOnMesh;
  

    }

    float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
        }
        return sizes;

        /*
         * 
         * more readably:
         * 
for(int ii = 0 ; ii < indices.Length; ii+=3)
{
    Vector3 A = Points[indices[ii]];
    Vector3 B = Points[indices[ii+1]];
    Vector3 C = Points[indices[ii+2]];
    Vector3 V = Vector3.Cross(A-B, A-C);
    Area += V.magnitude * 0.5f;
}
         * 
         * 
         * */
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
                RandomDestination();
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
