using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Raycasting : MonoBehaviour
{
    public static Raycasting instance;
    public List<GameObject> placedDustbinPrefabs = new List<GameObject>();
    public List<GameObject> placingDustbinPrefabs = new List<GameObject>();
    GameObject placedDustbin;
    GameObject placingDustbin = null;
    [HideInInspector]
    public bool place = true;
    public ARPlaneManager arPlaneManager;
    [SerializeField] ARSessionOrigin arSessionOrigin;
    [SerializeField] ARRaycastManager raycast;
    [SerializeField] float minimumDistance = 3f;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject cameraObj;
    GameObject paperManager;
    public GameObject placeButton;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        paperManager = GameObject.Find("PaperManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && arPlaneManager.enabled)
            Spawn();
    }

    private void Spawn()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.position.y < 250)
            return;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (raycast.Raycast(ray, hits, TrackableType.PlaneWithinPolygon) && place)
        {
            var hitPose = hits[Mathf.FloorToInt((float)hits.Count / 2f)].pose;
            if (placingDustbin == null)
                placingDustbin = Instantiate(placingDustbinPrefabs[0]);

            arSessionOrigin.MakeContentAppearAt(placingDustbin.transform, (hitPose.position + new Vector3(0f, -0.1f, 0f)));
        }
    }

    public void FreezeObject()
    {
        arPlaneManager.enabled = !arPlaneManager.enabled;
        DisableAllARPlanes();

        ReplaceDustbins();

        place = !place;

        if (!place)
        {
            //placeButton.SetActive(false);
            PaperManager.Instance.ResetBall();
            if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
            {
                UIManagerChallengeMode.Instance.StartTimer();
            }
        }
        else
        {
            //placeButton.SetActive(true);
            DestroyActivePaperBall();
        }

        DestroyThrownPaperBalls();
    }

    public void PlaceDustbin()
    {
        arPlaneManager.enabled = false;
        DisableAllARPlanes();
        ReplaceDustbins();

        placeButton.SetActive(false);
        PaperManager.Instance.ResetBall();
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            UIManagerChallengeMode.Instance.StartTimer();
        }
        if(SceneManager.GetActiveScene().buildIndex == 5)
            UIManagerEasyMode.Instance.StartTimer();
    }

    public void RemoveDustbin()
    {
        arPlaneManager.enabled = true;
        DestroyActivePaperBall();
        EnableAllARPlanes();
        ReplaceDustbins();

        placeButton.SetActive(true);
    }

    private void DestroyThrownPaperBalls()
    {
        if (arPlaneManager.enabled)
        {
            foreach (Transform child in paperManager.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void DestroyActivePaperBall()
    {
        PaperManager.Instance.StopInvoke();

        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            if (cameraObj.transform.childCount >= 2)
            {
                bool firstChild = true;
                foreach (Transform child in cameraObj.transform)
                {
                    if (!firstChild)
                        Destroy(child.gameObject);
                    firstChild = false;
                }
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            foreach (Transform child in cameraObj.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void ReplaceDustbins()
    {
        if (arPlaneManager.enabled)
        {
            if (placedDustbin != null)
            {
                Vector3 pos = placedDustbin.transform.position;
                Quaternion rot = placedDustbin.transform.rotation;
                Destroy(placedDustbin);
                placingDustbin = Instantiate(placingDustbinPrefabs[0], pos, rot);
            }
        }
        else
        {
            if (placingDustbin != null)
            {
                Vector3 pos = placingDustbin.transform.position;
                Quaternion rot = placingDustbin.transform.rotation;
                Destroy(placingDustbin);
                placedDustbin = Instantiate(placedDustbinPrefabs[ShopManager.Instance.dustbinIndex], pos, rot);
            }
        }
    }

    private void DisableAllARPlanes()
    {
        foreach (ARPlane planes in arPlaneManager.trackables)
        {
            planes.gameObject.SetActive(false);
        }
    }

    private void EnableAllARPlanes()
    {
        foreach (ARPlane planes in arPlaneManager.trackables)
        {
            planes.gameObject.SetActive(true);
        }
    }
}
