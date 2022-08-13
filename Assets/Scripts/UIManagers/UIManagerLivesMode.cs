using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManagerLivesMode : MonoBehaviour
{
    public static UIManagerLivesMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;
    [SerializeField] CanvasGroup panelClose;
    [SerializeField] CanvasGroup panelOpen;
    [SerializeField] CanvasGroup mainPanel;
    [SerializeField] RectTransform SidePanel;
    public bool isPanelOpen = false;

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

        UpdateSensitivity();
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
        SceneManager.LoadScene(0);
    }

    public void UpdateScore()
    {
        scoreText.text = ScoreManager.Instance.Score.ToString();
    }

    public void OnSliderYChanged(int value)
    {
        CoinsManager.Instance.sensitivity = value;
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
}
