using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerShop : MonoBehaviour
{
    //[SerializeField] TMP_Text noFundsText;
    public void OnDustbinChoose(int index)//, int price)
    {
/*        if (ShopManager.Instance.TryToBuy(price))
        {
            CoinsManager.Instance.Coins -= price;*/
            ShopManager.Instance.dustbinIndex = index;
            ShopManager.Instance.Save();
        //}
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);
    }
}
