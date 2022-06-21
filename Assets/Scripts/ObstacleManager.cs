using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;
    [SerializeField] ARRaycastManager raycast;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField] GameObject obstaclePrefab;
    GameObject obstacleInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Raycasting.instance.arPlaneManager.enabled)
        {
            if (obstacleInstance != null)
                Destroy(obstacleInstance);
            Spawn();
        }
    }

    private void Spawn()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.position.y > (Screen.height - 100))
            return;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (raycast.Raycast(ray, hits, TrackableType.PlaneWithinPolygon) && Raycasting.instance.place)
        {
            var hitPose = hits[Mathf.FloorToInt((float)hits.Count / 2f)].pose;
            if (obstacleInstance == null)
                obstacleInstance = Instantiate(obstaclePrefab);
        }   
    }
}
