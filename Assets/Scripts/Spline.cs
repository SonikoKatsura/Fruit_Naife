using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static Naife;
using static RandomObjectSelector;

[ExecuteInEditMode]
public class Spline : MonoBehaviour {
    public Transform _start, _middle, _end;

    [SerializeField]
    private bool showGizmos = true;

    [SerializeField, Min(0.01f)] float minHeightOffset = 1f;
    [SerializeField, Min(0.01f)] float maxHeightOffset = 2f;

    [SerializeField, Range(0.25f, 0.75f)]
    private float _placementOffset = 0.5f;

    //[SerializeField] private GameObject objectPrefab;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float launchRadius = 2f;

    [SerializeField]
    private float launchSpeed = 5f;

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        RandomObjectSelector.OnThrownObject += LaunchObject;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        RandomObjectSelector.OnThrownObject -= LaunchObject;
    }

#if UNITY_EDITOR
    private void Update()
    {
        /*#region Ver Parábola en update
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        targetPosition.y = player.position.y;
        CalculateMidPoint(startPoint, targetPosition);
        
        #endregion
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchObject();
        }*/
    }
#endif

    public void CalculateMidPoint(Vector3 startPoint, Vector3 targetPosition)
    {
        Vector3 end = targetPosition;
        Vector3 midPointPosition = Vector3.Lerp(startPoint, end, _placementOffset);
        // Random HeightOffset
        float randHeight = Random.Range(minHeightOffset, maxHeightOffset +1);
        midPointPosition.y += randHeight;
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

    private void LaunchObject(GameObject objectPrefab)
    {
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        targetPosition.y = player.position.y;
        CalculateMidPoint(startPoint, targetPosition);

        GameObject newObject = Instantiate(objectPrefab, startPoint, Quaternion.identity);
        StartCoroutine(LaunchObjectCoroutine(newObject, startPoint, targetPosition));
    }

    private IEnumerator LaunchObjectCoroutine(GameObject objectToLaunch, Vector3 startPoint, Vector3 endPoint)
    {
        float duration = Vector3.Distance(startPoint, endPoint) / launchSpeed;
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
