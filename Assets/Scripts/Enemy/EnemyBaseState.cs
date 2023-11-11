using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{
  public abstract void enterState(EnemyController enemy);

  public abstract void updateState(EnemyController enemy);
}
