using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteInEditMode]
public class Spline : MonoBehaviour {
    public Transform _start, _middle, _end;

    [SerializeField]
    private bool showGizmos = true;

    [SerializeField, Min(0.01f)]
    private float minHeightOffset = 1f;

    [SerializeField, Min(0.01f)]
    private float maxHeightOffset = 2f;

    [SerializeField, Range(0.25f, 0.85f)]
    private float _placementOffset = 0.5f;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float launchRadius = 2f;

    [SerializeField, Range(-0.25f, 0.25f)]
    float zOffset = -0.1f;

    [SerializeField]
    private float launchSpeed = 5f;

    [SerializeField] private float rotationSpeed = 45f; // Degrees per second

    //SUSCRIPCIÓN al EVENTO
    void OnEnable() {
        RandomObjectSelector.OnThrownObject += LaunchObject;
    }
    //DESUSCRIPCIÓN al EVENTO
    void OnDisable() {
        RandomObjectSelector.OnThrownObject -= LaunchObject;
    }

    private void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            if (player == null)
                Debug.Log("No hay Target / Player");
        }
    }

    #if UNITY_EDITOR
    /*private void Update() {
        #region Ver Parábola en update
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        // Static y, z position
        targetPosition.y = player.position.y;
        targetPosition.z = player.position.z + zOffset;
        CalculateMidPoint(startPoint, targetPosition);
        
        #endregion
    }*/
    #endif

    public void CalculateMidPoint(Vector3 startPoint, Vector3 targetPosition) {
        Vector3 end = targetPosition;
        Vector3 midPointPosition = Vector3.Lerp(startPoint, end, _placementOffset);
        // Random HeightOffset
        float randHeight = Random.Range(minHeightOffset, maxHeightOffset + 1);
        midPointPosition.y += randHeight;
        SetPoints(startPoint, midPointPosition, end);
    }

    private Vector3 CalculatePosition(float value01, Vector3 startPos, Vector3 endPos, Vector3 midPos) {
        value01 = Mathf.Clamp01(value01);
        Vector3 startMiddle = Vector3.Lerp(startPos, midPos, value01);
        Vector3 middleEnd = Vector3.Lerp(midPos, endPos, value01);
        return Vector3.Lerp(startMiddle, middleEnd, value01);
    }

    public Vector3 CalculatePosition(float interpolationAmount01)
        => CalculatePosition(interpolationAmount01,
            _start.position, _end.position, _middle.position);

    public void SetPoints(Vector3 startPoint, Vector3 midPointPosition,
        Vector3 endPoint) {
        if (_start != null && _middle != null && _end != null) {
            _start.position = startPoint;
            _middle.position = midPointPosition;
            _end.position = endPoint;
        }
    }

    private void LaunchObject(GameObject objectPrefab) {
        Vector3 startPoint = _start.position;
        Vector3 targetPosition = player.position + Random.insideUnitSphere * launchRadius;
        // Static y, z position
        targetPosition.y = player.position.y;
        targetPosition.z = player.position.z + zOffset;

        // Calcula el punto medio para este lanzamiento específico
        Vector3 midPointPosition = Vector3.Lerp(startPoint, targetPosition, _placementOffset);
        float randHeight = Random.Range(minHeightOffset, maxHeightOffset + 1);
        midPointPosition.y += randHeight;

        GameObject newObject = Instantiate(objectPrefab, startPoint, Quaternion.identity);
        StartCoroutine(LaunchObjectCoroutine(newObject, startPoint, targetPosition, midPointPosition));
    }

    private IEnumerator LaunchObjectCoroutine(GameObject objectToLaunch, Vector3 startPoint, Vector3 endPoint, Vector3 midPoint) {
        float duration = Vector3.Distance(startPoint, endPoint) / launchSpeed;
        float t = 0f;

        #region rotation lerp
        // Start / End Random rotation
        //Quaternion initialRotation = objectToLaunch.transform.rotation;
        //Quaternion randomRotation = Random.rotation;
        #endregion

        while (t < 1f) {
            t += Time.deltaTime / duration;
            if (objectToLaunch != null) {
                objectToLaunch.transform.position = CalculatePosition(t, startPoint, endPoint, midPoint);

                #region Incremental rotation
                // Incremental rotation
                objectToLaunch.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
                objectToLaunch.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
                objectToLaunch.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
                #endregion
                #region rotation lerp
                // Random rotation
                //objectToLaunch.transform.rotation = Quaternion.Slerp(initialRotation, randomRotation, t);
                #endregion
            }
            yield return null;
        }

        Destroy(objectToLaunch);
    }

    private void OnDrawGizmos() {
        if (showGizmos && _start != null && _middle != null && _end != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_start.position, 0.1f);
            Gizmos.DrawSphere(_end.position, 0.1f);
            Gizmos.DrawSphere(_middle.position, 0.1f);
            Gizmos.color = Color.magenta;
            int granularity = 5;
            for (int i = 0; i < granularity; i++) {
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