using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeChanger : MonoBehaviour, IItem
{
    [Range(-10, 10)]
    public int healAmount = 1;
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
        if (!acted)
        {
            acted = true;
            FindObjectOfType<AudioManager>().Play("getitem");
            GameController gc = FindObjectOfType<GameController>();
            if (gc)
            {
                if (healAmount >= 0) {
                    gc.heal(healAmount);
                }else
                {
                    gc.damage(healAmount * -1);
                }
            }

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
