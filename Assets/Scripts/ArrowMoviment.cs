using System.Collections;
using UnityEngine;

// this class steers the arrow and its behaviour


public class ArrowMoviment : MonoBehaviour
{

    // register collision
    [HideInInspector]
    public bool collisionOccurred = false;
    public GameObject arrowHead;
    public LayerMask targetLayerMask;

    [HideInInspector]
    public Rigidbody _rBody;
    SpriteRenderer arrowRender, headRender;
    [HideInInspector]
    public bool fliped = false;

    IArrowCollision collisionActions;

    private void Awake()
    {
        _rBody = GetComponent<Rigidbody>();
        arrowRender = GetComponent<SpriteRenderer>();
        headRender = arrowHead.GetComponent<SpriteRenderer>();
        collisionActions = GetComponent<IArrowCollision>();
        
    }

    // Use this for initialization
    void Start()
    {        
        StartCoroutine(destroyUnHitArrow());
    }

    IEnumerator destroyUnHitArrow()
    {
        yield return new WaitForSeconds(5);
        if (!collisionOccurred)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (_rBody != null)
        {
            if (_rBody.velocity != Vector3.zero)
            {
                Vector3 vel = _rBody.velocity;
                fliped = false;

                float angleZ = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                float angleY = Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg;

                if (_rBody.velocity.x < 0)
                {
                    
                    renderFlip();
                    angleZ *= -1;
                    fliped = true;
                    
                }

                transform.eulerAngles = new Vector3(0, -angleY, angleZ);

            }

        }

        if (!collisionOccurred)
        {

            Vector2 testAngleBase = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z * ((fliped) ? -1 : 1)) * Vector2.right);
            Vector2 drawLineAngleTarget = (testAngleBase * 0.45f) + (Vector2)transform.position;
            Debug.DrawLine(transform.position, drawLineAngleTarget, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, testAngleBase, 0.45f, targetLayerMask);

            if (hit)
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    if (collisionActions != null)
                        collisionActions.GroundCollision(hit, this);
                }
                else if (hit.collider.gameObject.tag == "Enemy")
                {
                    if (collisionActions != null)
                        collisionActions.EnemyCollision(hit, this);
                }
                else if (hit.collider.gameObject.tag == "Item")
                {
                    if (collisionActions != null)
                        collisionActions.ItemCollision(hit, this);
                }
                else if (hit.collider.gameObject.tag == "Protection")
                {
                    if (collisionActions != null)
                        collisionActions.ProtectionCollision(hit, this);
                }
                else if (hit.collider.gameObject.tag == "Projetil")
                {
                    if (collisionActions != null)
                        collisionActions.ProjetilCollision(hit, this);
                }


            }

        }

    }

    public void renderFlip()
    {

        arrowRender.flipX = true;
        headRender.flipX = true;
    }

    public void renderUnFlip()
    {

        arrowRender.flipX = false;
        headRender.flipX = false;
    }

}
