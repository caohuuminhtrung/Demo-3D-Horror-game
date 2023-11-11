using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
  Vector3 hallwayPos, preWindowPos;
  Quaternion hallwayRotation, preWindowRotation;

  public override void enterState(EnemyController enemy)
  {
    hallwayPos = enemy.hallwayPos.transform.position;
    hallwayRotation = enemy.hallwayPos.transform.rotation;

    preWindowPos = enemy.preWindowPos.transform.position;
    preWindowRotation = enemy.preWindowPos.transform.rotation;

    //randomly choosing between appearing at hallway or near window
    if (Random.Range(0, 2) == 0)
    {
      //enemy apprearing at hallway
      enemy.transform.SetPositionAndRotation(hallwayPos, hallwayRotation);
      enemy.playIdleAnimID();
      Debug.Log("enemy at hallway");
      return;
    }
    else
    {
      if (Random.Range(0, 3) == 0)
      {
        if (Random.Range(0, 2) == 0)
        {
          enemy.windowKnocking1.Play();
        }
        else enemy.windowKnocking2.Play();
      }
      //enemy appearing at window
      enemy.transform.SetPositionAndRotation(preWindowPos, preWindowRotation);
      enemy.playIdleAnimID();
      Debug.Log("enemy near window");
      return;
    }
  }

  public override void updateState(EnemyController enemy)
  {
    //randomly move back to prepare state or initial state
    if (Random.Range(0, 2) == 0)
    {
      enemy.SwitchState(enemy.enemyInitialState);
    }
    else
    {
      enemy.SwitchState(enemy.enemyPrepareState);
    }
  }
}
