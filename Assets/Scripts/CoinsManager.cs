using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;
    int coinsAmount = 0, heartAmount = 0;
    public int[] isUnlocked;
    public int sensitivity = 65;
    public bool gameOver = false;
    [HideInInspector]
    public bool tutorial = true;
    public int Coins { get { return coinsAmount; } set { coinsAmount = value; } }
    public int Heart { get { return heartAmount; } set { heartAmount = value; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("coinsAmount"))
        {
            coinsAmount = PlayerPrefs.GetInt("coinsAmount");
            heartAmount = PlayerPrefs.GetInt("heartAmount");
            sensitivity = PlayerPrefs.GetInt("sensitivity");
            for(int i = 0; i < 6; i++)
            {
                isUnlocked[i] = PlayerPrefs.GetInt("isUnlocked" + i);
            }
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("coinsAmount", coinsAmount);
        PlayerPrefs.SetInt("heartAmount", heartAmount);
        PlayerPrefs.SetInt("sensitivity", sensitivity);
        for(int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("isUnlocked" + i, isUnlocked[i]);
        }
    }
}
