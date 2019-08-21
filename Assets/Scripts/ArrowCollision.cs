using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowCollision {
    
    void EnemyCollision(RaycastHit2D hit, ArrowMoviment arrow);
    void GroundCollision(RaycastHit2D hit, ArrowMoviment arrow);
    void ItemCollision(RaycastHit2D hit, ArrowMoviment arrow);
    void ProtectionCollision(RaycastHit2D hit, ArrowMoviment arrow);
    void ProjetilCollision(RaycastHit2D hit, ArrowMoviment arrow);
}
