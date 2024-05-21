using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Lose Game
    public delegate void LoseGame();
    public static event LoseGame OnLoseGame;    //(EVENTO)

    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;
    [SerializeField] int currentPoints = 0;

    [Header("Timer")]
    [SerializeField] Timer timer;
    [SerializeField] float timerTime = 0f;
    [SerializeField] string timerTimeTxt;

    [Header("Crono")]
    [SerializeField] Crono crono;
    private Coroutine _cronoCoroutine;

    [Header("Game Over")]
    [SerializeField] bool isPlayground = false;
    [SerializeField] string nextScene = "RankingScene";
    [SerializeField] Canvas GameOverCanvas;
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI timeTxt;

    [Header("Edit Enemy")]
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] int stepToIncrease = 2;
    [SerializeField] int valueToIncrease = 2;
    private int _currentSteps = 0;

    private int _minObjectsToThrow;
    private int _maxObjectsToThrow;
    private int _minAnimSpeed;
    private int _maxAnimSpeed;
    private float _agentSpeed = 20;
    private float _agentAcceleration = 15;


    private bool _hasMultiplier = false;
    private int _pointsMultiplier = 1;

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        Naife.OnHitBarrel += OnHitBarrel;
        Naife.OnHitFruit += OnHitFruit;
        DoublePoints.OnDoublePoints += StartDoublePoints;
        EnemyPatrol.OnObjectsThrowed += IncreaseThrowValues;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        Naife.OnHitBarrel -= OnHitBarrel;
        Naife.OnHitFruit -= OnHitFruit;
        DoublePoints.OnDoublePoints -= StartDoublePoints;
        EnemyPatrol.OnObjectsThrowed -= IncreaseThrowValues;
    }

    void Start() {
        ResetLives();
        ResetPoints();
        // Deactivate GameOverCanvas
        GameOverCanvas.gameObject.SetActive(false);

        if (timer == null) {
            timer = FindObjectOfType<Timer>();
            if (timer == null)
                Debug.Log("Missing Timer");
        }
        StartTimer();

        if (enemyPatrol == null) {
            enemyPatrol = GameObject.FindAnyObjectByType<EnemyPatrol>();
            if (enemyPatrol == null)
                Debug.Log("No hay EnemyPatrol");
        }
    }

    private void OnHitBarrel(Vector3 position) {
        // Decrease live
        DecreaseLive();

        // TNT Explosion Sound
        //AudioManager.instance.PlaySFX("Explosion");

        // Check if Lose (wait some seconds and load Ranking Scene)
        if (!isPlayground)
            CheckIfLose();
    }
    private void OnHitFruit(int amountOfPoints) {
        // Increase points
        AddPoints(amountOfPoints);

        // Fruit Sound ??
        //AudioManager.instance.PlaySFX("Fruit");
    }

    public void ResetLives() {
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
        }
        else {
            currentPoints += amountOfPoints;
        }
    }

    #region Double Points
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
    #endregion

    private void CheckIfLose() {
        if (currentLives <= 0) {
            timerTime = timer.GetFloatTimer();
            timerTimeTxt = timer.GetTransformTextTimer();
            timer.StopTimer();

            // Event hit TNT Barrel
            if (OnLoseGame != null)
                OnLoseGame();

            // Show GameOverCanvas
            GameOverCanvas.gameObject.SetActive(true);
            if (pointsTxt) pointsTxt.text = currentPoints.ToString();
            if (timeTxt) timeTxt.text = timerTimeTxt;

            DataManager.instance.SetScore(currentPoints);
            DataManager.instance.SetTime(timerTime);
            DataManager.instance.SetTimeTxt(timerTimeTxt);

            // Load next scene waiting some seconds
            //SCManager.instance.LoadSceneWaiting(nextScene);
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


    #region Update Enemy Throw config values
    private void IncreaseThrowValues() {
        Debug.Log(_currentSteps);
        if (_currentSteps >= stepToIncrease) {
            _minObjectsToThrow += valueToIncrease;
            _maxObjectsToThrow += valueToIncrease;
            _agentSpeed += valueToIncrease;
            _agentAcceleration += valueToIncrease;

            //Debug.Log("Increase Throw & Agent Speed " + _minObjectsToThrow + ", " + _maxObjectsToThrow + ", " + _agentSpeed + ", " + _agentAcceleration);

            UpdateConfigValues(_minObjectsToThrow, _maxObjectsToThrow, _agentSpeed, _agentAcceleration);
            _currentSteps = 0;
        }
        _currentSteps++;
    }
    private void UpdateConfigValues(int minThrow, int maxThrow, float speed, float acceleration) {
        enemyPatrol.SetMinThrow(minThrow);
        enemyPatrol.SetMaxThrow(maxThrow);

        NavMeshAgent agent = enemyPatrol.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = acceleration;
    }
    #endregion
}
