using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCanvas : MonoBehaviour
{

    public GameObject firstCan;
    public GameObject startCan;
    public GameObject settingsCan;
    public AudioClip[] clips;
    public AudioSource audioSource;


    public void TurnOnStartMenu()
    {
        audioSource.clip = GetRandomClip();
        audioSource.Play();
        firstCan.SetActive(false);
        startCan.SetActive(true);
        settingsCan.SetActive(false);
    }

    public void TurnOnOptionsMenu()
    {
        audioSource.clip = GetRandomClip();
        audioSource.Play();
        firstCan.SetActive(false);
        startCan.SetActive(false);
        settingsCan.SetActive(true);
    }

    public void BackButton()
    {
        audioSource.clip = GetRandomClip();
        audioSource.Play();
        firstCan.SetActive(true);
        startCan.SetActive(false);
        settingsCan.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(01);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }


}
