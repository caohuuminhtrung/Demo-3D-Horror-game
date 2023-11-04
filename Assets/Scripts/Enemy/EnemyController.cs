using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public EnemyBaseState currentState;
  EnemyIdleState enemyIdleState;
  EnemyAttackState enemyAttackState;
  EnemyPrepareState enemyPrepareState;
    // Start is called before the first frame update
  void Start()
  {
    enemyIdleState = GetComponent<EnemyIdleState>();
    enemyAttackState = GetComponent<EnemyAttackState>();
    enemyPrepareState = GetComponent<EnemyPrepareState>();

  }

  IEnumerator EnemyOneBehaviour(int level) {
    while (true) {
      yield return new WaitForSeconds(4f);

      int movement = Random.Range(1, 30);

      if (level < movement && currentState.GetType().Equals(typeof(EnemyIdleState))) {
        SwitchState(enemyPrepareState);
      }
    }
  }

  public void SwitchState(EnemyBaseState state)
  {
    currentState = state;
    state.enterState(this);
  }

}
