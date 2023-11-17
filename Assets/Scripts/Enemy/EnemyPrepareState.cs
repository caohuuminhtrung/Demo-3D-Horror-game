using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyPrepareState : EnemyBaseState
{
  Vector3 doorPos, closetPos;
  Quaternion doorRotation, closetRotation;

  public override void enterState(EnemyController enemy)
  {
    doorPos = enemy.doorPos.transform.position;
    doorRotation = enemy.doorPos.transform.rotation;

    closetPos = enemy.closetPos.transform.position;
    closetRotation = enemy.closetPos.transform.rotation;

    // enemy.StopEnemyBehaviour();

    if (enemy.isEnemyAtPrewindow())
    {
      if (Random.Range(0, 2) == 0)
      {
        if (Random.Range(0, 2) == 0)
        {
          enemy.windowKnocking1.Play();
        }
        else enemy.windowKnocking2.Play();
      }
      //if enemy is near window then move to window
      Debug.Log("enemy at window");
      // enemy.transform.SetPositionAndRotation(windowPos, windowRotation);
      enemy.StartCoroutine(enemy.playCrawlAnimID());
      // return;
    }
    else if (enemy.isEnemyAtHallway())
    {
      //if enemy is at hallway then move to door
      Debug.Log("enemy at door");
      enemy.transform.localScale = new Vector3(-4, enemy.transform.localScale.y, enemy.transform.localScale.z);
      enemy.transform.SetPositionAndRotation(doorPos, doorRotation);
      enemy.StartCoroutine(enemy.playWaveAnimID());
    }
    else if (!enemy.isEnemyAtWindow() && enemy.RNGcount == enemy.RNGlimit)
    {
      //if RNG counter reaches limit, jump into wardrobe
      Debug.Log("enemy jumped into closet");
      enemy.transform.SetPositionAndRotation(closetPos, closetRotation);
      enemy.playIdleAnimID();
      // return;
    }
  }


  public override void updateState(EnemyController enemy) { }
}
