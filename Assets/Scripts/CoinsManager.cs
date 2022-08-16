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
    public float volume = 1f;
    public int tutorial = 1;
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
            tutorial = PlayerPrefs.GetInt("tutorial");
            volume = PlayerPrefs.GetInt("volume");
            LoadVolume();
        }
        else
        {
            Save();
            SaveVolume();
            LoadVolume();
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
        PlayerPrefs.SetInt("tutorial", tutorial);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void LoadVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
