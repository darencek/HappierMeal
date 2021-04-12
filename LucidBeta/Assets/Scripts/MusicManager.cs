using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource dayMusic;
    public AudioSource sleepMusic;

    public GameObject levelUpSound;
    public GameObject levelUpSound2;

    bool levelUpPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.musicManager = this;
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
