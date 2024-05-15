using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;
    [SerializeField] int currentPoints = 0;

    [SerializeField] ParticleSystem explosionParticles;

    [SerializeField] string nextScene = "RankingScene";

    [Header("Timer")]
    [SerializeField] Timer timer;
    [SerializeField] float timerTime = 0f;

    [Header("Crono")]
    [SerializeField] Crono crono;
    private Coroutine _cronoCoroutine;

    private bool _hasMultiplier = false;
    private int _pointsMultiplier = 1;

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        Naife.OnHitBarrel += OnHitBarrel;
        Naife.OnHitFruit += OnHitFruit;
        DoublePoints.OnDoublePoints += StartDoublePoints;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        Naife.OnHitBarrel -= OnHitBarrel;
        Naife.OnHitFruit -= OnHitFruit;
        DoublePoints.OnDoublePoints -= StartDoublePoints;
    }

    void Start() {
        ResetLives();
        ResetPoints();

        if (timer == null) {
            timer = FindObjectOfType<Timer>();
            if (timer == null)
                Debug.Log("Missing Timer");
        }
        StartTimer();
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
        if (_hasMultiplier) {
            currentPoints += amountOfPoints * _pointsMultiplier;
        } else {
            currentPoints += amountOfPoints;
        }
    }

    private void StartDoublePoints(int multiplier, float duration) {
        if (!_hasMultiplier) {
            _hasMultiplier = true;
            _pointsMultiplier = multiplier;
            _cronoCoroutine = StartCoroutine(StartDoublePointsCrono(duration));
        }
        if (_hasMultiplier) {
            StopCoroutine(_cronoCoroutine);
            _pointsMultiplier = multiplier;
            _cronoCoroutine = StartCoroutine(StartDoublePointsCrono(duration));
        }
    }
    private IEnumerator StartDoublePointsCrono(float duration) {
        crono.StartCrono(duration);
        yield return new WaitForSeconds(duration);
        _hasMultiplier = false;
    }

    private void CheckIfLose() {
        if (currentLives <= 0) {
            timerTime = timer.GetFloatTimer();
            timer.StopTimer();

            // Load next scene waiting some seconds
            SCManager.instance.LoadSceneWaiting(nextScene);
        }
    }

    #region Crono / timer
    private void StartTimer() {
        timer.StartTimer();
    }
    public float GetLastCronoTimeFloat() {
        return timerTime;
    }
    public string GetLastCronoTimeText() {
        return timer.GetTransformTextTimer();
    }
    #endregion

    #region UI Points & Lives
    public int GetPoints() {
        return currentPoints;
    }
    public int GetLives() {
        return currentLives;
    }
    #endregion
}
