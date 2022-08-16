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
    [SerializeField] TMP_Text heartsGainedText;
    [SerializeField] TMP_Text windText;
    [SerializeField] RectTransform timerUp;
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
    [SerializeField] RectTransform helpPanel;
    [SerializeField] GameObject questionMark;
    [SerializeField] GameObject back;
    [SerializeField] Slider audioSlider;
    [SerializeField] GameObject[] pressedImage = new GameObject[2];

    public bool isPanelOpen = false;
    public float maxTimer = 120f;
    public float timerLeft = 0;
    public bool timerActive = false, played = false;
    int startCoins, endCoins, startHeart, endHeart;
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

        audioSlider.value = CoinsManager.Instance.volume;

        startCoins = CoinsManager.Instance.Coins;
        startHeart = CoinsManager.Instance.Heart;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            pressedImage[0].SetActive(true);
            pressedImage[1].SetActive(false);
        }
        else
        {
            pressedImage[0].SetActive(false);
            pressedImage[1].SetActive(true);
        }

        CoinsManager.Instance.gameOver = false;
        UpdateCoins();
        UpdateSensitivity();
    }

    private void Update()
    {
        if (timerActive)
        {
            if (timerLeft <= 10f && !played)
            {
                AudioManager.Instance.timerAudioSource.Play();
                played = true;
            }
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
        pressedImage[0].SetActive(false);
        pressedImage[1].SetActive(false);

        CoinsManager.Instance.Save();
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(0);
    }

    public void TimerUp()
    {
        if(isPanelOpen)
            OnExitclick();
        panelOpen.gameObject.SetActive(false);
        endCoins = CoinsManager.Instance.Coins;
        endHeart = CoinsManager.Instance.Heart;
        timerUp.DOScale(1f, 0.5f).SetEase(Ease.OutCubic);
        CoinsManager.Instance.gameOver = true;
        coinsGainedText.text = (endCoins - startCoins).ToString();
        heartsGainedText.text = (endHeart - startHeart).ToString();
        finalScore.text = ScoreManager.Instance.Score.ToString();
    }

    public void OnPlaceClick()
    {
        Raycasting.instance.PlaceDustbin();
        AudioManager.Instance.uiClickSource.Play();
    }

    public void OnRemoveClick()
    {
        Raycasting.instance.RemoveDustbin();
        AudioManager.Instance.uiClickSource.Play();
        OnExitclick();
    }

    public void OnSettingsClick()
    {
        AudioManager.Instance.uiClickSource.Play();
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
        AudioManager.Instance.uiClickSource.Play();
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
        AudioManager.Instance.uiClickSource.Play();
        CoinsManager.Instance.Save();
        AudioManager.Instance.OnMainScreenLoad();
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
        ySlider.value = (float)CoinsManager.Instance.sensitivity;
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
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(5);
    }

    public void OnMediumClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(2);
    }

    public void OnEasyClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(4);
    }

    public void OnReload()
    {
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuestionClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        helpPanel.DOScale(1f, 0.5f).SetEase(Ease.OutCubic);
        questionMark.SetActive(false);
        back.SetActive(true);
        panelOpen.gameObject.SetActive(false);
    }

    public void OnBackClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        helpPanel.DOScale(0f, 0.5f).SetEase(Ease.OutCubic);
        questionMark.SetActive(true);
        back.SetActive(false);
        panelOpen.gameObject.SetActive(true);
    }

    public void OnVolumeSliderChange(float value)
    {
        CoinsManager.Instance.volume = value;
        CoinsManager.Instance.SaveVolume();
        CoinsManager.Instance.LoadVolume();
    }

    public void OnMuteClick()
    {
        if (CoinsManager.Instance.volume == 0)
            CoinsManager.Instance.volume = 1;
        else
            CoinsManager.Instance.volume = 0;

        CoinsManager.Instance.SaveVolume();
        CoinsManager.Instance.LoadVolume();
        audioSlider.value = CoinsManager.Instance.volume;
    }
}
