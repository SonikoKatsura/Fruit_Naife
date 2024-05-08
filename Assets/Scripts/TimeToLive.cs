using UnityEngine;

public class TimeToLive : MonoBehaviour {
    public float timeToLife = 1f;

    private float _spawnTime;

    private void Awake() {
        _spawnTime = Time.time;
    }

    private void Update() {
        float elapsedTime = Time.time - _spawnTime;
        if (elapsedTime > timeToLife) {           
                Destroy(gameObject);
        }
    }
}