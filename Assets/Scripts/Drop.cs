using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    public GameObject[] dropList;
    bool hasDroped = false;
    [Range(1, 100)]
    public int DropChence = 20;
    public float dropArea = 1;
	
    public void drop(int times)
    {
        if (!hasDroped)
        {
            for (int i = 0; i < times; i++)
            {
                int dropRoleta = Random.Range(1, 101);

                if (dropRoleta <= DropChence && dropList.Length > 0)
                {
                    Vector3 pos = Random.insideUnitCircle * dropArea;

                    int itemIndex = Random.Range(0, dropList.Length);
                    Instantiate(dropList[itemIndex], transform.position + pos, Quaternion.identity);
                }

            }

            hasDroped = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dropArea);
    }

}
