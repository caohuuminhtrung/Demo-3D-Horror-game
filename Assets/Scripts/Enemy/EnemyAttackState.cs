using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class EnemyAttackState : EnemyBaseState
{
  Vector3 playerPos, playerDirection;
  Quaternion playerRotation;
  public override void enterState(EnemyController enemyController)
  {
    playerPos = enemyController.player.transform.position;
    playerRotation = enemyController.player.transform.rotation;
    playerDirection = enemyController.player.transform.forward;

    float spawnDistance = 10;

    Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
    spawnPos.y = spawnPos.y + 5;

    enemyController.transform.SetPositionAndRotation(spawnPos, playerRotation);
  }

  public override void updateState(EnemyController enemyController)
  {
    enemyController.SetBasePosition();
  }
}
