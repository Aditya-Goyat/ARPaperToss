using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if (GetComponent<Collider>().isTrigger == true)
                return;
            Destroy(this.gameObject, PaperManager.Instance.resetBallAfterSeconds);
            ScoreManager.Instance.Score++;
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UIManagerLivesMode.Instance.UpdateScore();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 5)
            {
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    if (Vector3.Distance(Camera.main.transform.position, transform.position) >= 2f && Vector3.Distance(Camera.main.transform.position, transform.position) <= 3f)
                        CoinsManager.Instance.Coins += 10;
                    else if (Vector3.Distance(Camera.main.transform.position, transform.position) > 3f)
                        CoinsManager.Instance.Coins += 20;
                }else if(SceneManager.GetActiveScene().buildIndex == 5)
                {
                    if (Vector3.Distance(Camera.main.transform.position, transform.position) >= 2f && Vector3.Distance(Camera.main.transform.position, transform.position) <= 3f)
                        CoinsManager.Instance.Coins += 20;
                    else if (Vector3.Distance(Camera.main.transform.position, transform.position) > 3f)
                        CoinsManager.Instance.Coins += 30;
                }

                if(Vector3.Distance(Camera.main.transform.position, transform.position) >= 2f)
                    StreakController.ResetTimer(Vector3.Distance(Camera.main.transform.position, transform.position));

                if(StreakController.GetStreak() > 2)
                    CollisionAudio.instance.StarCoroutineCoin();

                UIManagerChallengeMode.Instance.UpdateScore();
                UIManagerChallengeMode.Instance.UpdateCoins();

                if (SceneManager.GetActiveScene().buildIndex == 2)
                    Wind.Instance.ResetWind();
                
            }else if(SceneManager.GetActiveScene().buildIndex == 4)
            {
                if (Vector3.Distance(Camera.main.transform.position, transform.position) >= 2f && Vector3.Distance(Camera.main.transform.position, transform.position) <= 3f)
                    CoinsManager.Instance.Coins += 5;
                else if (Vector3.Distance(Camera.main.transform.position, transform.position) > 3f)
                    CoinsManager.Instance.Coins += 10;

                if (Vector3.Distance(Camera.main.transform.position, transform.position) >= 2f)
                    StreakController.ResetTimer(Vector3.Distance(Camera.main.transform.position, transform.position));

                if (StreakController.GetStreak() > 2)
                    CollisionAudio.instance.StarCoroutineCoin();

                UIManagerEasyMode.Instance.UpdateCoins();
                UIManagerEasyMode.Instance.UpdateScore();
            }
        }
    }
}
