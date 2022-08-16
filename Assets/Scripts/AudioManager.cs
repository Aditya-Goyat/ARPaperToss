using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource, audioSourceTransition, uiClickSource, timerAudioSource, ballInDustbin;
    public AudioClip mainMenuLoop, gameLoop, transition, transition2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = mainMenuLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void OnPlayGame()
    {
        audioSource.Stop();

        audioSourceTransition.PlayScheduled(AudioSettings.dspTime);

        audioSource.clip = gameLoop;
        audioSource.loop = true;
        audioSource.PlayScheduled(AudioSettings.dspTime + transition.length);
    }

    public void OnMainScreenLoad()
    {
        audioSource.Stop();

        audioSourceTransition.PlayScheduled(AudioSettings.dspTime);

        audioSource.clip = mainMenuLoop;
        audioSource.loop = true;
        audioSource.PlayScheduled(AudioSettings.dspTime + transition.length);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
