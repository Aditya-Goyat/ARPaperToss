using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManagerShop : MonoBehaviour
{
    //[SerializeField] TMP_Text noFundsText;
    int[] price = new int[6] {0, 1000, 3000, 7000, 20000, 100000};
    public Image[] buyButtons = new Image[6];
    public Sprite buy, equip;
    [SerializeField] TMP_Text coinsAmount;
    [SerializeField] TMP_Text heartAmount;

    public void Start()
    {
        UpdateCoinText();
        SetBuyButtons();
    }

    public void SetBuyButtons()
    {
        Debug.Log(1);
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
            SetBuyButtons();
            ShopManager.Instance.Save();
        }
        else
        {
            if(ShopManager.Instance.TryToBuy(price[index]))
            {
                if(price[index] > 10000)
                    CoinsManager.Instance.Heart -= (price[index] / 100);
                else
                    CoinsManager.Instance.Coins -= price[index];

                CoinsManager.Instance.isUnlocked[index] = 1;
                CoinsManager.Instance.Save();
                SetBuyButtons();
                ShopManager.Instance.Save();
                UpdateCoinText();
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
