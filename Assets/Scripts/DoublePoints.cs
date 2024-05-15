using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Naife;

public class DoublePoints : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Start double points
    public delegate void doublePoints(int multiplier, float duration);
    public static event doublePoints OnDoublePoints;    //(EVENTO)

    [SerializeField] int multiplier = 2;
    [SerializeField] float duration = 15f;

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Naife")) {

                // Event double points
                if (OnDoublePoints != null)
                    OnDoublePoints(multiplier, duration);
            }
        }
    }
}
