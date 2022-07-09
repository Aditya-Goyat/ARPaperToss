using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;
    int coinsAmount = 0;
    public int[] isUnlocked;
    public int Coins { get { return coinsAmount; } set { coinsAmount = value; } }

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
        for(int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("isUnlocked" + i, isUnlocked[i]);
        }
    }
}
