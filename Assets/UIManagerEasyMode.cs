using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class UIManagerEasyMode : MonoBehaviour
{
    public static UIManagerEasyMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text heartText;
    [SerializeField] TMP_Text coinsGainedText;
    [SerializeField] TMP_Text heartsGainedText;
    [SerializeField] RectTransform timerUp;
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
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject firstFrame;
    [SerializeField] GameObject secondFrame;
    [SerializeField] GameObject thirdFrame;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject handInverted;
    [SerializeField] GameObject flick;
    [SerializeField] GameObject phone;
    [SerializeField] ARPlaneManager arPlaneManager;
    [SerializeField] Slider audioSlider;
    [SerializeField] GameObject pressedImage;
    [SerializeField] GameObject HandPointingQuestionMark;
    [SerializeField] GameObject HandPointingOnMenu;
    [SerializeField] GameObject menuHand;
    [SerializeField] Sprite mute, unmute;
    [SerializeField] Image muteButton;
    public bool isPanelOpen, placed = false;
    public float maxTimer = 120f;
    public float timerLeft;
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

        pressedImage.SetActive(true);
        CoinsManager.Instance.gameOver = false;
        UpdateCoins();
        UpdateSensitivity();

        CoinsManager.Instance.gameOver = false;
        if (CoinsManager.Instance.tutorial == 1)
        {
            ShowTutorial();
            arPlaneManager.enabled = false;
        }
    }

    private void Update()
    {
        if ((CoinsManager.Instance.tutorial == 1) && arPlaneManager.trackables.count > 0 && !placed && !tutorial.activeInHierarchy)
        {
            StopPhone();
            ShowHand();
        }

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
        timerLeft = maxTimer;
        timerSlider.value = timerLeft;
        timerActive = true;
    }

    public void ExitScene()
    {
        pressedImage.SetActive(false);
        CoinsManager.Instance.Save();
        AudioManager.Instance.OnMainScreenLoad();
        SceneManager.LoadScene(0);
    }

    public void TimerUp()
    {
        if (isPanelOpen)
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
        AudioManager.Instance.uiClickSource.Play();
        StopHandInverted();
        if(CoinsManager.Instance.tutorial == 1)
            ShowFlick();
        Raycasting.instance.PlaceDustbin();
    }

    public void OnRemoveClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        Raycasting.instance.RemoveDustbin();
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
        if (CoinsManager.Instance.tutorial == 1)
        {
            StopHandOnMenu();
            StartCoroutine(ShowMenuHand());
        }
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

    public IEnumerator ShowMenuHand()
    {
        menuHand.SetActive(true);

        yield return new WaitForSecondsRealtime(2.0f);

        menuHand.SetActive(false);
        CoinsManager.Instance.tutorial = 0;
    }

    public void OnExitGame()
    {
        AudioManager.Instance.uiClickSource.Play();
        CoinsManager.Instance.Save();
        AudioManager.Instance.OnMainScreenLoad();
        CoinsManager.Instance.tutorial = 0;
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

    public void OnSliderYChanged(int value)
    {
        CoinsManager.Instance.sensitivity = value;
        ySens.text = value.ToString();
        CoinsManager.Instance.Save();
    }

    public void OnReload()
    {
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        AudioManager.Instance.uiClickSource.Play();
        if(CoinsManager.Instance.tutorial == 1)
            CoinsManager.Instance.tutorial = 0;
        SceneManager.LoadScene(5);
    }

    public void OnMediumClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        if (CoinsManager.Instance.tutorial == 1)
            CoinsManager.Instance.tutorial = 0;
        SceneManager.LoadScene(2);
    }

    public void OnEasyClick()
    {
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(4);
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
        if (CoinsManager.Instance.tutorial == 1)
            StopHandOnQuestionMark();
    }

    public void StopHandOnQuestionMark()
    {
        HandPointingQuestionMark.SetActive(false);
        StartHandOnMenu();
    }

    public void StartHandOnQuestionMark()
    {
        HandPointingQuestionMark.SetActive(true);
    }

    public void StopHandOnMenu()
    {
        HandPointingOnMenu.SetActive(false);
    }

    public void StartHandOnMenu()
    {
        HandPointingOnMenu.SetActive(true);
    }

    public void ShowTutorial()
    {
        mainPanel.gameObject.SetActive(false);
        panelOpen.gameObject.SetActive(false);
        tutorial.SetActive(true);
    }

    public void OnPlayGame()
    {
        tutorial.SetActive(false);
        mainPanel.gameObject.SetActive(true);
        panelOpen.gameObject.SetActive(true);
        arPlaneManager.enabled = true;
        ShowPhone();
    }

    public void ShowHand()
    {
        hand.SetActive(true);
    }

    public void StopHand()
    {
        hand.SetActive(false);
    }

    public void ShowHandInverted()
    {
        handInverted.SetActive(true);
    }

    public void StopHandInverted()
    {
        handInverted.SetActive(false);
    }

    public void ShowFlick()
    {
        flick.SetActive(true);
    }

    public void StopFlick()
    {
        flick.SetActive(false);
/*        CoinsManager.Instance.tutorial = 0;*/
        CoinsManager.Instance.Save();
    }

    public void ShowPhone()
    { 
        phone.SetActive(true);
    }

    public void StopPhone()
    {
        phone.SetActive(false);
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
        {
            CoinsManager.Instance.volume = 1;
            muteButton.sprite = mute;
        }
        else
        {
            CoinsManager.Instance.volume = 0;
            muteButton.sprite = unmute;
        }

        CoinsManager.Instance.SaveVolume();
        CoinsManager.Instance.LoadVolume();
        audioSlider.value = CoinsManager.Instance.volume;
    }
}
