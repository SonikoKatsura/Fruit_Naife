using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    [SerializeField]
    private Transform _start, _middle, _end;

    [SerializeField]
    private bool showGizmos = true;

    [SerializeField, Min(0.01f)]
    private float _heightOffset = 1;

    [SerializeField, Range(0.25f, 0.75f)]
    private float _placementOffset = 0.5f;

#if UNITY_EDITOR
    private void Update()
    {
        Vector3 start = _start.position;
        Vector3 end = _end.position;
        Vector3 midPointPosition = Vector3.Lerp(start, end, _placementOffset);
        midPointPosition.y += _heightOffset;
        SetPoints(
            start,
            midPointPosition,
            end);
    }
#endif

    private Vector3 CalculatePosition(float value01, Vector3 startPos,
        Vector3 endPos, Vector3 midPos)
    {
        value01 = Mathf.Clamp01(value01);
        Vector3 startMiddle = Vector3.Lerp(startPos, midPos, value01);
        Vector3 middleEnd = Vector3.Lerp(midPos, endPos, value01);
        return Vector3.Lerp(startMiddle, middleEnd, value01);
    }

    public Vector3 CalculatePosition(float interpolationAmount01)
        => CalculatePosition(interpolationAmount01,
            _start.position, _end.position, _middle.position);

    public Vector3 CalculatePositionCustomStart(float interpolationAmount01,
        Vector3 startPosition)
    => CalculatePosition(interpolationAmount01,
            startPosition, _end.position, _middle.position);

    public Vector3 CalculatePositionCustomEnd(float interpolationAmount01,
        Vector3 endPosition)
    => CalculatePosition(interpolationAmount01,
            _start.position, endPosition, _middle.position);

    public void SetPoints(Vector3 startPoint, Vector3 midPointPosition,
        Vector3 endPoint)
    {
        if (_start != null && _middle != null && _end != null)
        {
            _start.position = startPoint;
            _middle.position = midPointPosition;
            _end.position = endPoint;
        }
    }
    private void OnDrawGizmos()
    {
        if (showGizmos && _start != null && _middle != null && _end != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_start.position, 0.1f);
            Gizmos.DrawSphere(_end.position, 0.1f);
            Gizmos.DrawSphere(_middle.position, 0.1f);
            Gizmos.color = Color.magenta;
            int granularity = 5;
            for (int i = 0; i < granularity; i++)
            {
                Vector3 startPoint =
                    i == 0 ? _start.position
                    : CalculatePosition(i / (float)granularity);
                Vector3 endPoint =
                    i == granularity ? _end.position
                    : CalculatePosition((i + 1) / (float)granularity);
                Gizmos.DrawLine(startPoint, endPoint);
            }

        }
    }

}
