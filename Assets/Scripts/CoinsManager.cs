using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance;
    int coinsAmount = 0;
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
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("coinsAmount", coinsAmount);
    }
}
