using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour, IItem {

    public GameObject unFreezeClock;
    bool acted = false;
    public float vanishTimer = 10;
    public GameObject hitParticle;

    private void Start()
    {
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(vanishTimer);
        FindObjectOfType<AudioManager>().Play("itemdestroy");
        DestroyItem();
    }

    public void Action()
    {
        int activeClocksNumber = FindObjectsOfType<ClockUnFreeze>().Length;
        if (activeClocksNumber < 1)
        {
            if (!acted)
            {
                acted = true;
                FindObjectOfType<AudioManager>().Play("getitem");
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                RespawEnemy[] respawns = FindObjectsOfType<RespawEnemy>();
                Projetil[] projeteis = FindObjectsOfType<Projetil>();

                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].freeze();
                }

                for (int i = 0; i < respawns.Length; i++)
                {
                    respawns[i].paused = true;
                }

                for (int i = 0; i < projeteis.Length; i++)
                {
                    projeteis[i].freeze();
                }

                Instantiate(unFreezeClock, transform.position, Quaternion.identity);
                DestroyItem();
            }
        }else
        {
            DestroyItem();
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject, 0.1f);
        ParticleEmmit();
    }

    public void ParticleEmmit()
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
    }
}
