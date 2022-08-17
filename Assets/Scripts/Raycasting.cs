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
    public TMP_Text distance;
    public AudioSource audioSource;
    [SerializeField] GameObject rightArrow;
    [SerializeField] GameObject leftArrow;

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
        placeButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && arPlaneManager.enabled)
            Spawn();

        if (placedDustbin != null && !CoinsManager.Instance.gameOver)
        {
            var pos = Camera.main.WorldToScreenPoint(placedDustbin.transform.position);

            if (pos.x > Screen.safeArea.xMax)
            {
                rightArrow.SetActive(true);
                leftArrow.SetActive(false);
            }else if(pos.x < Screen.safeArea.xMin)
            {
                rightArrow.SetActive(false);
                leftArrow.SetActive(true);
            }
            else
            {
                rightArrow.SetActive(false);
                leftArrow.SetActive(false);
            }
        }

        if(placedDustbin != null)
            distance.text = Vector3.Distance(Camera.main.transform.position, placedDustbin.transform.position).ToString("F1") + "m";
        else if (placingDustbin != null)
            distance.text = Vector3.Distance(Camera.main.transform.position, placingDustbin.transform.position).ToString("F1") + "m";
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
            {
                if (CoinsManager.Instance.tutorial == 1)
                {
                    Debug.Log("Inside raycasting tutorial");
                    UIManagerEasyMode.Instance.placed = true;
                    UIManagerEasyMode.Instance.StopHand();
                    UIManagerEasyMode.Instance.ShowHandInverted();
                }

                placingDustbin = Instantiate(placingDustbinPrefabs[0]);
            }

            arSessionOrigin.MakeContentAppearAt(placingDustbin.transform, (hitPose.position + new Vector3(0f, -0.1f, 0f)));

            if(placeButton.activeInHierarchy == false)
                placeButton.SetActive(true);
        }
    }

    public void PlaceDustbin()
    {
        audioSource.Play();
        arPlaneManager.enabled = false;
        DisableAllARPlanes();
        ReplaceDustbins();

        placeButton.SetActive(false);
        PaperManager.Instance.ResetBall();
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 5 && !UIManagerChallengeMode.Instance.timerActive)
        {
            UIManagerChallengeMode.Instance.StartTimer();
        }
        if(SceneManager.GetActiveScene().buildIndex == 4 && !UIManagerEasyMode.Instance.timerActive)
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

        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 5)
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
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 4)
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
