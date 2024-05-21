using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Update Enemy Throw config values
    public delegate void UpdateConfig(int minThrow, int maxThrow, float speed, float acceleration);
    public static event UpdateConfig OnUpdateConfig;    //(EVENTO)

    [SerializeField] int minObjectsToThrow = 3;
    [SerializeField] int maxObjectsToThrow = 6;

    [SerializeField] float agentSpeed = 20;
    [SerializeField] float agentAcceleration = 15;

    void Start() {
        
    }

    void UpdateThrowValues() {
        // Event Throw Object
        if (OnUpdateConfig != null)
            OnUpdateConfig(minObjectsToThrow, maxObjectsToThrow, agentSpeed, agentAcceleration);
    }

    public void SetMinThrow(int minThrow) {
        minObjectsToThrow = minThrow;
        UpdateThrowValues();
    }
    public void SetMaxThrow(int maxThrow) {
        maxObjectsToThrow = maxThrow;
        UpdateThrowValues();
    }
    public void SetAgentSpeed(float speed) {
        agentSpeed = speed;
        UpdateThrowValues();
    }
    public void SetAgentAcceleration(float acceleration) {
        agentAcceleration = acceleration;
        UpdateThrowValues();
    }
}
