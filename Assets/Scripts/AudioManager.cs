using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource, audioSourceTransition;
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

/*    public IEnumerator OnMainScreenLoad()
    {
        audioSource.Stop();

        audioSource.PlayOneShot(transition);
        yield return new WaitUntil(() => audioSource.isPlaying == false);

        audioSource.clip = mainMenuLoop;
        audioSource.loop = true;
        audioSource.Play();
    }*/

/*    public IEnumerator OnPlayGame()
    {
        audioSource.Stop();

        double clipDuration = (double)audioSource.clip.samples / audioSource.clip.frequency;

        audioSource.PlayOneShot(transition);
        yield return new WaitUntil(() => audioSource.isPlaying == false);

        audioSource.clip = gameLoop;
        audioSource.loop = true;
        audioSource.Play();
    }*/

    // Update is called once per frame
    void Update()
    {
      
    }
}
