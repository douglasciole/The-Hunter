using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFunctions : MonoBehaviour {

    public void emmitInterfaceButtonAudio()
    {
        FindObjectOfType<AudioManager>().Play("interfaceButton");
    }
	
}
