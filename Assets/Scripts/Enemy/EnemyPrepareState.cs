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

    if (enemy.IsEnemyAtPrewindow())
    {
      if (Random.Range(0, 3) > 0)
      {
        Debug.Log("Play window knocking sounds");
        if (Random.Range(0, 2) == 0)
        {
          enemy.windowKnocking1.Play();
        }
        else enemy.windowKnocking2.Play();
      }
      //if enemy is near window then move to window
      Debug.Log("enemy at window");
      // enemy.transform.SetPositionAndRotation(windowPos, windowRotation);
      enemy.jumpscareWindowFlag = false;
      enemy.StartCoroutine(enemy.PlayCrawlAnimID());
      // return;
    }
    else if (enemy.IsEnemyAtHallway())
    {
      //if enemy is at hallway then move to door
      Debug.Log("enemy at door");
      enemy.transform.localScale = new Vector3(-4, enemy.transform.localScale.y, enemy.transform.localScale.z);
      enemy.transform.SetPositionAndRotation(doorPos, doorRotation);
      enemy.waveDoorFlag = false;
      enemy.StartCoroutine(enemy.PlayWaveAnimID());
    }
    else if (!enemy.IsEnemyAtWindow() && enemy.RNGcount == enemy.RNGlimit || enemy.IsEnemyAtDoor())
    {
      //if RNG counter reaches limit, jump into wardrobe
      Debug.Log("enemy jumped into closet");
      enemy.transform.SetPositionAndRotation(closetPos, closetRotation);
      enemy.PlayIdleAnimID();
      enemy.jumpscareClosetFlag = false;
      // return;
    }
  }


  public override void updateState(EnemyController enemy) { }
}
