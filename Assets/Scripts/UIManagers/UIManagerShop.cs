using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerShop : MonoBehaviour
{
    //[SerializeField] TMP_Text noFundsText;
    int[] price = new int[6] {0, 1000, 2000, 3000, 5000, 10000};
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
                ShopManager.Instance.dustbinIndex = index;
                CoinsManager.Instance.isUnlocked[index] = 1;
                ShopManager.Instance.Save();
            }
            else
            {
                NotEnoughFunds();
            }
        }
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);
    }

    public void NotEnoughFunds()
    {

    }
}
