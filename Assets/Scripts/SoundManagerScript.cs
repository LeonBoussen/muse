using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManagerScript : MonoBehaviour
{
    private static SoundManagerScript SoundManager;

    public AudioClip[] EnemySounds;
    public AudioClip[] PlayerSounds;
    public AudioClip[] BGM;
    public AudioClip[] SFX;

    [Range(0.1f, 1f)]
    public float BGMVolume;
    [Range(0.1f, 1f)]
    public float SFXVolume;

    public bool GameStarted = false;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public bool PlayedMusic = false;
    
    private void Start()
    {
        //StartCoroutine(MainMenuMusic());
        //StartCoroutine(BGM_Music());
    }

    void Update()
    {
        bgmSource.volume = BGMVolume;
        sfxSource.volume = SFXVolume;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!PlayedMusic)
            {
                StartCoroutine(MainMenuMusic());
                PlayedMusic = true;
            }
        }
        else
        {
            if (!PlayedMusic)
            {
                StartCoroutine(BGM_Music());
                PlayedMusic = true;
            } 
        }
    }
    public void StopMusic()
    {
        bgmSource.Stop();

    }
    public void PlayPlayerSFX(AudioSource playersource, int position) // 0 = walk | 1 = attack | 2 = death | 3 = jump | 4 = camera
    {
        playersource.clip = PlayerSounds[position];
        playersource.volume = SFXVolume;
        playersource.PlayOneShot(playersource.clip);
    }
    public void PlayEnemySFX(AudioSource enemysource, int position) // 0 = hit | 1 = attack | 2 = death | 3 = walk | 4 = idle |5 = morph
    {
        enemysource.clip = EnemySounds[position];
        enemysource.volume = SFXVolume;
        enemysource.PlayOneShot(enemysource.clip);
    }
    public void PlaySFX(int pos) // 0 = win sound | 1 = death sound | 2 = UI button hover | 3 = UI button click
    {
        sfxSource.clip = SFX[pos];
        sfxSource.volume = SFXVolume;
        sfxSource.PlayOneShot(sfxSource.clip);
    }
   
    public IEnumerator MainMenuMusic()
    {
        while (true)
        {
            bgmSource.volume = BGMVolume;
            bgmSource.PlayOneShot(BGM[0]);
            yield return new WaitForSecondsRealtime(BGM[0].length);
        }
    }
    public IEnumerator BGM_Music()
    {
        while (true)
        {
            bgmSource.volume = BGMVolume;
            bgmSource.PlayOneShot(BGM[1]);
            yield return new WaitForSecondsRealtime(BGM[1].length);
        }
    }
    private void Awake()
    {
        if (!SoundManager)
        {
            DontDestroyOnLoad(gameObject);
            SoundManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
