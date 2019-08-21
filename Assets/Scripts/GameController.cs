using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum arrowsType
    {
        basic, brust, piercing
    }

    public Animator _playerAnim;
    public bool dead = false;

    public int qtdBrustArrow = 0;
    public int qtdPiercingArrow = 0;

    public Text brustQTD;
    public Text PiercingQTD;
    public Text scoreQTD;
    public Text scoreShadowQTD;

    public Button btnChuvaDeFlechas;
    public Image lifeBar;

    bool imortal = false;

    public arrowsType selectedArrowType = arrowsType.basic;

    public ParticleSystem healEffect;
    public ParticleSystem damageEffect;

    //Desativar ao morrer
    [Space(20)]
    public Player _player;
    public Skills _skill;
    public GameObject shotArea1;
    public GameObject shotArea2;
    public GameObject shotArea3;
    public GameObject shotArea4;

    public GameObject painelActions;
    public GameObject score;
    public GameObject scoreShadow;
    public GameObject BTNMenu;

    [Space(20)]
    public Animator MainCanvasAnim;
    public Text finalScore;

    [Space(10)]
    public int valorEstrela1;
    public Animator estrela1;
    public Text estrelaMinimumScore1;

    [Space(10)]
    public int valorEstrela2;
    public Animator estrela2;
    public Text estrelaMinimumScore2;

    [Space(10)]
    public int valorEstrela3;
    public Animator estrela3;
    public Text estrelaMinimumScore3;

    bool star1GetGoal = false;
    bool star2GetGoal = false;
    bool star3GetGoal = false;


    [Space(20)]
    public int _points = 0;
    public int points
    {
        get
        {
            return _points;
        }
        set
        {
            _points += value;
            scoreQTD.text = _points.ToString();
            scoreShadowQTD.text = _points.ToString();
        }
    }

    int _maxLife = 10;
    int _life = 10;
    public int life
    {
        get
        {
            return _life;
        }
    }

    public int maxLife
    {
        get
        {
            return _maxLife;
        }
    }

    int layerNumer = 0;

    public int getLayerNumber()
    {
        layerNumer++;
        return layerNumer;
    }

    private void Start()
    {
        estrelaMinimumScore1.text = valorEstrela1.ToString();
        estrelaMinimumScore2.text = valorEstrela2.ToString();
        estrelaMinimumScore3.text = valorEstrela3.ToString();
    }

    public void addArrowInterface(arrowsType arrow, int qtd)
    {
        if (arrow == arrowsType.brust)
        {
            qtdBrustArrow += qtd;
            brustQTD.text = "x" + qtdBrustArrow.ToString();
        }
        else if (arrow == arrowsType.piercing)
        {
            qtdPiercingArrow += qtd;
            PiercingQTD.text = "x" + qtdPiercingArrow.ToString();
        }
    }

    public void removeArrowInterface(arrowsType arrow, int qtd)
    {
        if (arrow == arrowsType.brust)
        {
            qtdBrustArrow -= qtd;
            if (qtdBrustArrow < 0)
                qtdBrustArrow = 0;
            brustQTD.text = "x" + qtdBrustArrow.ToString();
        }
        else if (arrow == arrowsType.piercing)
        {
            qtdPiercingArrow -= qtd;
            if (qtdPiercingArrow < 0)
                qtdPiercingArrow = 0;
            PiercingQTD.text = "x" + qtdPiercingArrow.ToString();
        }
    }

    bool gamePaused = false;
    public void togglePause() {
        gamePaused = !gamePaused;
        MainCanvasAnim.SetBool("paused", gamePaused);
        if (gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }

    public void damage(int d)
    {
        if (!imortal)
        {
            if (!_player.rolando)
            {
                FindObjectOfType<AudioManager>().Play("hurt");
                damageEffect.Play();

                _life -= d;
                if (_life <= 0)
                {
                    _life = 0;
                    dead = true;
                    _player.enabled = false;
                    _skill.enabled = false;
                    shotArea1.active = false;
                    shotArea2.active = false;
                    shotArea3.active = false;
                    shotArea4.active = false;
                    painelActions.active = false;
                    score.active = false;
                    scoreShadow.active = false;
                    BTNMenu.active = false;

                    finalScore.text = _points.ToString();

                    if (_points >= valorEstrela1)
                    {
                        //TODO: Liberar no google
                        star1GetGoal = true;
                    }

                    if (_points >= valorEstrela2)
                    {
                        //TODO: Liberar no google
                        star2GetGoal = true;
                    }

                    if (_points >= valorEstrela3)
                    {
                        //TODO: Liberar no google
                        star3GetGoal = true;

                    }


                    StartCoroutine(finishGame());

                    _playerAnim.SetBool("dead", true);
                }

                updateLifeBar();
                StartCoroutine(imortalTimer());
            }
        }
    }


    IEnumerator finishGame()
    {
        yield return new WaitForSeconds(1);
        MainCanvasAnim.Play("gameOver");
        yield return new WaitForSeconds(1);
        if (star1GetGoal)
            estrela1.Play("StarGeted");
        yield return new WaitForSeconds(0.5f);
        if (star2GetGoal)
            estrela2.Play("StarGeted");
        yield return new WaitForSeconds(0.5f);
        if (star3GetGoal)
            estrela3.Play("StarGeted");
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
    }


    IEnumerator imortalTimer()
    {
        imortal = true;
        _playerAnim.SetBool("imortal", imortal);
        yield return new WaitForSeconds(3);
        imortal = false;
        _playerAnim.SetBool("imortal", imortal);
    }


    public void heal(int h)
    {
        FindObjectOfType<AudioManager>().Play("heal");
        healEffect.Play();

        _life += h;
        if (_life > _maxLife)
        {
            _life = _maxLife;
        }

        updateLifeBar();
    }

    public void updateLifeBar()
    {
        float p = (100 * _life) / _maxLife;
        lifeBar.fillAmount = (p / 100f);
    }

    public bool canChuvaDeFlechas()
    {
        bool r = false;
        if (selectedArrowType == arrowsType.basic)
        {
            r = true;
        }
        else if (selectedArrowType == arrowsType.brust)
        {
            if (qtdBrustArrow >= 10)
            {
                r = true;
            }
            else
            {
                r = false;
            }
        }
        else if (selectedArrowType == arrowsType.piercing)
        {
            if (qtdPiercingArrow >= 10)
            {
                r = true;
            }
            else
            {
                r = false;
            }
        }

        return r;
    }

}
