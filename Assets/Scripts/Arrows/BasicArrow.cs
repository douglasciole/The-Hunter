using System.Collections.Generic;
using UnityEngine;

public class BasicArrow : MonoBehaviour, IArrowCollision
{
    public ArrowMoviment arrow;
    public TrailRenderer _trail;
    public TrailRenderer _trail2;

    public GameObject ArrowDamage;

    public int damage = 1;

    void communAction(ArrowMoviment arrow)
    {
        
        arrow.collisionOccurred = true;
        arrow.arrowHead.SetActive(false);

        _trail.enabled = false;
        _trail2.enabled = false;

        arrow._rBody.velocity = Vector3.zero;
        arrow._rBody.isKinematic = true;
        arrow._rBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnemyCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {

        EnemyPart ep = hit.collider.gameObject.GetComponent<EnemyPart>();
        if (!ep._enemy.dead)
        {
            FindObjectOfType<AudioManager>().Play("basicArrow");
            communAction(arrow);
            ep.hit(damage, GameController.arrowsType.basic);
            arrow.transform.SetParent(ep.transform);

            if (ArrowDamage)
                Instantiate(ArrowDamage, hit.collider.transform.position, Quaternion.identity);
            Destroy(arrow.gameObject, 8);
        }
    }

    public void GroundCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("arrowOnGround");
        communAction(arrow);
        Destroy(arrow.gameObject, 5);
    }

    public void ItemCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("basicArrow");
        communAction(arrow);
        hit.collider.GetComponent<IItem>().Action();

        if (ArrowDamage)
            Instantiate(ArrowDamage, hit.collider.transform.position, Quaternion.identity);

        Destroy(hit.collider.gameObject, 0.5f);
        Destroy(arrow.gameObject);
    }

    private void Update()
    {
        if (arrow._rBody.velocity != Vector3.zero)
        {
            

            if (arrow._rBody.velocity.x < 0)
            {
                if (!_trail.enabled)
                    _trail.enabled = true;
            }
            else
            {
                if (!_trail2.enabled)
                    _trail2.enabled = true;
            }
        }
    }

    public void ProtectionCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("protection");
        if (_trail.enabled)
        {
            _trail.enabled = false;
            _trail2.enabled = true;
        }else
        {
            _trail.enabled = true;
            _trail2.enabled = false;
        }

        if (arrow.transform.position.x > hit.collider.transform.position.x)
        {
            arrow.renderUnFlip();
        }

        arrow._rBody.velocity = (arrow._rBody.velocity * - 1) + (new Vector3(0, 1, 0) * 10);
    }

    List<Projetil> hitedProjetils = new List<Projetil>();
    public void ProjetilCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        Projetil _proj = hit.collider.GetComponent<Projetil>();

        if (!hitedProjetils.Contains(_proj))
        {
            hitedProjetils.Add(_proj);
            if (_proj.destrutivel)
            {
                FindObjectOfType<AudioManager>().Play("basicArrow");
                if (ArrowDamage)
                    Instantiate(ArrowDamage, _proj.transform.position, Quaternion.identity);
            }

            if (_proj.solido)
            {
                arrow.transform.SetParent(_proj.transform);
                communAction(arrow);
            }
            else
            {
                if (_proj.colideFlecha)
                {
                    Destroy(gameObject);
                }

            }

            _proj.damage(damage);
        }
    }
}
