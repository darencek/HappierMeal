using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource dayMusic;
    public AudioSource sleepMusic;

    public GameObject levelUpSound;
    public GameObject levelUpSound2;

    AudioSource audioPlayer;

    public AudioClip demolishSound;
    public AudioClip clickSound;
    public AudioClip popSound;
    public AudioClip dinkSound;
    public AudioClip doubleDinkSound;
    public AudioClip jingleSound;
    public AudioClip boomSound;

    bool levelUpPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.musicManager = this;
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelUpPlaying)
        {
            dayMusic.volume = Mathf.MoveTowards(dayMusic.volume, 0f, Time.unscaledDeltaTime * 2f);
            sleepMusic.volume = Mathf.MoveTowards(sleepMusic.volume, 0f, Time.unscaledDeltaTime * 2f);
        }
        else
        {
            if (MainManager.instance.sleepState == 0)
            {
                dayMusic.volume = Mathf.MoveTowards(dayMusic.volume, 0.5f, Time.unscaledDeltaTime);
                sleepMusic.volume = Mathf.MoveTowards(sleepMusic.volume, 0f, Time.unscaledDeltaTime);
            }
            else
            {
                dayMusic.volume = Mathf.MoveTowards(dayMusic.volume, 0f, Time.unscaledDeltaTime);
                sleepMusic.volume = Mathf.MoveTowards(sleepMusic.volume, 1f, Time.unscaledDeltaTime);
            }
        }
    }

    public void PlayDemolishSound()
    {
        audioPlayer.PlayOneShot(demolishSound);
    }

    public void PlayClick()
    {
        audioPlayer.PlayOneShot(clickSound);
    }

    public void PlayPop()
    {
        audioPlayer.PlayOneShot(popSound);
    }
    public void PlayDink()
    {
        audioPlayer.PlayOneShot(dinkSound);
    }
    public void PlayDoubleDink()
    {
        audioPlayer.PlayOneShot(doubleDinkSound);
    }
    public void PlayJingle()
    {
        audioPlayer.PlayOneShot(jingleSound);
    }
    public void PlayBoom()
    {
        audioPlayer.PlayOneShot(boomSound);
    }

    public void PlayLevelUpSound()
    {
        StartCoroutine("CRlevelUpSound");
    }

    IEnumerator CRlevelUpSound()
    {
        levelUpPlaying = true;
        levelUpSound.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        levelUpSound2.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        levelUpSound.SetActive(false);
        levelUpSound2.SetActive(false);
        levelUpPlaying = false;
    }
}
