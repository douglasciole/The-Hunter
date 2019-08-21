using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuvaDeFlechasSpawner : MonoBehaviour
{


    public Vector3 spanwArea;
    public GameObject flecha;
    bool raining = false;
    
    public void Rain()
    {
        raining = true;
        StartCoroutine(Spawn());
        StartCoroutine(Timer());
    }

    IEnumerator Spawn()
    {
        while (raining)
        {
            Vector3 position = new Vector3(Random.Range(transform.position.x - (spanwArea.x / 2), transform.position.x + (spanwArea.x / 2)), transform.position.y, 0);
            Instantiate(flecha, position, Quaternion.identity);
            yield return new WaitForSeconds(0.09f);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(10);
        raining = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, spanwArea);
    }


}
