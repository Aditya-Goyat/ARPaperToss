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
    public Image[] icons = new Image[6];
    public Image[] priceIcons = new Image[6];
    public TMP_Text[] priceAmount = new TMP_Text[6];
    public Sprite buy, equip, tick;
    [SerializeField] TMP_Text coinsAmount;
    [SerializeField] TMP_Text heartAmount;
    [SerializeField] GameObject cheaterButton;
    public AudioSource audioSource;
    public AudioClip buySound, notEnoughBalanceSound;

    public void Start()
    {
        UpdateCoinText();
        SetBuyButtons();

        if(Debug.isDebugBuild)
            cheaterButton.SetActive(true);
    }

    public void SetBuyButtons()
    {
        for(int i = 1; i < 6; i++)
        {
            if (CoinsManager.Instance.isUnlocked[i] == 1)
            {
                priceIcons[i].gameObject.SetActive(false);
                priceAmount[i].gameObject.SetActive(false);
                icons[i].rectTransform.sizeDelta = new Vector2(200f, 200f);
            }
        }

        for(int i = 0; i < 6; i++)
        {
            if (CoinsManager.Instance.isUnlocked[i] == 1)
                buyButtons[i].sprite = equip;
            if (ShopManager.Instance.dustbinIndex == i)
                buyButtons[i].sprite = tick;
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
                audioSource.PlayOneShot(buySound);
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
        audioSource.PlayOneShot(notEnoughBalanceSound);
    }

    public void OnCheaterClick()
    {
        CoinsManager.Instance.Coins += 1000000;
        CoinsManager.Instance.Heart += 1000000;

        CoinsManager.Instance.Save();
        UpdateCoinText();
    }
}
