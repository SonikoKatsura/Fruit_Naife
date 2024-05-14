using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    [SerializeField] Transform target;

    private void Start() {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            if (target == null)
                Debug.Log("No hay Target / Player");
        }
    }

    void Update() {
        if (target != null) {
            // Obtener la direcci�n hacia el jugador
            Vector3 directionToPlayer = target.position - transform.position;

            // Calcular la rotaci�n para mirar hacia el jugador
            Quaternion lookRotation = Quaternion.LookRotation(-directionToPlayer);

            // Aplicar la rotaci�n al Canvas
            transform.rotation = lookRotation;
        }
        else {
            Debug.LogWarning("Player transform reference not set in LookAtPlayer script.");
        }
    }
}
