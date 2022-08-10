using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManagerChallengeMode : MonoBehaviour
{

    public static UIManagerChallengeMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text coinsGainedText;
    [SerializeField] TMP_Text windText;
    [SerializeField] GameObject timerUp;
    [SerializeField] GameObject leftWind;
    [SerializeField] GameObject rightWind;
    [SerializeField] RectTransform mainPanel;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;
    [SerializeField] Slider timerSlider;
    [SerializeField] Animator panel;
    [SerializeField] Image fillImage;
    public float maxTimer = 120f;
    public float timerLeft;
    bool timerActive = true;
    int startCoins, endCoins;
    float hue = 130;

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

            hue += ((maxTimer - timerLeft) * 5.1f) + 130;
            timerLeft -= Time.deltaTime;
            timerSlider.value = timerLeft;
            fillImage.color = Color.HSVToRGB(hue, 100f, 100f);

            if (timerLeft <= 0f)
            {
                timerLeft = 0f;
            }
        }
    }

    public void StartTimer()
    {
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
        panel.SetBool("isClick", true);
    }

    public void OnExitclick()
    {
        panel.SetBool("isClick", false);
        //mainPanel.SetActive(true);
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
