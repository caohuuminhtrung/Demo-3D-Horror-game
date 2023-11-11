using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitialState : EnemyBaseState
{
    int wardrobeMovementRNG;
    public override void enterState(EnemyController enemy)
    {
        wardrobeMovementRNG = enemy.RNGcount;
        //Set enemy initial position
        Debug.Log("Enemy back to base");
        enemy.SetBasePosition();
    }


    public override void updateState(EnemyController enemy)
    {
        if (wardrobeMovementRNG == 30)
        {
            enemy.SwitchState(enemy.enemyPrepareState);
        }
        else
        {
            //Switch to idle state
            enemy.SwitchState(enemy.enemyIdleState);
        }

    }
}
