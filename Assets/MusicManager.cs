using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource backgroundMusicIntro;
    public AudioSource gameWinningMusic;
    public AudioSource gameLosingMusic;

    bool playingBackgroundMusic = false;
    
    void Awake()
    {
        GameObject menuBackgroundMusicAlreadyPlaying = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
        if (menuBackgroundMusicAlreadyPlaying)
        {
            Destroy(menuBackgroundMusicAlreadyPlaying);
        }
    }

    void Start()
    {
        GameObject menuBackgroundMusicAlreadyPlaying = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
        if (menuBackgroundMusicAlreadyPlaying)
        {
            Destroy(menuBackgroundMusicAlreadyPlaying);
        }




        // play intro
        backgroundMusicIntro.time = 0.25f;
        backgroundMusicIntro.loop = false;
        backgroundMusicIntro.Play();
    }

    void Update()
    {
        if (!backgroundMusicIntro.isPlaying && !playingBackgroundMusic)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
            playingBackgroundMusic = true;
        }
    }

    public void playGameWinningMusic()
    {
        backgroundMusic.Stop();
        gameWinningMusic.loop = false;
        gameWinningMusic.Play();
    }

    public void playGameLosingMusic()
    {
        backgroundMusic.Stop();
        gameLosingMusic.loop = false;
        gameLosingMusic.Play();
    }

    public void startMenuMusicBackUp()
    {
        gameWinningMusic.Stop();
        gameLosingMusic.Stop();
    }
}
