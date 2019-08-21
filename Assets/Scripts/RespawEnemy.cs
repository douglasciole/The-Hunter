using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawEnemy : MonoBehaviour {

    public enum spawnerSides {
        leftSide = 1,
        rightSide = -1
    }

    Waves waveController;

    public spawnerSides spawnerSideOfCamera = spawnerSides.leftSide;
    GameController _gameController;
    [HideInInspector]
    public bool paused = false;

    void Start () {
        Vector3 p = new Vector3();
        Camera c = Camera.main;
        waveController = FindObjectOfType<Waves>();

        _gameController = FindObjectOfType<GameController>();

        if (spawnerSideOfCamera == spawnerSides.leftSide)
        {
            RectTransform t = GameObject.FindGameObjectWithTag("leftBorder").GetComponent<RectTransform>();
            p = t.TransformPoint(t.rect.center);
        }else
        {
            RectTransform t = GameObject.FindGameObjectWithTag("rightBorder").GetComponent<RectTransform>();
            p = t.TransformPoint(t.rect.center);
        }

        transform.position = new Vector2(p.x, transform.position.y);

        StartCoroutine (Wait(5));
	}

	IEnumerator Wait(float t)
	{
        while (paused)
        {
            yield return new WaitForSeconds(1);
        }

		yield return new WaitForSeconds (t);

        while (paused)
        {
            yield return new WaitForSeconds(1);
        }

        if (waveController && waveController.actualWaveSize > 0)
        {   
            MobWave mob = waveController.getSpawn();
            if (mob.Prefab)
            {
                float y = (float)Random.Range(mob.yPosition - mob.yVariationMin, mob.yPosition + mob.yVariationMax);

                GameObject enemy = Instantiate(mob.Prefab, new Vector3(transform.position.x, y, 0), Quaternion.identity);

                enemy.GetComponent<SpriteRenderer>().sortingOrder = _gameController.getLayerNumber();
                enemy.GetComponent<Enemy>().setDirection((int)spawnerSideOfCamera);
            }
        }

		StartCoroutine (Wait(Random.Range(3, 8)));
	
	}

}
