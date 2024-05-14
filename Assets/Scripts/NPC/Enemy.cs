using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyPatrol _input;
    [SerializeField]
    private AgentAnimation _agentAnimation;


    private void Start()
    {
        _input.OnSpeedChanged += _agentAnimation.SetSpeed;
        _input.OnThrow.AddListener(_agentAnimation.Throw);
        _input.OnPick.AddListener(_agentAnimation.Pick);
        _agentAnimation.SetSpeed(0);
    }

}