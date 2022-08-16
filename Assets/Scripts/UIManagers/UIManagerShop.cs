using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManagerShop : MonoBehaviour
{
    //[SerializeField] TMP_Text noFundsText;
    int[] price = new int[6] {0, 100, 500, 1000, 10000, 20000};
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
    public RectTransform notEnoughFundsCoins, notEnoughFundsStars;
    private Sequence sequence;

    public void Start()
    {
        UpdateCoinText();
        SetBuyButtons();

        if(Debug.isDebugBuild)
            cheaterButton.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            PlayNotEnoughFundsStars();
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
        if (CoinsManager.Instance.isUnlocked[index] == 1)
        {
            AudioManager.Instance.uiClickSource.Play();
            ShopManager.Instance.dustbinIndex = index;
            SetBuyButtons();
            ShopManager.Instance.Save();
        }
        else
        {
            if(ShopManager.Instance.TryToBuy(price[index]))
            {
                AudioManager.Instance.uiClickSource.Play();
                audioSource.PlayOneShot(buySound);
                if(price[index] >= 10000)
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
                NotEnoughFunds(index);
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
        AudioManager.Instance.uiClickSource.Play();
        SceneManager.LoadScene(0);
    }

    public void NotEnoughFunds(int index)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(notEnoughBalanceSound, 0.5f);

        if (price[index] >= 10000)
            PlayNotEnoughFundsStars();
        else
            PlayNotEnougFundsCoins();
    }

    public void OnCheaterClick()
    {
        CoinsManager.Instance.Coins += 10000;
        CoinsManager.Instance.Heart += 10000;

        CoinsManager.Instance.Save();
        UpdateCoinText();
    }

    public void PlayNotEnoughFundsStars()
    {
        sequence = DOTween.Sequence();
        sequence.Append(notEnoughFundsStars.gameObject.GetComponent<CanvasGroup>().DOFade(1.0f, 0.25f));
        sequence.Append(notEnoughFundsStars.DOAnchorPos(new Vector2(notEnoughFundsStars.anchoredPosition.x, (notEnoughFundsStars.anchoredPosition.y + 30f)), 0.5f, false));
        sequence.Append(notEnoughFundsStars.gameObject.GetComponent<CanvasGroup>().DOFade(0.0f, 0.25f));
        sequence.Append(notEnoughFundsStars.DOAnchorPos(new Vector2(notEnoughFundsStars.anchoredPosition.x, (notEnoughFundsStars.anchoredPosition.y)), 0.0f, false));
    }

    public void PlayNotEnougFundsCoins()
    {
        sequence = DOTween.Sequence();
        sequence.Append(notEnoughFundsCoins.gameObject.GetComponent<CanvasGroup>().DOFade(1.0f, 0.25f));
        sequence.Append(notEnoughFundsCoins.DOAnchorPos(new Vector2(notEnoughFundsCoins.anchoredPosition.x, (notEnoughFundsCoins.anchoredPosition.y + 30f)), 0.5f, false));
        sequence.Append(notEnoughFundsCoins.gameObject.GetComponent<CanvasGroup>().DOFade(0.0f, 0.25f));
        sequence.Append(notEnoughFundsCoins.DOAnchorPos(new Vector2(notEnoughFundsCoins.anchoredPosition.x, (notEnoughFundsCoins.anchoredPosition.y)), 0.0f, false));
    }
}
