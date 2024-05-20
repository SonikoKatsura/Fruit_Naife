using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static Naife;
using static RandomObjectSelector;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Throw Object
    public delegate void ThrownObject(GameObject obj);
    public static event ThrownObject OnThrownObject;    //(EVENTO)

    // Variables para destino, área de lanzamiento y jugador
    [SerializeField] Transform destination, throwarea, player;
    // Duración de las animaciones de lanzar y recoger
    [SerializeField] float pickduration;
    // Agente de navegación
    [SerializeField] private NavMeshAgent agent;
    // Array de cestas
    [SerializeField] private GameObject[] baskets;

    [Header("Throw")]
    [SerializeField] List<GameObject> objectList;
    [SerializeField] float throwAnimSpeed = 1; 
    //[SerializeField] float throwAnimDelay = 0.625f;

    [SerializeField] int minObjectsToThrow = 1;
    [SerializeField] int maxObjectsToThrow = 10;

    //[SerializeField] float minTimeBetweenThrows = 1f;
    //[SerializeField] float maxTimeBetweenThrows = 2f;

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
        yield return new WaitForSeconds(pickduration);

        isPicking = false;

        // Ir a la zona de lanzamiento si ha recogido más de 2 cestas
        StartCoroutine(GoThrow());
    }

    private IEnumerator GoThrow()
    {
        // Establecer el destino a la zona de lanzamiento
        SetAgentDestination(throwarea.position);
        while (true)
        {
            // Invocar el evento de cambio de velocidad
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

        // Animación y nº de lanzamientos
        yield return StartCoroutine(ThrowFor());

        // Volver a patrullar
        if (!isThrowing)
            StartCoroutine(Patrol());

        yield return null;
    }

    private IEnumerator ThrowFor() {
        int randNumbObjects = Random.Range(minObjectsToThrow, maxObjectsToThrow + 1);
        Animator anim = GetComponent<Animator>();

        anim.speed = throwAnimSpeed;

        for (int i = 0; i < randNumbObjects; i++) {
            if (isThrowing)
            // Iniciar la animación de lanzar
            anim.SetTrigger("throwing");

            // Esperar a que la animación de lanzamiento comience
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"));

            // Esperar a que la animación termine
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / throwAnimSpeed);
        }

        isThrowing = false;
    }

    // Se llama desde la animacion
    public void ThrowObject() {
        Debug.Log("throw");
        if (isThrowing) {
            Debug.Log("isThrowing");
            // Select Random Object 
            int randomIndex = Random.Range(0, objectList.Count);
            GameObject randomObject = objectList[randomIndex];

            // Event Throw Object
            if (OnThrownObject != null)
                OnThrownObject(randomObject);
        }
    }
 

    /*private IEnumerator ThrowObjectWithDelay() {
        int randNumbObjects = Random.Range(minObjectsToThrow, maxObjectsToThrow + 1);

        for (int i = 0; i < randNumbObjects; i++) {
            // Select Random Object 
            int randomIndex = Random.Range(0, objectList.Count);
            GameObject randomObject = objectList[randomIndex];

            //Debug.Log(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            // Iniciar la animación de lanzar
            //GetComponent<Animator>().SetTrigger("throwing");

            // Delay de la animacion antes de lanzar
            //yield return new WaitForSeconds(throwAnimDelay);

            // Event Throw Object
            //if (OnThrownObject != null)
            //    OnThrownObject(randomObject);

            // Random Delay (FireRate)
            //float randomDelay = Random.Range(minTimeBetweenThrows, maxTimeBetweenThrows);
            yield return new WaitForSeconds(randomDelay);
        }
        isThrowing = false;
    }*/

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
        /*Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        */
        // Obtener la dirección hacia el jugador
        Vector3 directionToPlayer = target - transform.position;
        // Calcular la rotación para mirar hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        // Aplicar la rotación al Canvas
        transform.rotation = lookRotation;
    }

    private Transform GetRandomBasketPosition()
    {
        // Obtener una posición aleatoria de una cesta
        int index = Random.Range(0, baskets.Length);
        destination = baskets[index].transform;
        return destination;
    }
}
