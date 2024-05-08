using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naife : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Hit Barrel
    public delegate void HitBarrel(Vector3 position);
    public static event HitBarrel OnHitBarrel;    //(EVENTO)

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Barrel")) {                

                // Event hit TNT Barrel
                if (OnHitBarrel != null)
                    OnHitBarrel(transform.position);

                // Destroy Barrel
                Destroy(other.gameObject);
            }
            Debug.Log(other.gameObject.name);
        }
    }
}
