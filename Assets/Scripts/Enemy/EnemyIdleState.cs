using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
  Vector3 enemyPos, hallwayPos, preWindowPos;
  Quaternion hallwayRotation, preWindowRotation;
  public override void enterState(EnemyController enemyController)
  {
    enemyPos = enemyController.transform.position;

    hallwayPos = enemyController.hallwayPos.transform.position;
    hallwayRotation = enemyController.hallwayPos.transform.rotation;

    preWindowPos = enemyController.preWindowPos.transform.position;
    preWindowRotation = enemyController.preWindowPos.transform.rotation;

    if (enemyPos == hallwayPos)
    {
      enemyController.transform.SetPositionAndRotation(preWindowPos, preWindowRotation);
      Debug.Log("enemy near window");
      return;
    }
    else if (enemyPos == preWindowPos)
    {
      enemyController.transform.SetPositionAndRotation(hallwayPos, hallwayRotation);
      Debug.Log("enemy at hallway");
      return;
    }

    if (Random.Range(0, 2) == 0)
    {
      enemyController.transform.SetPositionAndRotation(hallwayPos, hallwayRotation);
      Debug.Log("enemy at hallway");
      return;
    }
    else
    {
      enemyController.transform.SetPositionAndRotation(preWindowPos, preWindowRotation);
      Debug.Log("enemy near window");
      return;
    }
  }

  public override void updateState(EnemyController enemyController)
  {
    if (Random.Range(0, 2) == 0)
    {
      enemyController.currentState = enemyController.enemyPrepareState;
    }
    else
    {
      enemyController.currentState = enemyController.enemyIdleState;
      // enemyController.SetBasePosition();
    }
    // enemyController.currentState.enterState(enemyController);
  }
}
