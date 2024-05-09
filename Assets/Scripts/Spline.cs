using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    public Transform _start, _middle, _end;

    [SerializeField]
    private bool showGizmos = true;

    [SerializeField, Min(0.01f)]
    private float _heightOffset = 1;

    [SerializeField, Range(0.25f, 0.75f)]
    private float _placementOffset = 0.5f;

    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float launchRadius = 2f;

#if UNITY_EDITOR
    private void Update()
    {
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        CalculateMidPoint(startPoint, targetPosition);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchObject();
        }
    }
#endif

    public void CalculateMidPoint(Vector3 startPoint, Vector3 targetPosition)
    {
        Vector3 end = targetPosition;
        Vector3 midPointPosition = Vector3.Lerp(startPoint, end, _placementOffset);
        midPointPosition.y += _heightOffset;
        SetPoints(startPoint, midPointPosition, end);
    }

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

    private void LaunchObject()
    {
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        CalculateMidPoint(startPoint, targetPosition);

        GameObject newObject = Instantiate(objectPrefab, startPoint, Quaternion.identity);
        StartCoroutine(LaunchObjectCoroutine(newObject, startPoint, targetPosition));
    }

    private IEnumerator LaunchObjectCoroutine(GameObject objectToLaunch, Vector3 startPoint, Vector3 endPoint)
    {
        float duration = Vector3.Distance(startPoint, endPoint);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            objectToLaunch.transform.position = CalculatePosition(t, startPoint, endPoint, _middle.position);
            yield return null;
        }

        Destroy(objectToLaunch);
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
