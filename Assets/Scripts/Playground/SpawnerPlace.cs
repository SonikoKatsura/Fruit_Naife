using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlace : MonoBehaviour {
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] float spawnInterval = 5f;

    private GameObject _spawnedObject;
    private bool _canSpawn = true;

    void Start() {
        Spawn();
    }

    void Update() {
        // Verifica si el objeto anterior ha sido recogido (destruido)
        if (_spawnedObject == null && _canSpawn) {
            // Inicia la cuenta atrás para el próximo spawn
            StartCoroutine(SpawnDelay());
        }
    }

    IEnumerator SpawnDelay() {
        // Desactiva la capacidad de spawnear para evitar instanciar repetidamente
        _canSpawn = false;

        // Espera el tiempo de intervalo antes de activar la capacidad de spawnear
        yield return new WaitForSeconds(spawnInterval);

        // Activa la capacidad de spawnear y reinicia la cuenta atrás
        _canSpawn = true;
        Spawn();
    }

    void Spawn() {
        // Instancia un nuevo objeto en la posición del Spawner
        _spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
