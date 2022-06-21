using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerChallengeMode : MonoBehaviour
{

    public static UIManagerChallengeMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text coinsGainedText;
    [SerializeField] TMP_Text windText;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject timerUp;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;
    [SerializeField] TMP_Text timerText;
    public float maxTimer = 120f;
    public float timerLeft;
    bool timerActive = false;
    int startCoins, endCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        startCoins = CoinsManager.Instance.Coins;

        UpdateCoins();
    }

    private void Update()
    {
        if (timerActive)
        {
            if (timerLeft <= 0f)
            {
                TimerUp();
            }

            timerLeft -= Time.deltaTime;

            if (timerLeft <= 0f)
            {
                timerLeft = 0f;
            }

            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        timerLeft = maxTimer;
        timerActive = true;
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = timerLeft < 0 ? timerLeft.ToString("F1") : Mathf.CeilToInt(timerLeft).ToString("F0");
    }

    public void UpdateWind(int windForce)
    {
        windText.text = windForce.ToString();
    }

    public void ExitScene()
    {
        CoinsManager.Instance.Save();
        SceneManager.LoadScene(0);
    }

    public void TimerUp()
    {
        endCoins = CoinsManager.Instance.Coins;
        timerUp.SetActive(true);
        coinsGainedText.text = (endCoins - startCoins).ToString();
        finalScore.text = ScoreManager.Instance.Score.ToString();
    }

    public void onButtonClick()
    {
        Raycasting.instance.FreezeObject();
    }

    public void OnSettingsClick()
    {
        settings.SetActive(true);
    }

    public void OnExitclick()
    {
        settings.SetActive(false);
    }

    public void OnExitGame()
    {
        CoinsManager.Instance.Save();
        SceneManager.LoadScene(0);
    }

    public void UpdateScore()
    {
        scoreText.text = ScoreManager.Instance.Score.ToString();
    }

    public void UpdateCoins()
    {
        coinsText.text = CoinsManager.Instance.Coins.ToString();
    }

    public void OnSliderYChanged(float value)
    {
        PaperManager.Instance.sensivity.y = value;
        ySens.text = value.ToString();
    }

    public void OnEnterYValue(string s)
    {
        float num = 0;
        if (float.TryParse(s, out num))
        {
            if (num > 130)
            {
                num = 130;
            }
            else if (num < 1)
            {
                num = 1;
            }

            int n = Mathf.RoundToInt(num);
            PaperManager.Instance.sensivity.y = n;
            ySlider.value = n;
            ySens.text = n.ToString();
        }
    }
}
