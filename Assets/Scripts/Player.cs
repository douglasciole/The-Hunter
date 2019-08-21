using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    private float velocity = 8f;
    float rolarVelocity = 12f;
    public GameObject arrow;

    public GameObject powerIndicator;
    public Image clickedPoint;

    public SpriteRenderer powerIndicatorBar;
    float maxSize = 12;

    float pixelWidth, pixelHeight;
    bool startedShoot = false;

    Vector2 startedTouch, endedTouch;

    public Animator _anim;
    public SpriteRenderer _plyerView;
    float timer = 0.5f;
    public bool canShot = true;
    public bool canReleaseShot = false;
    public bool onSkill = false;

    public ParticleSystem chuvaDeFlechaParticle;

    GameController _gameController;
    public ChangeArrow basicChanger;

    public GameObject BTNRolarLeft;
    public GameObject BTNRolarRight;
    public GameObject BTNRolarShot;

    public bool rolando = false;
    float _distance = 0;
    Vector2 rolarShotSide;

    public void rolar(float distance)
    {
        rolando = true;
        cancelShot();
        _anim.SetBool("rolando", rolando);
        _anim.Play("esquiva");
        _distance = distance;
        if (_distance < 0)
        {
            rolarShotSide = new Vector2(-1, -0.2f);
            _plyerView.flipX = true;
        }
        else
        {
            rolarShotSide = new Vector2(1, -0.2f);
            _plyerView.flipX = false;
        }

        StartCoroutine(rolarCoolDown());
    }

    public void rolarShot()
    {
        BTNRolarShot.active = false;
        Shoot(rolarShotSide, 1);
    }

    IEnumerator rolarCoolDown()
    {
        while (onSkill)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BTNRolarLeft.active = false;
        BTNRolarRight.active = false;
        BTNRolarShot.active = true;

        yield return new WaitForSeconds(0.5f);
        while (onSkill)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BTNRolarShot.active = false;

        yield return new WaitForSeconds(0.2f);
        while (onSkill)
        {
            yield return new WaitForSeconds(0.1f);
        }
        BTNRolarLeft.active = true;
        BTNRolarRight.active = true;
    }

    void Start()
    {

        this.pixelWidth = Camera.main.pixelWidth;
        this.pixelHeight = Camera.main.pixelHeight;

        _gameController = FindObjectOfType<GameController>();
    }

    Vector2 getAngle()
    {
        endedTouch = Input.mousePosition;
        Vector2 angleVec = (endedTouch - startedTouch).normalized;

        return angleVec;
    }

    float getDegAngle(Vector2 angle)
    {
        if (angle.x < 0)
        {
            return 360 - (Mathf.Atan2(angle.x, angle.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(angle.x, angle.y) * Mathf.Rad2Deg;
        }
    }


    public void startShot()
    {
        if (!rolando)
        {
            clickedPoint.enabled = true;

            var screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
            clickedPoint.rectTransform.position = Camera.main.ScreenToWorldPoint(screenPoint);


            if (canShot)
            {
                powerIndicatorBar.size = new Vector2(powerIndicatorBar.size.x, 0);
                canShot = false;
                canReleaseShot = true;
                timer = 0.1f;
                _anim.SetBool("onBegin", true);
                _anim.SetInteger("stage", 1);
                powerIndicator.SetActive(true);
                startedTouch = Input.mousePosition;
                startedShoot = true;
            }
        }
    }

    public void cancelShot()
    {
        _anim.SetInteger("stage", 0);
        _anim.SetBool("onBegin", false);
        clickedPoint.enabled = false;
        canShot = true;
        canReleaseShot = false;
        powerIndicator.SetActive(false);
        startedShoot = false;
    }

    public void performShot()
    {
        if (!rolando)
        {
            clickedPoint.enabled = false;
            if (canReleaseShot)
            {
                canReleaseShot = false;
                _anim.SetInteger("stage", 0);
                _anim.SetBool("onBegin", false);
                StartCoroutine(cooldown());
                startedShoot = false;
                powerIndicator.SetActive(false);
                endedTouch = Input.mousePosition;
                // Define a rotation da flecha
                Vector2 angle = getAngle();

                if (angle.y > 0)
                {
                    angle.y = 0;
                    if (angle.x > 0)
                    {
                        angle.x = 1;
                    }
                    else if (angle.x < 0)
                    {
                        angle.x = -1;
                    }
                }

                float power = getPower(endedTouch);
                Shoot(angle, power);
            }
        }
    }

    public void dragHandler()
    {
        if (!rolando)
        {
            if (startedShoot && timer <= 0)
            {
                Vector3 angle = getAngle();

                if (angle.y > 0)
                    angle.y = 0;

                float angleZ = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
                float angleY = Mathf.Atan2(angle.z, angle.x) * Mathf.Rad2Deg;

                angleZ -= 90;
                angleZ += 180;

                updateAlture(angleZ);



                if (angle.x < 0)
                {
                    _plyerView.flipX = false;
                    angleZ *= -1;
                }
                else
                {
                    _plyerView.flipX = true;
                }

                powerIndicator.transform.eulerAngles = new Vector3(0, -angleY, angleZ);

                float power = getPower(Input.mousePosition);
                powerIndicatorBar.size = new Vector2(powerIndicatorBar.size.x, (power * 8) + 4);

            }
        }
    }

    void Update()
    {

        if (startedShoot && timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            rolar(-0.4f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rolar(0.4f);
        }

        if (_distance != 0)
        {
            if (_distance < 0)
            {
                _distance += Time.deltaTime;
                if (_distance >= 0)
                {
                    _distance = 0;
                    rolando = false;
                    _anim.SetBool("rolando", rolando);
                }
                transform.position = new Vector3(this.transform.position.x + rolarVelocity * Time.deltaTime * -1, this.transform.position.y, 0);
            }
            else
            {
                _distance -= Time.deltaTime;
                if (_distance <= 0)
                {
                    _distance = 0;
                    rolando = false;
                    _anim.SetBool("rolando", rolando);
                }
                transform.position = new Vector3(this.transform.position.x + rolarVelocity * Time.deltaTime * 1, this.transform.position.y, 0);
            }
        }

    }

    IEnumerator cooldown()
    {

        yield return new WaitForSeconds(0.5f);
        canShot = true;

    }

    void updateAlture(float angleZ)
    {
        if (angleZ > 0 && angleZ <= 30)
        {
            _anim.SetInteger("altura", 3);
            //-x a = 3
        }
        else if (angleZ > 31 && angleZ <= 60)
        {
            _anim.SetInteger("altura", 2);
            //-x a = 2
        }
        else if (angleZ > 61 && angleZ <= 90)
        {
            _anim.SetInteger("altura", 1);
            //-x a = 1
        }
        else if (angleZ <= 0 && angleZ >= -30)
        {
            _anim.SetInteger("altura", 3);
            //x a = 3
        }
        else if (angleZ < -31 && angleZ >= -60)
        {
            _anim.SetInteger("altura", 2);
            //x a = 2
        }
        else if (angleZ < -61 && angleZ > -90 || angleZ == 270)
        {
            _anim.SetInteger("altura", 1);
            //x a = 1
        }
    }

    float getPower(Vector2 endedPoint)
    {
        float distance = Vector2.Distance(endedPoint, startedTouch);
        if (distance > 300)
            distance = 300;

        return ((distance * 100) / 300) / 100;
    }

    void Shoot(Vector2 angle, float power)
    {
        FindObjectOfType<AudioManager>().Play("shot");

        angle *= -1;
        Vector2 angleVec = (startedTouch - endedTouch);
        Rigidbody bulletInstance = Instantiate(arrow, transform.position, Quaternion.LookRotation(new Vector3(angle.x, angle.y, 0))).GetComponent<Rigidbody>();
        bulletInstance.AddForce(new Vector3(power * (angle.x * 100 * velocity), power * (angle.y * 100 * velocity), 0));

        if (_gameController.selectedArrowType == GameController.arrowsType.brust)
        {
            _gameController.removeArrowInterface(GameController.arrowsType.brust, 1);
            if (_gameController.qtdBrustArrow <= 0)
            {
                basicChanger.changeArrow();
            }
        }
        else if (_gameController.selectedArrowType == GameController.arrowsType.piercing)
        {
            _gameController.removeArrowInterface(GameController.arrowsType.piercing, 1);
            if (_gameController.qtdPiercingArrow <= 0)
            {
                basicChanger.changeArrow();
            }
        }

    }




}
