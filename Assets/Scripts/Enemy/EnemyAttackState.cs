using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class EnemyAttackState : EnemyBaseState
{
  Vector3 playerPos, playerDirection;
  Quaternion playerRotation;
  public override void enterState(EnemyController enemy)
  {
    Debug.Log("attack!");
    StopCoroutine(enemy.EnemyMovementBehaviour(enemy.AIlevel));
  }


  public override void updateState(EnemyController enemyController)
  {
    return;
  }

}
