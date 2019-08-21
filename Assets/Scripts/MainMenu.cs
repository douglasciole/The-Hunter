using System;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("mainMenu");
    }
}
