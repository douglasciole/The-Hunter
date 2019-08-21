using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour {
    
    public Enemy _enemy;
    public float attackTimer;
    [HideInInspector]
    public bool inAttack = false;

    Collider2D collider;
    public int damage = 1;
    public float cooldownTime = 1;
    bool coolDown = false;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        _enemy = GetComponentInParent<Enemy>();
    }

    public void attack()
    {
        if (!inAttack && !coolDown)
        {
            StartCoroutine(cooldown());
            StartCoroutine(performAttack());
        }
    }

    IEnumerator performAttack()
    {
        inAttack = true;
        if (_enemy.type == Enemy.monsterType.terrestre) {
            if (_enemy.anim)
                _enemy.anim.Play("attack");
            
            //Terrestre com projetil
            if (_enemy.useProjetil)
            {
                _enemy.onCombate = true;
                yield return new WaitForSeconds(attackTimer);
                if (_enemy.tProjSpawnPoint)
                {
                    float force = Vector2.Distance(_enemy._gameController._player.transform.position, _enemy.transform.position) / 2.4f;
                    Rigidbody2D _proj = Instantiate(_enemy.tProjetil, _enemy.tProjSpawnPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                    if (_enemy.direction >= 0) {
                        _proj.AddTorque(force + 5);
                        _proj.AddForce(((Vector2.up / 2) + Vector2.right) * force, ForceMode2D.Impulse);
                    }else
                    {
                        _proj.AddTorque((force + 5)*-1);
                        _proj.AddForce(((Vector2.up / 2) + Vector2.left) * force, ForceMode2D.Impulse);
                    }
                }
                yield return new WaitForSeconds(0.1f);
                _enemy.onCombate = false;
            }
            else
            {

                yield return new WaitForSeconds(attackTimer);
                collider.enabled = true;
                yield return new WaitForSeconds(0.1f);
                collider.enabled = false;
            }
            
            
        }else
        {
            _enemy.onCombate = true;
            Vector3 originalScale = _enemy.transform.localScale;

            if (_enemy.transform.position.x >= _enemy._gameController._player.transform.position.x)
            {
                if (_enemy.transform.localScale.x > 0)
                {
                    _enemy.transform.localScale = new Vector3(_enemy.transform.localScale.x * -1, _enemy.transform.localScale.y, _enemy.transform.localScale.z);
                }
            }else
            {
                if (_enemy.transform.localScale.x < 0)
                {
                    _enemy.transform.localScale = new Vector3(_enemy.transform.localScale.x * -1, _enemy.transform.localScale.y, _enemy.transform.localScale.z);
                }
            }

            yield return new WaitForSeconds(0.2f);
            if (_enemy.anim)
                _enemy.anim.Play("attack");
            yield return new WaitForSeconds(attackTimer);
            Instantiate(_enemy.projetil, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            _enemy.transform.localScale = originalScale;
            yield return new WaitForSeconds(0.3f);
            _enemy.restartFlightAttackCooldown();
            _enemy.onCombate = false;
        }
        inAttack = false;
    }

    IEnumerator cooldown()
    {
        coolDown = true;
        yield return new WaitForSeconds(cooldownTime);
        coolDown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemy._gameController.damage(damage);
    }
}
