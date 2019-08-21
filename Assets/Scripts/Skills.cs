using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{

    public Player _player;
    public ShotArea[] shootingAreas;
    public ChuvaDeFlechasSpawner ChuvaDeFlechasSpawner;

    public Button btnChuvaDeFlechas;
    public Image cooldownChuvaDeFlechas;
    float cooldownChuvaDeFlechasTimer;
    float cooldownChuvaDeFlechasTime = 40;

    public ParticleSystem skillEffect;

    public Animator brustAlert;
    public Animator piercingAlert;

    GameObject actualArrow;
    
    public GameObject rolarLeft;
    public GameObject rolarRight;

    public void startChuvaDeFlechas()
    {
        if (!_player.rolando)
        {
            GameController gc = FindObjectOfType<GameController>();
            if (!gc.canChuvaDeFlechas())
            {
                if (gc.selectedArrowType == GameController.arrowsType.brust)
                {
                    brustAlert.Play("AlertShowUpAnimation");
                }
                else if (gc.selectedArrowType == GameController.arrowsType.piercing)
                {
                    piercingAlert.Play("AlertShowUpAnimation");
                }
                return;
            }

            if (gc.selectedArrowType == GameController.arrowsType.brust)
            {
                gc.removeArrowInterface(GameController.arrowsType.brust, 10);
            }
            else if (gc.selectedArrowType == GameController.arrowsType.piercing)
            {
                gc.removeArrowInterface(GameController.arrowsType.piercing, 10);
            }
            
            rolarLeft.active = false;
            rolarRight.active = false;
            _player.onSkill = true;

            skillEffect.Play();
            FindObjectOfType<AudioManager>().Play("chuvaDeflechasCharge");

            actualArrow = _player.arrow;

            btnChuvaDeFlechas.interactable = false;
            cooldownChuvaDeFlechas.fillAmount = 1;
            cooldownChuvaDeFlechasTimer = cooldownChuvaDeFlechasTime;

            for (int i = 0; i < shootingAreas.Length; i++)
            {
                shootingAreas[i].enabled = false;
            }
            _player._anim.Play("ChuvaDeFlechas");
        }

    }

    public void chuvaDeFlechas()
    {
        FindObjectOfType<AudioManager>().Play("chuvaDeflechasShooting");
        _player._anim.Play("ChuvaDeFlechas2");
        _player.chuvaDeFlechaParticle.Play();
        StartCoroutine(stopChuvaDeFlechas());
    }

    IEnumerator stopChuvaDeFlechas()
    {
        yield return new WaitForSeconds(2);
        ChuvaDeFlechasSpawner.flecha = actualArrow;
        ChuvaDeFlechasSpawner.Rain();
        rolarLeft.active = true;
        rolarRight.active = true;
        FindObjectOfType<AudioManager>().Stop("chuvaDeflechasShooting");
        _player.chuvaDeFlechaParticle.Stop();
        _player._anim.Play("IDLE");
        _player.onSkill = false;
        for (int i = 0; i < shootingAreas.Length; i++)
        {
            shootingAreas[i].enabled = true;
        }

    }

    private void Update()
    {
        if (cooldownChuvaDeFlechasTimer > 0)
        {
            cooldownChuvaDeFlechasTimer -= Time.deltaTime;
            float p = ((100 * cooldownChuvaDeFlechasTimer) / cooldownChuvaDeFlechasTime) / 100;
            cooldownChuvaDeFlechas.fillAmount = p;
        }
        else if (!btnChuvaDeFlechas.interactable)
        {
            btnChuvaDeFlechas.interactable = true;
        }
    }

}
