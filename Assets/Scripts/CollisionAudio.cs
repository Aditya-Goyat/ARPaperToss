using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    public static CollisionAudio instance;
    public AudioSource success;
    public ParticleSystem fireworks, fireworksInstance = null;
    public GameObject star, coin;
    public Animator starAnimator, coinAnimator;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            StartCoroutine(PlayStarAnimation());
    }

    public void PlayFireworks()
    {
        success.Play();
        if(fireworksInstance == null)
            fireworksInstance = Instantiate(fireworks, transform.position, Quaternion.identity, transform);
        fireworksInstance.Play();
    }

    public void StarCoroutineStar()
    {
        StartCoroutine(PlayStarAnimation());
    }

    public IEnumerator PlayStarAnimation()
    {
        starAnimator.SetBool("isStreak", true);
        yield return new WaitForEndOfFrame();
        starAnimator.SetBool("isStreak", false);
    }

    public void StarCoroutineCoin()
    {
        StartCoroutine(PlayCoinAnimation());
    }

    public IEnumerator PlayCoinAnimation()
    {
        coinAnimator.SetBool("isIn", true);
        yield return new WaitForEndOfFrame();
        coinAnimator.SetBool("isIn", false);
    }

    // Start is called before the first frame update
    public void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.ballInDustbin.Play();
    }
}