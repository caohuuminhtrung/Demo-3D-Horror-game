using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{
  public abstract void enterState(EnemyController enemyController);

  public abstract void updateState(EnemyController enemyController);
}
