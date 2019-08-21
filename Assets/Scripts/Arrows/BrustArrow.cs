using System.Collections.Generic;
using UnityEngine;

public class BrustArrow : MonoBehaviour, IArrowCollision
{

    public GameObject fireParticle;
    public Transform firePoint1;
    public Transform firePoint2;
    public ArrowMoviment arrow;
    GameObject fireInArrow;
    public GameObject explosionParticle;
    public TrailRenderer _trail;
    public TrailRenderer _trail2;

    public GameObject ArrowDamage;

    List<Enemy> enemiesHited = new List<Enemy>();

    public float explosionRadio = 1;

    public int damage = 4;

    private void Start()
    {
        if (!arrow.fliped)
        {
            fireInArrow = Instantiate(fireParticle, firePoint1);
            fireInArrow.transform.localPosition = Vector3.zero;
        }
        else
        {
            fireInArrow = Instantiate(fireParticle, firePoint2);
            fireInArrow.transform.localPosition = Vector3.zero;
        }
    }

    void communAction(ArrowMoviment arrow)
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        RaycastHit2D[] callisions = Physics2D.CircleCastAll(transform.position, explosionRadio, new Vector2(1, 1), 0, arrow.targetLayerMask);

        for (int i = 0; i < callisions.Length; i++)
        {
            GameObject go = callisions[i].collider.gameObject;
            if (go.tag == "Enemy")
            {
                EnemyPart ep = callisions[i].collider.gameObject.GetComponent<EnemyPart>();

                if (!enemiesHited.Contains(ep._enemy))
                {
                    enemiesHited.Add(ep._enemy);
                    ep.hit(damage, GameController.arrowsType.brust);
                }
                
            }
            else if (go.tag == "Item")
            {
                go.GetComponent<IItem>().Action();
                Destroy(go, 0.5f);
            }
            else if (go.tag == "Projetil")
            {
                go.GetComponent<Projetil>().damage(damage);
            }
        }

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
            FindObjectOfType<AudioManager>().Play("brustArrow");
            arrow.transform.SetParent(ep.transform);
            communAction(arrow);
            Destroy(arrow.gameObject, 8);
        }
    }

    public void GroundCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("brustArrow");
        communAction(arrow);
        Destroy(arrow.gameObject, 5);
    }

    public void ItemCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("brustArrow");
        communAction(arrow);
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
                fireInArrow.transform.SetParent(firePoint2);
            }
            else
            {
                if (!_trail2.enabled)
                    _trail2.enabled = true;
                fireInArrow.transform.SetParent(firePoint1);
            }
            
            fireInArrow.transform.localPosition = Vector3.zero;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadio);
    }

    public void ProtectionCollision(RaycastHit2D hit, ArrowMoviment arrow)
    {
        FindObjectOfType<AudioManager>().Play("protection");
        if (_trail.enabled)
        {
            _trail.enabled = false;
            _trail2.enabled = true;
        }
        else
        {
            _trail.enabled = true;
            _trail2.enabled = false;
        }

        if (arrow.transform.position.x > hit.collider.transform.position.x)
        {
            arrow.renderUnFlip();
        }

        arrow._rBody.velocity = (arrow._rBody.velocity * -1) + (new Vector3(0, 1, 0) * 10);
    }

    public void ProjetilCollision2(RaycastHit2D hit, ArrowMoviment arrow)
    {
        communAction(arrow);
        Destroy(arrow.gameObject, 8);
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
                FindObjectOfType<AudioManager>().Play("brustArrow");
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
                    communAction(arrow);
                    Destroy(gameObject, 0.5f);
                }

            }
        }
    }
}
