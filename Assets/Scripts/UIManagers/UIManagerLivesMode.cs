using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerLivesMode : MonoBehaviour
{
    public static UIManagerLivesMode Instance;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject settings;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;

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
        SceneManager.LoadScene(0);
    }

    public void UpdateScore()
    {
        scoreText.text = ScoreManager.Instance.Score.ToString();
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
