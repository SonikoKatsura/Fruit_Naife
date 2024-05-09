using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Naife : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Hit Barrel
    public delegate void HitBarrel(Vector3 position);
    public static event HitBarrel OnHitBarrel;    //(EVENTO)

    [SerializeField] ParticleSystem naifeParticles;

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            // Effects
            NaifeCollisionEffects();

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

    private void NaifeCollisionEffects() {
        // Play sound
        //AudioManager.instance.PlaySFX("Cut");


        Vector3 swordTipPosition = this.transform.position;                     // Position        
        Quaternion rotationOffset = Quaternion.Euler(0f, -90f, 0f);             // Particle rotation offset
        Quaternion swordRotation = this.transform.rotation * rotationOffset;    // Fixed rotation

        // PLay Particle system Effect
        Instantiate(naifeParticles, swordTipPosition, swordRotation);
    }
}
