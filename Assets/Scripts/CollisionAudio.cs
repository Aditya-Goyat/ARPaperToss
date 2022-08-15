using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    public static CollisionAudio instance;
    public AudioSource success;
    public ParticleSystem fireworks, fireworksInstance = null;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            PlayFireworks();
    }

    public void PlayFireworks()
    {
        success.Play();
        if(fireworksInstance == null)
            fireworksInstance = Instantiate(fireworks, transform.position, Quaternion.identity, transform);
        fireworksInstance.Play();
    }

    // Start is called before the first frame update
    public void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.ballInDustbin.Play();
    }
}