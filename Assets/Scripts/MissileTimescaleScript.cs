using UnityEngine;
//Script for triggering missile maze mode
public class MissileTimescaleScript : MonoBehaviour
{
    public float missileSpeedMultiplier = 0.5f;
    public float zoomMultiplier = 2f;
    
    private bool isMazeMode = false;
    private Camera followCamera;
    private Transform originalTarget;
    private SmoothCameraScript smoothCam;
    private float originalZoom;
    
    void Start()
    {
        followCamera = Camera.main;
        smoothCam = followCamera.GetComponent<SmoothCameraScript>();
        originalTarget = smoothCam.target;
        originalZoom = followCamera.orthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MissileScript missile = collision.GetComponent<MissileScript>();
        if (missile != null && !isMazeMode)
        {
            ActivateMazeMode(missile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MissileScript missile = collision.GetComponent<MissileScript>();
        if (missile != null && isMazeMode)
        {
            DeactivateMazeMode(missile);
        }
    }

    void ActivateMazeMode(MissileScript missile)
    {
        isMazeMode = true;
        if (smoothCam != null)
        {
            smoothCam.target = missile.transform;
            Debug.Log(smoothCam.target);
        }
        missile.SetMazeMode(true, missileSpeedMultiplier);
        smoothCam.targetZoom = originalZoom / zoomMultiplier;
    }

    void DeactivateMazeMode(MissileScript missile)
    {
        isMazeMode = false;
        if (smoothCam != null)
        {
            smoothCam.target = originalTarget;
            Debug.Log(smoothCam.target);
        }
        missile.SetMazeMode(false, 1f);
        smoothCam.targetZoom = originalZoom;
    }
}
