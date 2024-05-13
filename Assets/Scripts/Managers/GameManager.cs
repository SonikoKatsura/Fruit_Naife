using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Naife;

public class GameManager : MonoBehaviour {
    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;
    [SerializeField] int currentPoints = 0;

    [SerializeField] ParticleSystem explosionParticles;

    [SerializeField] string nextScene = "RankingScene";


    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        Naife.OnHitBarrel += OnHitBarrel;
        Naife.OnHitFruit += OnHitFruit;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        Naife.OnHitBarrel -= OnHitBarrel;
        Naife.OnHitFruit -= OnHitFruit;
    }

    void Start() {
        ResetLives();
        ResetPoints();
    }

    private void OnHitBarrel(Vector3 position) {
        // Decrease live
        DecreaseLive();

        // TNT Explosion Sound
        //AudioManager.instance.PlaySFX("Explosion");

        // Check if Lose (wait some seconds and load Ranking Scene)
        CheckIfLose();
    }
    private void OnHitFruit(int amountOfPoints) {
        // Increase points
        AddPoints(amountOfPoints);

        // Fruit Sound ??
        //AudioManager.instance.PlaySFX("Fruit");
    }

    private void ResetLives() {
        currentLives = maxLives;
    }
    private void ResetPoints() {
        currentPoints = 0;
    }
    private void DecreaseLive() {
        currentLives--;
    }
    private void AddPoints(int amountOfPoints) {
        currentPoints += amountOfPoints;
    }

    private void CheckIfLose() {
        if (currentLives <= 0) {
            // Load next scene waiting some seconds
            SCManager.instance.LoadSceneWaiting(nextScene);
        }
    }

    public int GetPoints() {
        return currentPoints;
    }
    public int GetLives() {
        return currentLives;
    }
}
