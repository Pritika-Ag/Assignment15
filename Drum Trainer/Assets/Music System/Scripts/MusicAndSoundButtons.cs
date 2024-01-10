using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAndSoundButtons : MonoBehaviour
{
    public GameObject soundX;
    public GameObject musicX;
    public MusicSet musicSet;
    private void Start()
    {
        musicSet = FindObjectOfType<MusicSet>();
    }

    public void MusicMuteButton()
    {
        musicSet.MuteMusic();
        if (musicSet.musicOn)
        {
            musicX.SetActive(true);
        }
       else if (!musicSet.musicOn)
        {
            musicX.SetActive(false);
        }

    }

    public void MusicSoundButton()
    {
        musicSet.MuteSound();
        if (musicSet.soundOn)
        {
            soundX.SetActive(true);
        }
        else if (!musicSet.soundOn)
        {
            soundX.SetActive(false);
        }

    }
}
