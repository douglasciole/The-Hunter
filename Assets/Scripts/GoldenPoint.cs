using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPoint : MonoBehaviour {

    EnemyPart _enemyPart;

    public GameObject goldenPointFX;
    public GameObject goldenPointHitFX;
    GameObject gp;

    // Use this for initialization
    void Start () {
        _enemyPart = GetComponentInParent<EnemyPart>();

        if (_enemyPart && goldenPointFX && goldenPointHitFX)
        {
            bool goldenShot;

            goldenShot = (Random.Range(1, 101) <= 5);
            if (goldenShot)
            {
                gp = Instantiate(goldenPointFX, transform);
                gp.transform.localPosition = Vector3.zero;

                _enemyPart._goldenPoint = this;
            }
        }
    }

    public void hit()
    {
        FindObjectOfType<AudioManager>().Play("goldenPoint");
        Destroy(gp);
        Instantiate(goldenPointHitFX, transform.position, Quaternion.identity);
    }
	
}
