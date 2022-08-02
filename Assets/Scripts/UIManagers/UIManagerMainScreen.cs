using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerMainScreen : MonoBehaviour
{
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
