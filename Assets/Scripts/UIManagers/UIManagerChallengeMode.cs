using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UIManagerChallengeMode : MonoBehaviour
{

    public static UIManagerChallengeMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text heartText;
    [SerializeField] TMP_Text coinsGainedText;
    [SerializeField] TMP_Text windText;
    [SerializeField] GameObject timerUp;
    [SerializeField] GameObject leftWind;
    [SerializeField] GameObject rightWind;
    [SerializeField] CanvasGroup mainPanel;
    [SerializeField] RectTransform SidePanel;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image fillImage;
    [SerializeField] CanvasGroup panelClose;
    [SerializeField] CanvasGroup panelOpen;
    public bool isPanelOpen = false;
    public float maxTimer = 120f;
    public float timerLeft = 0;
    bool timerActive = false;
    int startCoins, endCoins;
    float t = 0f;

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
        UpdateSensitivity();
    }

    private void Update()
    {
        if (timerActive)
        {
            if (timerLeft <= 0f)
            {
                TimerUp();
            }

            t += Time.deltaTime / maxTimer;
            timerLeft -= Time.deltaTime;
            timerSlider.value = timerLeft;
            fillImage.color = Color.Lerp(Color.green, Color.red, t);

            if (timerLeft <= 0f)
            {
                timerLeft = 0f;
            }
        }
    }

    public void StartTimer()
    {
        if (timerActive)
            return;
        timerLeft = maxTimer;
        timerSlider.value = timerLeft;
        timerActive = true;
    }

    public void UpdateWind(int windForce)
    {
        windText.text = Mathf.Abs(windForce).ToString();
        if(windForce < 0)
        {
            leftWind.SetActive(false);
            rightWind.SetActive(true);
        }
        else
        {
            leftWind.SetActive(true);
            rightWind.SetActive(false);
        }
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

    public void OnPlaceClick()
    {
        Raycasting.instance.PlaceDustbin();
    }

    public void OnRemoveClick()
    {
        Raycasting.instance.RemoveDustbin();
    }

    public void OnSettingsClick()
    {
        isPanelOpen = true;
        SidePanel.DOAnchorPos(new Vector2(0f, 0f), 0.75f, false).SetEase(Ease.OutExpo);
        mainPanel.DOFade(0, 0.75f);
        panelOpen.transform.gameObject.SetActive(false);
        panelOpen.DOFade(0, 0.75f);
        panelClose.transform.gameObject.SetActive(true);
        panelClose.DOFade(1, 0.75f);
    }

    public void OnExitclick()
    {
        isPanelOpen = false;
        SidePanel.DOAnchorPos(new Vector2(Screen.width, 0f), 0.75f, false).SetEase(Ease.OutQuint);
        mainPanel.DOFade(1, 0.75f);
        panelOpen.transform.gameObject.SetActive(true);
        panelOpen.DOFade(1, 0.75f);
        panelClose.transform.gameObject.SetActive(false);
        panelClose.DOFade(0, 0.75f);
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
        heartText.text = CoinsManager.Instance.Heart.ToString();
        CoinsManager.Instance.Save();
    }

    public void OnSliderYChanged(float value)
    {
        CoinsManager.Instance.sensitivity = Mathf.RoundToInt(value);
        ySens.text = value.ToString();
        CoinsManager.Instance.Save();
    }

    private void UpdateSensitivity()
    {
        ySens.text = CoinsManager.Instance.sensitivity.ToString();
        ySlider.value = CoinsManager.Instance.sensitivity;
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
            CoinsManager.Instance.sensitivity = n;
            ySlider.value = n;
            ySens.text = n.ToString();
        }
    }

    public void OnHardClick()
    {
        SceneManager.LoadScene(6);
    }

    public void OnMediumClick()
    {
        SceneManager.LoadScene(2);
    }

    public void OnEasyClick()
    {
        SceneManager.LoadScene(5);
    }
}
