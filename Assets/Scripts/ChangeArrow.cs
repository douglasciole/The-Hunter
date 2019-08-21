using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeArrow : MonoBehaviour {

    public Player _player;
    public ChuvaDeFlechasSpawner _chuvaDeFlechas;
    public GameObject _arrow;
    public Sprite btnBoxSelected;
    public Sprite btnBoxUnselected;

    public GameController.arrowsType type;
    public ChangeArrow basicArrow;

    public GameController.arrowsType arrowType = GameController.arrowsType.basic;

    public Image este;
    public Image[] outros;

    public void changeArrow()
    {
        if (type == GameController.arrowsType.brust)
        {
            if (FindObjectOfType<GameController>().qtdBrustArrow < 1)
            {
                basicArrow.changeArrow();
                return;
            }
        }
        else if(type == GameController.arrowsType.piercing)
        {
            if (FindObjectOfType<GameController>().qtdPiercingArrow < 1)
            {
                basicArrow.changeArrow();
                return;
            }
        }


        FindObjectOfType<GameController>().selectedArrowType = arrowType;
        este.sprite = btnBoxSelected;
        for (int i = 0; i < outros.Length; i++)
        {
            outros[i].sprite = btnBoxUnselected;
        }

        _player.arrow = _arrow;
        //_chuvaDeFlechas.flecha = _arrow;
    }

}
