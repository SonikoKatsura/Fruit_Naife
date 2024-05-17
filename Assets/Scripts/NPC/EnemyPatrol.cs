using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    // Variables para destino, área de lanzamiento y jugador
    [SerializeField] Transform destination, throwarea, player;
    // Duración de las animaciones de lanzar y recoger
    [SerializeField] float throwduration, pickduration;
    // Agente de navegación
    [SerializeField] private NavMeshAgent agent;
    // Array de cestas
    [SerializeField] private GameObject[] baskets;

    // Eventos para cambios de velocidad y acciones específicas
    public event Action<float> OnSpeedChanged;
    public UnityEvent OnPick, OnThrow;

    // Contador de cestas recogidas
    private int basketCounter = 0;
    // Banderas para controlar el estado del enemigo
    private bool isPicking = false;
    private bool isThrowing = false;

    private void Start()
    {
        // Encontrar todas las cestas con la etiqueta "Basket"
        baskets = GameObject.FindGameObjectsWithTag("Basket");
        // Iniciar la corutina de patrullaje
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        // Establecer el destino inicial a una cesta aleatoria
        SetAgentDestination(GetRandomBasketPosition().position);
        while (true)
        {
            // Invocar el evento de cambio de velocidad
            OnSpeedChanged?.Invoke(Mathf.Clamp01(agent.velocity.magnitude / agent.speed));
            GetComponent<Animator>().SetFloat("vel", agent.speed);
            // Verificar si el agente ha alcanzado el destino
            if (HasReachedDestination())
            {
                // Detener al agente
                agent.isStopped = true;
                GetComponent<Animator>().SetFloat("vel", 0);
                // Iniciar la corutina de recoger
                StartCoroutine(PickUp());
                // Salir de la corutina de patrullaje
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator PickUp()
    {
        isPicking = true;
        // Mirar hacia la cesta
        FaceTarget(destination.position);
        // Iniciar la animación de recoger
        GetComponent<Animator>().SetTrigger("pick");
        OnPick?.Invoke();
        yield return new WaitForSeconds(pickduration);

        // Incrementar el contador de cestas recogidas
        basketCounter++;
        isPicking = false;

        if (basketCounter > 2)
        {
            // Ir a la zona de lanzamiento si ha recogido más de 2 cestas
            StartCoroutine(GoThrow());
        }
        else
        {
            // Continuar patrullando si no ha recogido suficientes cestas
            StartCoroutine(Patrol());
        }
    }

    private IEnumerator GoThrow()
    {
        // Establecer el destino a la zona de lanzamiento
        SetAgentDestination(throwarea.position);
        while (true)
        {
            // Invocar el evento de cambio de velocidad
            OnSpeedChanged?.Invoke(Mathf.Clamp01(agent.velocity.magnitude / agent.speed));
            GetComponent<Animator>().SetFloat("vel", agent.speed);
            // Verificar si el agente ha alcanzado el destino
            if (HasReachedDestination())
            {
                // Detener al agente
                agent.isStopped = true;
                GetComponent<Animator>().SetFloat("vel", 0);
                // Iniciar la corutina de lanzar
                StartCoroutine(Throw());
                // Salir de la corutina de ir a la zona de lanzamiento
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Throw()
    {
        isThrowing = true;
        // Mirar hacia el jugador
        FaceTarget(player.position);
        // Iniciar la animación de lanzar
        GetComponent<Animator>().SetTrigger("throwing");
        OnThrow?.Invoke();
        yield return new WaitForSeconds(throwduration);
        isThrowing = false;

        // Resetear el contador de cestas recogidas después de lanzar
        basketCounter = 0;
        // Volver a patrullar
        StartCoroutine(Patrol());
    }

    private void SetAgentDestination(Vector3 targetPosition)
    {
        // Reanudar el movimiento del agente
        agent.isStopped = false;
        // Establecer el destino del agente
        agent.SetDestination(targetPosition);
    }

    private bool HasReachedDestination()
    {
        // Verificar si el agente ha alcanzado su destino
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void FaceTarget(Vector3 target)
    {
        // Girar gradualmente hacia el objetivo
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private Transform GetRandomBasketPosition()
    {
        // Obtener una posición aleatoria de una cesta
        int index = Random.Range(0, baskets.Length);
        destination = baskets[index].transform;
        return destination;
    }
}
