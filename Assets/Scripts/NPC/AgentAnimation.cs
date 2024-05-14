using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _movementSpeed = "MovementSpeed",
        throwing = "throwing",
        pickup = "pick";


    public UnityEvent OnStep;

    public void SetSpeed(float speed)
    {
        _animator.SetFloat(_movementSpeed, speed);
    }

    public void Throw()
    {
        _animator.SetTrigger(throwing);

    }
    public void Pick()
    {
        _animator.SetTrigger(pickup);

    }

    public void StepEvent()
    {
        OnStep.Invoke();
    }
}
