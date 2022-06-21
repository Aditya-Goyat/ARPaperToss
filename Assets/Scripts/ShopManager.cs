using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public int dustbinIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("dustbinIndex"))
        {
            dustbinIndex = PlayerPrefs.GetInt("dustbinIndex");
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("dustbinIndex", dustbinIndex);
    }

    public bool TryToBuy(int price)
    {
        if(price <= CoinsManager.Instance.Coins)
            return true;
        else
            return false;
    }
}
