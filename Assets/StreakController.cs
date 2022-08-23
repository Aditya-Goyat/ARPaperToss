using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip streak01;
    [SerializeField] AudioClip streak02;
    private static StreakController instance;

    private void Awake()
    {
        instance = this;
    }

    public static int GetStreak()
    {
        return instance.streakAmount;
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

    public static void ResetTimer(float distance)
    {
        instance.streakBg.gameObject.SetActive(true);
        instance.streakText.gameObject.SetActive(true);
        instance.timerSlider.gameObject.SetActive(true);
        instance.streakAmountImage.gameObject.SetActive(true);
        instance.streakAmount++;
        if (instance.streakAmount == 1)
            instance.audioSource.PlayOneShot(instance.streak01);
        else if (instance.streakAmount == 2)
            instance.audioSource.PlayOneShot(instance.streak02);
        if (instance.streakAmount == 3)
        {
            CollisionAudio.instance.PlayFireworks();
            CollisionAudio.instance.StarCoroutineStar();
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (distance >= 2f && distance < 3f)
                    CoinsManager.Instance.Heart += 10;
                else if (distance >= 3f)
                    CoinsManager.Instance.Heart += 30;
            }else if(SceneManager.GetActiveScene().buildIndex == 4)
            {
                if (distance >= 2f && distance < 3f)
                    CoinsManager.Instance.Heart += 5;
                else if (distance >= 3f)
                    CoinsManager.Instance.Heart += 10;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                if (distance >= 2f && distance < 3f)
                    CoinsManager.Instance.Heart += 50;
                else if (distance >= 3f)
                    CoinsManager.Instance.Heart += 100;
            }
        }
        else
        {
            CollisionAudio.instance.StarCoroutineCoin();
        }
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
        instance.streakAmount = instance.streakAmount % 3;
        instance.timerLeft = instance.maxTimer;
    }
}
