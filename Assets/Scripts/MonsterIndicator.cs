using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterIndicator : MonoBehaviour
{

    public GameObject leftSign;
    public GameObject rightSign;

    Transform rightBorder;
    Transform leftBorder;

    // Use this for initialization
    void Start()
    {
        rightBorder = GameObject.FindGameObjectWithTag("marginRight").transform;
        leftBorder = GameObject.FindGameObjectWithTag("marginLeft").transform;

        StartCoroutine(checkEnemies());
    }

    IEnumerator checkEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length > 0)
            {
                bool hasEnemyOnScreen = false;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].transform.position.x >= leftBorder.position.x && enemies[i].transform.position.x <= rightBorder.position.x)
                    {
                        hasEnemyOnScreen = true;
                        break;
                    }
                }

                if (!hasEnemyOnScreen)
                {
                    if (enemies[0].transform.position.x < leftBorder.position.x) {
                        leftSign.active = true;
                        rightSign.active = false;
                    }
                    else
                    {
                        rightSign.active = true;
                        leftSign.active = false;
                    }
                }else
                {
                    rightSign.active = false;
                    leftSign.active = false;
                }
            }
        }
    }

}
