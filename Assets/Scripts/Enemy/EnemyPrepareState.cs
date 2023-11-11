using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyPrepareState : EnemyBaseState
{
  Vector3 enemyPos, initialPos, hallwayPos, preWindowPos, doorPos, windowPos, wardrobePos;
  Quaternion doorRotation, windowRotation, wardrobeRotation;
  float counter = 0;
  EnemyController enemyController;
  DoorManager doorController;
  void Update()
  {
    enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    if (enemyController.currentState.GetType().Equals(typeof(EnemyPrepareState)))
    {
      counter += Time.deltaTime;
      Debug.Log(counter);
    }
  }

  public override void enterState(EnemyController enemy)
  {

    enemyPos = enemy.transform.position;

    initialPos = enemy.basePos.transform.position;

    hallwayPos = enemy.hallwayPos.transform.position;
    preWindowPos = enemy.preWindowPos.transform.position;

    doorPos = enemy.doorPos.transform.position;
    doorRotation = enemy.doorPos.transform.rotation;

    windowPos = enemy.windowPos.transform.position;
    windowRotation = enemy.windowPos.transform.rotation;

    wardrobePos = enemy.WardrobePos.transform.position;
    wardrobeRotation = enemy.WardrobePos.transform.rotation;


    if (enemyPos == preWindowPos)
    {

      //if enemy is near window then move to window
      Debug.Log("enemy at window");
      enemy.transform.SetPositionAndRotation(windowPos, windowRotation);
      enemy.playCrawlAnimID();
      // return;
    }
    else if (enemyPos == hallwayPos)
    {

      //if enemy is at hallway then move to door
      Debug.Log("enemy at door");
      enemy.transform.SetPositionAndRotation(doorPos, doorRotation);
      doorController = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorManager>();
      doorController.switchState(doorController.doorHalfOpenState);
      enemy.playWaveAnimID();
      if (Random.Range(0, 5) == 0)
      {
        enemy.playWaveAnimID();
      }
      // return;
    }
    else if (enemyPos == initialPos || enemy.RNGcount == enemy.RNGlimit)
    {
      Debug.Log("enemy jumped into wardrobe");
      enemy.transform.SetPositionAndRotation(wardrobePos, wardrobeRotation);
      // return;
    }
  }


  public override void updateState(EnemyController enemy)
  {
    return;
  }


}
