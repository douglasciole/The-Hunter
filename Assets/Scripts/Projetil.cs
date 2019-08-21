using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Projetil : MonoBehaviour
{

    public enum projetilMovimentType
    {
        fixo,
        angular,
        gravitacional
    }

    [HideInInspector]
    public Rigidbody2D _rBody;
    CircleCollider2D _collider;
    GameController _gameController;
    Vector2 direction;

    [Range(1f, 10f)]
    public float speed = 1;
    [Range(1, 10)]
    public int damageToTarget = 1;
    [Range(1, 5)]
    public int life = 1;
    [Range(0.1f, 15f)]
    public float timer = 2;
    public bool solido = false;
    public bool colideFlecha = false;
    public bool destrutivel = true;
    public bool dinamicRotation = false;
    public projetilMovimentType moviment = projetilMovimentType.angular;
    public GameObject collisionFX;
    public string finishingAudio;
    float freezeMod = 1;

    Vector2 _freezedVel;
    float _freezedGrav;
    float _freezedAng;

    public void damage(int d)
    {
        if (destrutivel)
        {
            life -= d;
            if (life <= 0)
            {
                showFX();
                Destroy(gameObject);
            }
        }
    }

    public void showFX()
    {
        if (finishingAudio.Trim() != "")
        {
            FindObjectOfType<AudioManager>().Play(finishingAudio);
        }
        if (collisionFX)
            Instantiate(collisionFX, transform.position, Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        _rBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _gameController = FindObjectOfType<GameController>();

        if (moviment == projetilMovimentType.angular || moviment == projetilMovimentType.fixo)
        {
            _rBody.gravityScale = 0;
            _rBody.velocity = Vector2.zero;
        }
        else if (moviment == projetilMovimentType.gravitacional)
        {
            _rBody.gravityScale = 1;
        }

        Vector2 dir = _gameController._player.transform.position - transform.position;
        direction = dir.normalized;
        
    }

    public void freeze()
    {
        freezeMod = 0;
        if (moviment == projetilMovimentType.gravitacional)
        {
            _freezedVel = _rBody.velocity;
            _freezedGrav = _rBody.gravityScale;
            _freezedAng = _rBody.angularVelocity;

            _rBody.angularVelocity = 0;
            _rBody.velocity = Vector2.zero;
            _rBody.gravityScale = 0;
        }
    }

    public void unFreeze()
    {
        freezeMod = 1;
        if (moviment == projetilMovimentType.gravitacional)
        {
            _rBody.angularVelocity = _freezedAng;
            _rBody.velocity = _freezedVel;
            _rBody.gravityScale = _freezedGrav;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (freezeMod > 0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        if (_rBody != null)
        {
            if (moviment == projetilMovimentType.angular)
            {
                _rBody.velocity = direction * (speed * freezeMod);
            }

            if (dinamicRotation)
            {

                if (_rBody.velocity != Vector2.zero)
                {
                    Vector3 vel = _rBody.velocity.normalized;
                    float newRotation = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, newRotation);

                }
            }
        }

        Collider2D collider = Physics2D.OverlapCircle(_collider.bounds.center, _collider.radius, LayerMask.GetMask("Player"));

        if (collider)
        {
            if (collider.tag == "Player")
            {
                if (!_gameController._player.rolando)
                {
                    _gameController.damage(damageToTarget);
                    showFX();
                    Destroy(gameObject);
                    return;
                }
            }
        }


        collider = Physics2D.OverlapCircle(_collider.bounds.center, _collider.radius, LayerMask.GetMask("Ground"));

        if (collider)
        {
            showFX();
            Destroy(gameObject);
        }
        

    }

}
