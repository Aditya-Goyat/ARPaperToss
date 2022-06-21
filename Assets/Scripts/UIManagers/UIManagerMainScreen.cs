using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerMainScreen : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText;

    private void Start()
    {
        coinsText.text = CoinsManager.Instance.Coins.ToString();
    }

    public void OnPlayLives()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayTimer()
    {
        SceneManager.LoadScene(2);
    }

    public void OnShopClick()
    {
        SceneManager.LoadScene(3);
    }

    public void OnHardClick()
    {
        SceneManager.LoadScene(4);
    }
}
