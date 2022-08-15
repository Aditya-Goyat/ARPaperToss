using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerMainScreen : MonoBehaviour
{
    public GameObject mainPanel, settingsPanel;
    [SerializeField] TMP_InputField ySens;
    [SerializeField] Slider ySlider;
    [SerializeField] Image sensImage;
    [SerializeField] Sprite lowSens, medSens, highSens;

    public void OnPlayLives()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayTimer()
    {
        SceneManager.LoadScene(2);
        AudioManager.Instance.OnPlayGame();
        //AudioManager.Instance.StartCoroutine("OnPlayGame");
    }

    public void OnShopClick()
    {
        SceneManager.LoadScene(3);
    }

    public void OnHardClick()
    {
        SceneManager.LoadScene(4);
    }

    public void OnSettingsClick()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        UpdateSensitivity();
    }

    public void OnBackClick()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnSliderYChanged(float value)
    {
        CoinsManager.Instance.sensitivity = Mathf.RoundToInt(value);
        ySens.text = value.ToString();
        CoinsManager.Instance.Save();

        if (value >= 0 && value <= 45)
            sensImage.sprite = lowSens;
        else if (value > 45 && value <= 90)
            sensImage.sprite = medSens;
        else if(value > 90 && value <= 130)
            sensImage.sprite = highSens;
    }

    private void UpdateSensitivity()
    {
        if (CoinsManager.Instance.sensitivity >= 0 && CoinsManager.Instance.sensitivity <= 45)
            sensImage.sprite = lowSens;
        else if (CoinsManager.Instance.sensitivity > 45 && CoinsManager.Instance.sensitivity <= 90)
            sensImage.sprite = medSens;
        else if (CoinsManager.Instance.sensitivity > 90 && CoinsManager.Instance.sensitivity <= 130)
            sensImage.sprite = highSens;

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

            if (num >= 0 && num <= 45)
                sensImage.sprite = lowSens;
            else if (num > 45 && num <= 90)
                sensImage.sprite = medSens;
            else if (num > 90 && num <= 130)
                sensImage.sprite = highSens;

            CoinsManager.Instance.sensitivity = n;
            ySlider.value = n;
            ySens.text = n.ToString();
        }
    }
}
