using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    public enum monsterType
    {
        terrestre,
        voador
    }

    [Header("Atributos")]
    public monsterType type = monsterType.terrestre;
    public int health = 5;
    [Range(0.1f, 3f)]
    public float velocity = 1f;
    public int pointsPerHit = 10;
    public int killPoints = 150;
    public string dieAudio;
    public bool destroyOnLeaveScreen = false;
    public bool invertedFlip = false;

    [Space(15), Header("Drop")]
    public int dropRate = 1;

    [HideInInspector]
    public int direction = 1;

    [HideInInspector]
    public bool dead = false;
    bool freezed = false;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public GameController _gameController;
    Drop _drop;
    [HideInInspector]
    public bool onCombate = false;
    Combate _combate;
    public bool hasIdleAnimation = false;
    bool idleMode = false;

    [Space(15), Range(0.1f, 10f), Header("Terrestre")]
    public float attackDistance;
    [Range(0.5f, 8f)]
    public float verifyDirectionDelay = 1;
    public bool useProjetil = false;
    public GameObject tProjetil;
    public Transform tProjSpawnPoint;

    [Space(15), Header("Voador")]
    public bool useGravityOnDie = true;
    [Range(1f, 15f)]
    public float shotingSpeed = 4;
    [Range(0f, 3f)]
    public float shotingSpeedVariation = 0;
    float shotingCooldown = 15;
    public GameObject projetil;

    Transform leftMargin;
    Transform rightMargin;
    
    public delegate void DieDelegate();
    public static event DieDelegate OnDie;

    // Use this for initialization
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _drop = GetComponentInChildren<Drop>();
        _combate = GetComponentInChildren<Combate>();
        if (_combate)
            _combate._enemy = this;
        anim = GetComponent<Animator>();

        //Cooldown next action
        restartFlightAttackCooldown();

        if (type == monsterType.voador || destroyOnLeaveScreen)
        {
            leftMargin = GameObject.FindGameObjectWithTag("marginLeft").transform;
            rightMargin = GameObject.FindGameObjectWithTag("marginRight").transform;
        }

        if (type == monsterType.terrestre && !destroyOnLeaveScreen)
        {
            StartCoroutine(verifyDirection());
        }
    }

    IEnumerator verifyDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(verifyDirectionDelay);

            if (dead)
            {
                break;
            }

            if (direction > 0 && transform.position.x >= _gameController._player.transform.position.x)
            {
                setDirection(-1);
            }
            else if (direction < 0 && transform.position.x < _gameController._player.transform.position.x)
            {
                setDirection(1);
            }
        }
    }

    public void damage(int d)
    {
        health -= d;

        if (health <= 0)
        {
            if (OnDie != null && !dead)
                OnDie();

            LayerMask deadLayer = LayerMask.NameToLayer("Dead");
            gameObject.layer = deadLayer;
            EnemyPart[] parts = GetComponentsInChildren<EnemyPart>();
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].gameObject.layer = deadLayer;
            }

            if (dieAudio.Trim() != "")
            {
                FindObjectOfType<AudioManager>().Play(dieAudio);
            }
            anim.speed = 1;
            if (_drop)
            {
                _drop.drop(dropRate);
            }
            _gameController.points = killPoints;
            dead = true;
            if (anim)
                anim.SetBool("dead", true);
            if (type == monsterType.terrestre)
            {
                Destroy(gameObject, 6);
            }
            else if (type == monsterType.voador)
            {
                Rigidbody2D _rb = GetComponent<Rigidbody2D>();
                if (_rb && useGravityOnDie)
                    _rb.gravityScale = 1;
                Destroy(gameObject, 0.8f);
            }
        }
    }

    public void freeze()
    {
        if (!dead)
        {
            freezed = true;
            anim.speed = 0;
        }
    }

    public void unFreeze()
    {
        freezed = false;
        anim.speed = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (!dead && !freezed)
        {
            if (!onCombate)
            {

                if (destroyOnLeaveScreen)
                {
                    if (direction > 0 && transform.position.x >= rightMargin.position.x + 5)
                    {
                        Destroy(gameObject);
                    }
                    else if (direction < 0 && transform.position.x <= leftMargin.position.x - 5)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (type == monsterType.voador)
                    {
                        if (direction > 0)
                        {//indo para direita
                            if (transform.position.x >= rightMargin.position.x)
                            {
                                setDirection(-1);
                            }
                        }
                        else
                        {//indo para esquerda
                            if (transform.position.x <= leftMargin.position.x)
                            {
                                setDirection(1);
                            }
                        }
                    }
                }


                this.transform.position = new Vector3(this.transform.position.x + this.velocity * Time.deltaTime * this.direction, this.transform.position.y, 0);
            }

            if (_combate)
            {
                if (type == monsterType.terrestre)
                {

                    RaycastHit2D hit;

                    if (direction < 0)
                    {
                        hit = Physics2D.Raycast(_combate.transform.position, Vector2.left, attackDistance, LayerMask.GetMask("Player"));
                    }
                    else
                    {
                        hit = Physics2D.Raycast(_combate.transform.position, Vector2.right, attackDistance, LayerMask.GetMask("Player"));
                    }

                    if (hit)
                    {
                        onCombate = true;
                        if (!_combate.inAttack)
                        {
                            _combate.attack();
                        }

                    }
                    else
                    {
                        onCombate = false;
                    }

                    if (hasIdleAnimation && idleMode != onCombate)
                    {
                        idleMode = onCombate;
                        anim.SetBool("idle", idleMode);
                    }

                }
                else //Voador
                {
                    if (projetil && !onCombate)
                    {
                        if (shotingCooldown > 0)
                        {
                            shotingCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            _combate.attack();
                        }
                    }
                }
            }
        }

    }

    public void restartFlightAttackCooldown()
    {
        float sTime = Random.Range(shotingSpeed - shotingSpeedVariation, shotingSpeed + shotingSpeedVariation);
        if (sTime < 0)
            sTime = 0;
        shotingCooldown = sTime;
    }

    public void setDirection(int direction)
    {
        if (!invertedFlip)
        {
            if (direction < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }else
        {
            if (direction < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if (direction >= 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        this.direction = direction;
    }

    private void OnDrawGizmos()
    {
        if (_combate)
        {
            if (type == monsterType.terrestre)
            {
                Gizmos.color = Color.red;
                if (direction < 0)
                    Gizmos.DrawRay(_combate.transform.position, (Vector2.left * attackDistance));
                else
                    Gizmos.DrawRay(_combate.transform.position, (Vector2.right * attackDistance));
            }

        }

    }

}
