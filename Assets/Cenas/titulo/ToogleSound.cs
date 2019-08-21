using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToogleSound : MonoBehaviour {

    public AudioMixer audio;
    Image configBtn;
    Sprite audioIcon;
    public Sprite noAudioIcon;
    int hasAudio = 1;

	// Use this for initialization
	void Start () {
        configBtn = GetComponent<Image>();
        audioIcon = configBtn.sprite;

        if (PlayerPrefs.GetInt("hasEverStarted") != 1)
        {
            PlayerPrefs.SetInt("hasEverStarted", 1);
            PlayerPrefs.SetInt("hasAudio", 1);
        }
        
        hasAudio = PlayerPrefs.GetInt("hasAudio");
        updateIcon();
        
    }

    void updateIcon()
    {
        if (hasAudio == 0)
        {
            configBtn.sprite = noAudioIcon;
            audio.SetFloat("Volume", -80f);
        }
        else
        {
            configBtn.sprite = audioIcon;
            audio.SetFloat("Volume", 0f);
        }
    }
	
    public void toggle()
    {
        FindObjectOfType<AudioManager>().Play("interfaceButton");
        if (hasAudio == 0)
        {
            hasAudio = 1;
        }
        else
        {
            hasAudio = 0;
        }

        PlayerPrefs.SetInt("hasAudio", hasAudio);
        updateIcon();
    }

}
