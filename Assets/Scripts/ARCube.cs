using System.Collections;
using UnityEngine;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Extensions;

public class ARCube : MonoBehaviour
{
    [Header("AR Settings")]
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _planeDetectionExtent = 0.5f;

    [Header("Geofence Settings")]
    [Tooltip("Целевая точка (широта, долгота)")]
    [SerializeField] private double _targetLatitude = 0.0;
    [SerializeField] private double _targetLongitude = 0.0;
    [Tooltip("Радиус активации куба в метрах")]
    [SerializeField] private float _activationRadius = 50f;

    private IARSession _arSession;
    private GameObject _placedCube;
    private bool _locationReady = false;
    private bool _insideGeofence = false;

    private void Start()
    {
        StartCoroutine(StartLocationService());
        StartARSession();
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Геолокация не включена или нет разрешения у приложения");
            yield break;
        }

        Input.location.Start(10f, 10f);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait-- > 0)
            yield return new WaitForSeconds(1);

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("Не удалось запустить сервис геолокации");
            yield break;
        }

        _locationReady = true;
        CheckGeofence();
    }

    private void CheckGeofence()
    {
        var data = Input.location.lastData;
        _insideGeofence = HaversineDistance(
            data.latitude, data.longitude,
            _targetLatitude, _targetLongitude
        ) <= _activationRadius;

        Debug.Log($"Пользователь в зоне: {_insideGeofence} (расстояние: {HaversineDistance(data.latitude, data.longitude, _targetLatitude, _targetLongitude):F1} м)");
    }

    private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;
        var dLat = Mathf.Deg2Rad * (float)(lat2 - lat1);
        var dLon = Mathf.Deg2Rad * (float)(lon2 - lon1);
        var a = Mathf.Sin((float)dLat / 2) * Mathf.Sin((float)dLat / 2) +
                Mathf.Cos(Mathf.Deg2Rad * (float)lat1) * Mathf.Cos(Mathf.Deg2Rad * (float)lat2) *
                Mathf.Sin((float)dLon / 2) * Mathf.Sin((float)dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));
        return R * c;
    }

    private void StartARSession()
    {
        var config = ARWorldTrackingConfigurationFactory.Create();
        config.PlaneDetection = PlaneDetection.HorizontalAndVertical;
        config.PlaneDetectionExtent = _planeDetectionExtent;

        _arSession = ARSessionFactory.Create(config).Run();
        _arSession.SessionUpdated += OnFrameUpdated;
    }

    private void OnFrameUpdated(FrameUpdatedArgs args)
    {
        if (!_locationReady)
            return;

        CheckGeofence();
        if (!_insideGeofence)
        {
            if (_placedCube != null)
                _placedCube.SetActive(false);
            return;
        }

        if (_placedCube != null)
            _placedCube.SetActive(true);

        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
            return;

        var touchPos = Input.GetTouch(0).position;
        var hits = args.Session.HitTest(touchPos, ARHitTestResultType.ExistingPlaneUsingExtent);
        if (hits.Count == 0)
            return;

        var worldPos = hits[0].WorldTransform.ToUnity().position;
        if (_placedCube == null)
        {
            _placedCube = Instantiate(_cubePrefab, worldPos, Quaternion.identity);
        }
        else
        {
            _placedCube.transform.position = worldPos;
        }
    }

    private void OnDestroy()
    {
        if (_arSession != null)
            _arSession.SessionUpdated -= OnFrameUpdated;
        Input.location.Stop();
    }
}

