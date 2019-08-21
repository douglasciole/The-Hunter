using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUnFreeze : MonoBehaviour {

    public float timer = 1;
    float timerCounter;
    bool acted = false;

    public GameObject leaveFX;

    public SpriteRenderer[] barra;

	// Use this for initialization
	void Start () {
        timerCounter = timer;
	}

    void Update()
    {
        if (timerCounter > 0)
        {
            float p = ((timerCounter * 100) / timer) / 10;
            updateTimerDisplay(p);
            
            timerCounter -= Time.deltaTime;
        }else if (!acted)
        {
            acted = true;
            unFreeze();
        }


    }

    void updateTimerDisplay(float p)
    {
        int i = (int) Mathf.Floor(p) - 1;
        if (i >= 0 && barra[i].enabled)
        {
            FindObjectOfType<AudioManager>().Play("clockTick");
            barra[i].enabled = false;
        }
        
    }

    void unFreeze()
    {

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        RespawEnemy[] respawns = FindObjectsOfType<RespawEnemy>();
        Projetil[] projeteis = FindObjectsOfType<Projetil>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].unFreeze();
        }
        
        for (int i = 0; i < respawns.Length; i++)
        {
            respawns[i].paused = false;
        }

        for (int i = 0; i < projeteis.Length; i++)
        {
            projeteis[i].unFreeze();
        }

        FindObjectOfType<AudioManager>().Play("getitem");
        Instantiate(leaveFX, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }
}
