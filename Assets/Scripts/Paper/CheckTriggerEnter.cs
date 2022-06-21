using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckTriggerEnter : MonoBehaviour
{
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Destroy(this.gameObject, PaperManager.Instance.resetBallAfterSeconds);
            ScoreManager.Instance.Score++;
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UIManagerLivesMode.Instance.UpdateScore();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
            {
                CoinsManager.Instance.Coins += 5;
                UIManagerChallengeMode.Instance.UpdateScore();
                UIManagerChallengeMode.Instance.UpdateCoins();
                Wind.Instance.ResetWind();
            }
        }
    }
}
