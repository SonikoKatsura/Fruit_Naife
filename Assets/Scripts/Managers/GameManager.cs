using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Naife;

public class GameManager : MonoBehaviour {
    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;

    [SerializeField] ParticleSystem explosionParticles;

    [SerializeField] string nextScene = "RankingScene";


    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        Naife.OnHitBarrel += OnHitBarrel;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        Naife.OnHitBarrel -= OnHitBarrel;
    }

    void Start() {
        ResetLives();
    }

    private void OnHitBarrel(Vector3 position) {
        // Decrease live
        DecreaseLive();

        // TNT Explosion Sound
        //AudioManager.instance.PlaySFX("Explosion");

        // Check if Lose (wait some seconds and load Ranking Scene)
        CheckIfLose();
    }

    private void ResetLives() {
        currentLives = maxLives;
    }
    private void DecreaseLive() {
        currentLives--;
    }

    private void CheckIfLose() {
        if (currentLives <= 0) {
            // Load next scene waiting some seconds
            SCManager.instance.LoadSceneWaiting(nextScene);
        }
    }
}
