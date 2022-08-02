using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreakController : MonoBehaviour
{
    float maxTimer = 5f;
    float timerLeft;
    int streakAmount = 0;
    [SerializeField] TMP_Text streakAmountText;
    private static StreakController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerLeft = 0f;
        streakAmountText.text = streakAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerLeft <= 0f)
        {
            streakAmount = 0;
            streakAmountText.text = streakAmount.ToString();
        }

        timerLeft -= Time.deltaTime;

        if (timerLeft <= 0f)
        {
            timerLeft = 0f;
        }
    }

    public static void ResetTimer()
    {
        instance.streakAmount++;
        instance.streakAmountText.text = instance.streakAmount.ToString();
        instance.timerLeft = instance.maxTimer;
    }
}
