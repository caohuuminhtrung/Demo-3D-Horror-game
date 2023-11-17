using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class EnemyAttackState : EnemyBaseState
{
  AudioSource jumpscareSound;
  public override void enterState(EnemyController enemy)
  {
    Debug.Log("attack!");
    enemy.SetBasePosition();
    enemy.StopAllCoroutines();

    enemy.StartCoroutine(enemy.playAttackAnimID());
  }


  public override void updateState(EnemyController enemyController)
  {
    return;
  }

}
