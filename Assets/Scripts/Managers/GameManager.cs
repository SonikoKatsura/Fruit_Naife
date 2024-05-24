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
    private int _currentLives;
    private int _currentPoints = 0;

    [Header("Timer")]
    [SerializeField] Timer timer;
    private float _timerTime = 0f;
    private string _timerTimeTxt;

    [Header("Crono")]
    [SerializeField] Crono crono;
    private Coroutine _cronoCoroutine;

    [Header("InGameUI Canvas")]
    [SerializeField] Canvas InGameUICanvas;

    [Header("Game Over")]
    [SerializeField] bool isPlayground = false;
    [SerializeField] string nextScene = "RankingScene";
    [SerializeField] Canvas GameOverCanvas;
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI timeTxt;

    [Header("Edit Enemy")]
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] int stepToIncrease = 2;
    private int _currentSteps = 0;

    [Header("Objects Throw")]
    [SerializeField] int valueToIncreaseObjects = 2;
    [SerializeField] int minObjectsToThrow = 3;
    [SerializeField] int maxObjectsToThrow = 6;

    [Header("Anim Speed")]
    [SerializeField, Range(0.01f, 0.3f)]
    float valueToIncreaseAnim = 0.1f;
    [SerializeField] float minAnimSpeed = 1;
    [SerializeField] float maxAnimSpeed = 2f;
    [SerializeField] float maxAnimSpeedStatic = 4f;

    [Header("Agent Speed")]
    [SerializeField] float agentSpeed = 20;
    [SerializeField] float agentAcceleration = 15;


    private bool _hasMultiplier = false;
    private int _pointsMultiplier = 1;


    //SUSCRIPCI�N al EVENTO
    void OnEnable() {
        Naife.OnHitBarrel += OnHitBarrel;
        Naife.OnHitFruit += OnHitFruit;
        DoublePoints.OnDoublePoints += StartDoublePoints;
        EnemyPatrol.OnObjectsThrowed += IncreaseThrowValues;
        Spline.OnCheeseDecreaseLive += DecreaseLiveAndChecklose;
        SCSelectorBtn.OnRestartGame += ResetAndPlayGame;
    }
    //DESUSCRIPCI�N al EVENTO
    void OnDisable() {
        Naife.OnHitBarrel -= OnHitBarrel;
        Naife.OnHitFruit -= OnHitFruit;
        DoublePoints.OnDoublePoints -= StartDoublePoints;
        EnemyPatrol.OnObjectsThrowed -= IncreaseThrowValues;
        Spline.OnCheeseDecreaseLive -= DecreaseLiveAndChecklose;
        SCSelectorBtn.OnRestartGame -= ResetAndPlayGame;
    }

    void Start() {
        ResetAndPlayGame();
    }

    private void ResetAndPlayGame() {
        ResumeGame();

        ResetLives();
        ResetPoints();

        // Activate InGameUI
        if (InGameUICanvas) InGameUICanvas.gameObject.SetActive(true);
        // Deactivate GameOverCanvas
        if (GameOverCanvas) GameOverCanvas.gameObject.SetActive(false);

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

        // Set initial enemy values
        if (enemyPatrol != null)
            UpdateConfigValues(minObjectsToThrow, maxObjectsToThrow, minAnimSpeed, maxAnimSpeed, agentSpeed, agentAcceleration);
    }

    private void OnHitBarrel(Vector3 position) {
        // Decrease live
        DecreaseLiveAndChecklose();

        // TNT Explosion Sound
        AudioManager.instance.PlaySFX("Explosion");
    }
    private void OnHitFruit(int amountOfPoints) {
        // Increase points
        AddPoints(amountOfPoints);

        // Fruit Sound ??
        AudioManager.instance.PlaySFX("Fruit");
    }

    public void ResetLives() {
        _currentLives = maxLives;
    }
    private void ResetPoints() {
        _currentPoints = 0;
    }
    private void DecreaseLiveAndChecklose() {
        AudioManager.instance.PlaySFX("Hit");
        _currentLives--;

        // Check if Lose (wait some seconds and load Ranking Scene)
        if (!isPlayground)
            CheckIfLose();
    }
    private void AddPoints(int amountOfPoints) {
        if (_hasMultiplier) {
            _currentPoints += amountOfPoints * _pointsMultiplier;
        }
        else {
            _currentPoints += amountOfPoints;
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
        if (_currentLives <= 0) {
            _timerTime = GetLastCronoTimeFloat();
            _timerTimeTxt = GetLastCronoTimeText();
            timer.StopTimer();

            // Event hit TNT Barrel
            if (OnLoseGame != null)
                OnLoseGame();

            // Hide InGameUI
            if (InGameUICanvas) InGameUICanvas.gameObject.SetActive(false);

            // Show GameOverCanvas
            GameOverCanvas.gameObject.SetActive(true);
            if (pointsTxt) pointsTxt.text = _currentPoints.ToString();
            if (timeTxt) timeTxt.text = _timerTimeTxt;

            // Pause
            PauseGame();

            // Save Points and Time
            DataManager.instance.SetScore(_currentPoints);
            DataManager.instance.SetTime(_timerTime);
            DataManager.instance.SetTimeTxt(_timerTimeTxt);

            // Load next scene waiting some seconds
            //SCManager.instance.LoadSceneWaiting(nextScene);
        }
    }

    #region Crono / timer
    private void StartTimer() {
        timer.StartTimer();
    }
    public float GetLastCronoTimeFloat() {
        return timer.GetFloatTimer();
    }
    public string GetLastCronoTimeText() {
        return timer.GetTransformTextTimer();
    }
    #endregion

    #region UI Points & Lives
    public int GetPoints() {
        return _currentPoints;
    }
    public int GetLives() {
        return _currentLives;
    }
    #endregion


    #region Update Enemy Throw config values
    private void IncreaseThrowValues() {
        if (_currentSteps >= stepToIncrease) {
            // Num Objects
            minObjectsToThrow += valueToIncreaseObjects;
            maxObjectsToThrow += valueToIncreaseObjects;

            // Anim Speed
            if (minAnimSpeed < maxAnimSpeedStatic) minAnimSpeed += valueToIncreaseAnim;
            if (maxAnimSpeed < maxAnimSpeedStatic) maxAnimSpeed += valueToIncreaseAnim;
            if (maxAnimSpeed > maxAnimSpeedStatic) maxAnimSpeed = maxAnimSpeedStatic;
            if (minAnimSpeed > maxAnimSpeedStatic) minAnimSpeed = maxAnimSpeedStatic;

            // Agent config
            agentSpeed += valueToIncreaseObjects;
            agentAcceleration += valueToIncreaseObjects;

            Debug.Log("Increase Throw & Agent Speed " + minObjectsToThrow + ", " + maxObjectsToThrow + ", "+ minAnimSpeed + ", " + maxAnimSpeed + ", " + agentSpeed + ", " + agentAcceleration);

            UpdateConfigValues(minObjectsToThrow, maxObjectsToThrow, minAnimSpeed, maxAnimSpeed, agentSpeed, agentAcceleration);
            _currentSteps = 0;
        }
        _currentSteps++;
    }
    private void UpdateConfigValues(int minThrow, int maxThrow, float minAnimSpeed, float maxAnimSpeed, float speed, float acceleration) {
        // Num Objects
        enemyPatrol.SetMinThrow(minThrow);
        enemyPatrol.SetMaxThrow(maxThrow);

        // Anim Speed
        enemyPatrol.SetMinAnimSpeed(minAnimSpeed);
        enemyPatrol.SetMaxAnimSpeed(maxAnimSpeed);

        // Agent config
        NavMeshAgent agent = enemyPatrol.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = acceleration;
    }
    #endregion

    #region Play / Pause
    void PauseGame() {
        Time.timeScale = 0;
    }

    void ResumeGame() {
        Time.timeScale = 1;
    }
    #endregion
}
