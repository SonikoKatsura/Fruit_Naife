using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naife : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Hit Barrel
    public delegate void HitBarrel(Vector3 position);
    public static event HitBarrel OnHitBarrel;    //(EVENTO)

    //EVENTO (DELEGADO)   --> Hit Fruit
    public delegate void HitFruit(int amountOfPoints);
    public static event HitFruit OnHitFruit;    //(EVENTO)

    [SerializeField] ParticleSystem naifeParticles;
    [SerializeField] bool rightHand;


    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Barrel")) {
                // Naife Effects
                NaifeCollisionEffects();

                // Vibration
                HapticManager.instance.PlayHapticClip("explosion_2", rightHand);

                // Event hit TNT Barrel
                if (OnHitBarrel != null)
                    OnHitBarrel(transform.position);

                // Destroy Barrel
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("Fruit") || other.gameObject.CompareTag("Cheese")) {
                // Amount of current fruit points
                Fruit currentFruit = other.gameObject.GetComponent<Fruit>();
                int fruitPoints = currentFruit.GetAmountOfPoints();

                // Naife Effects
                NaifeCollisionEffects();

                // Vibration
                HapticManager.instance.PlayHapticClip("sword", rightHand);

                // Event hit Fruit
                if (OnHitFruit != null)
                    OnHitFruit(fruitPoints);
            }

            //Debug.Log(other.gameObject.name);
        }
    }

    private void NaifeCollisionEffects() {
        // Play sound
        //AudioManager.instance.PlaySFX("Cut");

        Vector3 swordTipPosition = this.transform.position;                     // Position        
        Quaternion rotationOffset = Quaternion.Euler(0f, -90f, 90f);            // Particle rotation offset
        Quaternion swordRotation = this.transform.rotation * rotationOffset;    // Fixed rotation

        // PLay Particle system Effect
        Instantiate(naifeParticles, swordTipPosition, swordRotation);
    }
}
