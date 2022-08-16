using UnityEngine;
using UnityEngine.SceneManagement;

public class ThrowPaper : MonoBehaviour
{
    public float flickSpeed = 0.4f;
    public float ballStartZ = 0.5f;
    float startTime, endTime, swipeDistance, swipeTime;
    Vector2 startPos, endPos;
    float tempTime;
    private bool thrown, holding;
    Vector3 newPosition;
    private Rigidbody _rigidbody;
    private Vector3 inputPositionCurrent;
    private Vector2 inputPositionPivot;
    private Vector2 inputPositionDifference;
    private Vector3 direction;
    public float speed = 5f;
    GameObject paperManager;
    string nameOfLayer = "Plane";
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        paperManager = GameObject.Find("PaperManager");
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        _rigidbody.useGravity = false;
        _Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 6) {
            if (UIManagerChallengeMode.Instance.isPanelOpen)
                return;
        }else if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            if (UIManagerEasyMode.Instance.isPanelOpen)
                return;
        }else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (UIManagerLivesMode.Instance.isPanelOpen)
                return;
        }

        if (CoinsManager.Instance.gameOver)
            return;

        if (holding)
        {
            OnTouch();
        }
        else if (thrown)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            inputPositionCurrent = Input.GetTouch(0).position;
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                LayerMask layer = ~(1 << LayerMask.NameToLayer(nameOfLayer));
                Ray ray = Camera.main.ScreenPointToRay(inputPositionCurrent);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, layer))
                {
                    if (hit.transform == this.transform)
                    {
                        startTime = Time.time;
                        startPos = _touch.position;
                        holding = true;
                        transform.SetParent(paperManager.transform);
                        inputPositionPivot = inputPositionCurrent;
                    }
                }
            }
            else if (_touch.phase == TouchPhase.Ended && holding)
            {
                endTime = Time.time;
                endPos = _touch.position;
                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if (swipeTime < flickSpeed && swipeDistance > 100f)
                {
                    Throw(inputPositionCurrent);
                }
                else
                {
                    _Reset();
                }
            }

            if (swipeTime > 0)
                tempTime = Time.time - startTime;
            if (tempTime > flickSpeed)
            {
                startTime = Time.time;
                startPos = _touch.position;
            }
        }

    }

    public void _Reset()
    {
        Vector3 screenPosition = new Vector3(0.5f, 0.1f, ballStartZ);
        transform.position = Camera.main.ViewportToWorldPoint(screenPosition);
        newPosition = transform.position;
        thrown = holding= false;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 200f, 0f);
        transform.SetParent(Camera.main.transform);
    }

    void Throw(Vector2 inputPosition)
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.useGravity = true;
        inputPositionDifference.y = (inputPosition.y - inputPositionPivot.y) / Screen.height * CoinsManager.Instance.sensitivity;
        inputPositionDifference.x = (inputPosition.x - inputPositionPivot.x) / Screen.width;
        inputPositionDifference.x = Mathf.Abs(inputPosition.x - inputPositionPivot.x) / Screen.width * PaperManager.Instance.sensivity.x * inputPositionDifference.x;
        direction = new Vector3(inputPositionDifference.x, 0f, 1f);
        direction = Camera.main.transform.TransformDirection(direction);
        _rigidbody.AddForce(inputPositionDifference.y * speed * (direction + Vector3.up));
        holding = false;
        thrown = true;
        audioSource.PlayOneShot(audioSource.clip);
        if (CoinsManager.Instance.tutorial == 1)
            UIManagerEasyMode.Instance.StopFlick();
        if (_rigidbody)
        {
            PaperManager.Instance.ResetBallInvoke();
            Invoke("DestroyComponenets", PaperManager.Instance.resetBallAfterSeconds + 0.2f);
        }
    }

    public void DestroyComponents()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<SphereCollider>());
        Destroy(gameObject.GetComponent<ThrowPaper>());
    }

    void OnTouch()
    {
        inputPositionCurrent.z = ballStartZ;
        newPosition = Camera.main.ScreenToWorldPoint(inputPositionCurrent);
        transform.localPosition = newPosition;
    }

}
