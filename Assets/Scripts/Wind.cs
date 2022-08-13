using UnityEngine;
using UnityEngine.SceneManagement;

public class Wind : MonoBehaviour
{
    public static Wind Instance;
    Rigidbody _rigidbody;
    float windForce = 0, previousWind;
    bool applyWind;
    [SerializeField] float maxWind = 5f;

    private void Awake()
    {
        if(Instance != null && Instance == this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetWind();
    }

    private void OnTriggerEnter(Collider other)
    {
        applyWind = true;
        _rigidbody = other.gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        applyWind = false;
        _rigidbody = null;
    }

    private void FixedUpdate()
    {
        if (applyWind && _rigidbody != null)
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
                _rigidbody.AddForce(Vector3.right * (windForce / 5f));
            else
                _rigidbody.AddForce(Vector3.right * (windForce / 3f));
        }
    }

    public void ResetWind()
    {
        previousWind = windForce;
        while(Mathf.FloorToInt(previousWind) == Mathf.FloorToInt(windForce))
            windForce = Random.Range(-maxWind, maxWind);
        UIManagerChallengeMode.Instance.UpdateWind(Mathf.RoundToInt(windForce));
    }
}
