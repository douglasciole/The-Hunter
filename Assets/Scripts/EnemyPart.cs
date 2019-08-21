using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour {

    [HideInInspector]
    public Enemy _enemy;
    public GoldenPoint _goldenPoint;

    public void hit(int damage, GameController.arrowsType arrow)
    {
        if (_goldenPoint && arrow == GameController.arrowsType.basic)
        {
            _goldenPoint.hit();
            Destroy(_goldenPoint.gameObject);
            _goldenPoint = null;

            _enemy._gameController.points = _enemy.killPoints;
            _enemy.damage(damage * 2);
        }else
        {
            _enemy._gameController.points = _enemy.pointsPerHit;
            _enemy.damage(damage);
        }
    }


    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        
    }

}
