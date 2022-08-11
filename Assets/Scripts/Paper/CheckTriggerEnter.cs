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
            StreakController.ResetTimer();
            Destroy(this.gameObject, PaperManager.Instance.resetBallAfterSeconds);
            ScoreManager.Instance.Score++;
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UIManagerLivesMode.Instance.UpdateScore();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 6 || SceneManager.GetActiveScene().buildIndex == 5)
            {
                if(Vector3.Distance(Camera.main.transform.position, transform.position) >= 3f && Vector3.Distance(Camera.main.transform.position, transform.position) <= 5f)
                    CoinsManager.Instance.Coins += 10000;
                else if (Vector3.Distance(Camera.main.transform.position, transform.position) > 5f)
                    CoinsManager.Instance.Coins += 20000;
                UIManagerChallengeMode.Instance.UpdateScore();
                UIManagerChallengeMode.Instance.UpdateCoins();
                if(SceneManager.GetActiveScene().buildIndex == 2)
                    Wind.Instance.ResetWind();
            }
        }
    }
}
