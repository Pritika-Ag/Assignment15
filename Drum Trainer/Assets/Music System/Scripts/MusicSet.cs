using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSet : MonoBehaviour
{
    public AudioMixer masterMixer;
    public bool musicMuteOn = false;
    public bool soundMuteOn = false;
    float recordCurrentMusicLevel = 0;
    float recordCurrentSoundLevel = 0;
    public bool musicOn = true;
    public bool soundOn = true;

    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("SoundVol", soundLevel);
        recordCurrentSoundLevel = soundLevel;
    }
    public void SetMusic(float musicLevel)
    {
        masterMixer.SetFloat("MusicVol", musicLevel);
        recordCurrentMusicLevel = musicLevel;
    }

    public void MuteMusic()
    {
        musicMuteOn = !musicMuteOn;
        if (musicMuteOn)
        {
            masterMixer.SetFloat("MusicVol", -80);
            musicOn = false;
        }
        else if (!musicMuteOn)
        {
            masterMixer.SetFloat("MusicVol", recordCurrentMusicLevel);
            musicOn = true;
        }
    }

    public void MuteSound()
    {
        soundMuteOn = !soundMuteOn;
        if (soundMuteOn)
        {
            masterMixer.SetFloat("SoundVol", -80);
            soundOn = false;
        }
        else if (!soundMuteOn)
        {
            masterMixer.SetFloat("SoundVol", recordCurrentSoundLevel);
            soundOn = true;
        }
    }

}
