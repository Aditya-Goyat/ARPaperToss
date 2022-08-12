using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManagerShop : MonoBehaviour
{
    //[SerializeField] TMP_Text noFundsText;
    int[] price = new int[6] {0, 1000, 2000, 3000, 5000, 10000};
    public Image[] buyButtons = new Image[6];
    public Sprite buy, equip;
    [SerializeField] TMP_Text coinsAmount;
    [SerializeField] TMP_Text heartAmount;

    public void Start()
    {
        UpdateCoinText();
    }

    public void SetBuyButtons()
    {
        for(int i = 0; i < 6; i++)
        {
            if (CoinsManager.Instance.isUnlocked[i] == 1)
                buyButtons[i].sprite = equip;
        }
    }

    public void OnDustbinChoose(int index)
    {
        if(CoinsManager.Instance.isUnlocked[index] == 1)
        {
            ShopManager.Instance.dustbinIndex = index;
            ShopManager.Instance.Save();
        }
        else
        {
            if(ShopManager.Instance.TryToBuy(price[index]))
            {
                CoinsManager.Instance.Coins -= price[index];
                if(CoinsManager.Instance.isUnlocked[index] == 1)
                    ShopManager.Instance.dustbinIndex = index;
                CoinsManager.Instance.isUnlocked[index] = 1;
                SetBuyButtons();
                ShopManager.Instance.Save();
                UpdateCoinText();
                CoinsManager.Instance.Save();
            }
            else
            {
                NotEnoughFunds();
            }
        }
    }

    public void UpdateCoinText()
    {
        coinsAmount.text = CoinsManager.Instance.Coins.ToString();
        heartAmount.text = CoinsManager.Instance.Heart.ToString();
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);
    }

    public void NotEnoughFunds()
    {

    }
}
