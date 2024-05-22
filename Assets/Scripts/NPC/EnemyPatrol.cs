using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Throw Object
    public delegate void ThrownObject(GameObject obj);
    public static event ThrownObject OnThrownObject;    //(EVENTO)

    //EVENTO (DELEGADO)   --> Objetos lanzados
    public delegate void ObjectsThrowed();
    public static event ObjectsThrowed OnObjectsThrowed;    //(EVENTO)

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
    private float _minThrowAnimSpeed;
    private float _maxThrowAnimSpeed;
    //[SerializeField] float throwAnimDelay = 0.625f;

    private int _minObjectsToThrow;
    private int _maxObjectsToThrow;

    // Lista de nombres de efectos de sonido
    [SerializeField] List<string> throwSoundNames = new List<string> { "Throw1", "Throw2", "Throw3" };

    // Banderas para controlar el estado del enemigo
    private bool isPicking = false;
    private bool isThrowing = false;

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        GameManager.OnLoseGame += GoIdle;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        GameManager.OnLoseGame -= GoIdle;
    }

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

        // Evento que avisa al GameManager que ha terminado de lanzar (para la logica de aumentar la velocidad)
        if (OnObjectsThrowed != null)
            OnObjectsThrowed();

        // Volver a patrullar
        if (!isThrowing)
            StartCoroutine(Patrol());

        yield return null;
    }

    private IEnumerator ThrowFor() {
        // + 1 porque es de tipo int y el máximo es exclusivo
        int randNumbObjects = Random.Range(_minObjectsToThrow, _maxObjectsToThrow + 1);

        // Random Anim Speed
        float randAnimSpeed = Random.Range(_minThrowAnimSpeed, _maxThrowAnimSpeed);
        Animator anim = GetComponent<Animator>();
        anim.speed = randAnimSpeed;

        //Debug.LogError("nº objects- " + randNumbObjects);
        //Debug.Log("anim- " + randAnimSpeed);

        for (int i = 0; i < randNumbObjects; i++) {
            if (isThrowing) {

                // Iniciar la animación de lanzar
                anim.SetTrigger("throwing");

                // Esperar a que la animación de lanzamiento comience
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"));

                // Esperar a que la animación termine
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        isThrowing = false;
        anim.speed = 1;
    }

    // Se llama desde la animacion
    public void ThrowObject() {
        Debug.Log("throw");
        if (isThrowing) {
            Debug.Log("isThrowing");
            // Select Random Object 
            int randomIndex = Random.Range(0, objectList.Count);
            GameObject randomObject = objectList[randomIndex];

            // Select Random Sound
            int soundIndex = Random.Range(0, throwSoundNames.Count);
            string soundName = throwSoundNames[soundIndex];

            // Throw Sound
            AudioManager.instance.PlaySFX(soundName);
            

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


    #region Game Over State
    private void GoIdle() {
        StartCoroutine(Idle());
    }
    private IEnumerator Idle() {
        StopAllCoroutines();

        // Iniciar la animación Idle
        GetComponent<Animator>().SetTrigger("idle");

        yield return null;
    }
    #endregion

    #region Update Enemy Throw config values
    public void SetMinThrow(int minThrow) {
        _minObjectsToThrow = minThrow;
    }
    public void SetMaxThrow(int maxThrow) {
        _maxObjectsToThrow = maxThrow;
    }
    public void SetMinAnimSpeed(float minAnimSpeed) {
        _minThrowAnimSpeed = minAnimSpeed;
    }
    public void SetMaxAnimSpeed(float maxAnimSpeed) {
        _maxThrowAnimSpeed = maxAnimSpeed;
    }
    #endregion
}