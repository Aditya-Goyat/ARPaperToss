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
        if (price > 10000)
        {
            if (price <= (CoinsManager.Instance.Heart / 100))
                return true;
            else
                return false;
        }
        else
        {
            if (price <= CoinsManager.Instance.Coins)
                return true;
            else
                return false;
        }
    }
}
