using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StreakController : MonoBehaviour
{
    public float maxTimer = 5f;
    float timerLeft;
    int streakAmount = 0;
    [SerializeField] Slider timerSlider;
    [SerializeField] Sprite _1x;
    [SerializeField] Sprite _2x;
    [SerializeField] Sprite _3x;
    [SerializeField] Image streakAmountImage;
    [SerializeField] Image streakBg;
    [SerializeField] Image streakText;
    private static StreakController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerLeft = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerLeft <= 0f)
        {
            streakBg.gameObject.SetActive(false);
            streakText.gameObject.SetActive(false);
            streakAmountImage.gameObject.SetActive(false);
            timerSlider.gameObject.SetActive(false);
            streakAmount = 0;
        }

        timerLeft -= Time.deltaTime;
        timerSlider.value = timerLeft;

        if (timerLeft <= 0f)
        {
            timerLeft = 0f;
        }
    }

    public static void ResetTimer()
    {
        instance.streakBg.gameObject.SetActive(true);
        instance.streakText.gameObject.SetActive(true);
        instance.timerSlider.gameObject.SetActive(true);
        instance.streakAmountImage.gameObject.SetActive(true);
        instance.streakAmount++;
        Debug.Log(CheckTriggerEnter.GetDistance());
        if (instance.streakAmount == 3 && CheckTriggerEnter.GetDistance() > 3f)
        {
            Debug.Log(2);
            CoinsManager.Instance.Heart += 10;
        }
        instance.streakAmount = instance.streakAmount % 3;
        switch (instance.streakAmount)
        {
            case 1:
                instance.streakAmountImage.sprite = instance._1x;
                break;
            case 2:
                instance.streakAmountImage.sprite = instance._2x;
                break;
            case 3:
                instance.streakAmountImage.sprite = instance._3x;
                break;
            case 0:
                instance.streakBg.gameObject.SetActive(false);
                instance.streakText.gameObject.SetActive(false);
                instance.streakAmountImage.gameObject.SetActive(false);
                instance.timerSlider.gameObject.SetActive(false);
                break;
        }
        instance.timerLeft = instance.maxTimer;
    }
}
