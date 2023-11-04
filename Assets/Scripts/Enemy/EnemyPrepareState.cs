using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrepareState : EnemyBaseState
{
  Vector3 enemyPos, doorPos, windowPos;
  Quaternion doorRotation, windowRotation;

  public override void enterState(EnemyController enemyController)
  {
    enemyPos = enemyController.transform.position;

    doorPos = enemyController.doorPos.transform.position;
    doorRotation = enemyController.doorPos.transform.rotation;

    windowPos = enemyController.windowPos.transform.position;
    windowRotation = enemyController.windowPos.transform.rotation;

    //update sate
    enemyController.currentState = enemyController.enemyPrepareState;

    if (enemyPos == doorPos)
    {
      Debug.Log("enemy at window");
      enemyController.transform.SetPositionAndRotation(windowPos, windowRotation);
      return;
    }
    else if (enemyPos == windowPos)
    {
      Debug.Log("enemy at door");
      enemyController.transform.SetPositionAndRotation(doorPos, doorRotation);
      return;
    }



    if (Random.Range(0, 2) == 0)
    {

      enemyController.transform.SetPositionAndRotation(doorPos, doorRotation);
      Debug.Log("enemy at door");
      return;
    }
    else
    {
      enemyController.transform.SetPositionAndRotation(windowPos, windowRotation);
      Debug.Log("enemy at window");
      return;
    }
  }

  public override void updateState(EnemyController enemyController)
  {
    if (Random.Range(0, 2) == 0)
    {
      enemyController.currentState = null;
      enemyController.SetBasePosition();
    }
    else
    {
      enemyController.currentState = enemyController.enemyAttackState;
      // enemyController.SetBasePosition();
    }
  }
}
