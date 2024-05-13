using Oculus.Interaction.DebugTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Naife;

public class RandomObjectSelector : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Throw Object
    public delegate void ThrownObject(GameObject obj);
    public static event ThrownObject OnThrownObject;    //(EVENTO)

    [SerializeField] List<GameObject> objectList;

    [SerializeField] int minObjectsToThrow = 1;
    [SerializeField] int maxObjectsToThrow = 10;

    [SerializeField] float minTimeBetweenThrows = 1f;
    [SerializeField] float maxTimeBetweenThrows = 2f;

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("Lanzar");
            ThrowObjects();
        }
    }

    public void ThrowObjects() {
        Debug.Log("-----Trhow-----");
        StartCoroutine(ThrowObjectWithDelay());
    }

    private IEnumerator ThrowObjectWithDelay() {
        int randNumbObjects = Random.Range(minObjectsToThrow, maxObjectsToThrow + 1);

        for (int i = 0; i < randNumbObjects; i++) {
            // Select Random Object 
            int randomIndex = Random.Range(0, objectList.Count);
            GameObject randomObject = objectList[randomIndex];

            // Event Throw Object
            if (OnThrownObject != null)
                OnThrownObject(randomObject);

            // Random Delay (FireRate)
            float randomDelay = Random.Range(minTimeBetweenThrows, maxTimeBetweenThrows);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
