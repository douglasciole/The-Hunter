using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pearcingArrow : MonoBehaviour, IArrowCollision {

    public ArrowMoviment arrow;
    public TrailRenderer _trail;
    public TrailRenderer _trail2;

    public GameObject ArrowDamage;

    List<EnemyPart> partsHited = new List<EnemyPart>();

    public int damage = 2;

    void communAction(ArrowMoviment arrow)
    {
        arrow.collisionOccurred = true;
        arrow.arrowHead.SetActive(false);

        arrow._rBody.velocity = Vector3.zero;
        arrow._rBody.isKinematic = true;
        arrow._rBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnemyCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        EnemyPart ep = hit.collider.gameObject.GetComponent<EnemyPart>();
        if (!ep._enemy.dead)
        {
            if (ArrowDamage)
                Instantiate(ArrowDamage, hit.collider.transform.position, Quaternion.identity);

            if (!partsHited.Contains(ep))
            {
                FindObjectOfType<AudioManager>().Play("piercingArrow");
                partsHited.Add(ep);
                ep.hit(damage, GameController.arrowsType.piercing);
            }
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
        FindObjectOfType<AudioManager>().Play("piercingArrow");
        if (ArrowDamage)
            Instantiate(ArrowDamage, hit.collider.transform.position, Quaternion.identity);
        hit.collider.GetComponent<IItem>().Action();
        Destroy(hit.collider.gameObject, 0.5f);
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
        FindObjectOfType<AudioManager>().Play("piercingArrow");
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
                FindObjectOfType<AudioManager>().Play("piercingArrow");
                if (ArrowDamage)
                    Instantiate(ArrowDamage, _proj.transform.position, Quaternion.identity);
            }

            _proj.damage(damage);
        }
    }
}
