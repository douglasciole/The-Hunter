using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemArrow : MonoBehaviour, IItem
{

    public int qtd;
    public Text qtdAmount;
    bool acted = false;
    public float vanishTimer = 10;
    public GameObject hitParticle;

    public GameController.arrowsType type;

    private void Start()
    {
        qtd = UnityEngine.Random.Range(1, 4);
        if (qtdAmount)
            qtdAmount.text = "x" + qtd.ToString();

        StartCoroutine(timer());
    }


    IEnumerator timer()
    {
        yield return new WaitForSeconds(vanishTimer);
        FindObjectOfType<AudioManager>().Play("itemdestroy");
        DestroyItem();
    }

    public void Action()
    {
        if (!acted)
        {
            acted = true;
            FindObjectOfType<AudioManager>().Play("getitem");
            FindObjectOfType<GameController>().addArrowInterface(type, qtd);
            DestroyItem();
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject, 0.1f);
        ParticleEmmit();
    }

    public void ParticleEmmit()
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
    }
}
