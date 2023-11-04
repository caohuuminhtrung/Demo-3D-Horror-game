using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

  public GameObject windowPos, preWindowPos, hallwayPos, doorPos, basePos, player;
  public EnemyBaseState currentState;
  public EnemyIdleState enemyIdleState;
  public EnemyAttackState enemyAttackState;
  public EnemyPrepareState enemyPrepareState;

  // Start is called before the first frame update
  void Start()
  {
    enemyIdleState = GetComponent<EnemyIdleState>();
    enemyAttackState = GetComponent<EnemyAttackState>();
    enemyPrepareState = GetComponent<EnemyPrepareState>();

    player = GameObject.FindGameObjectWithTag("Player");

    basePos = GameObject.FindGameObjectWithTag("BaseMovementPos");
    windowPos = GameObject.FindGameObjectWithTag("WindowMovementPos");
    preWindowPos = GameObject.FindGameObjectWithTag("PreWindowMovementPos");
    hallwayPos = GameObject.FindGameObjectWithTag("HallwayMovementPos");
    doorPos = GameObject.FindGameObjectWithTag("DoorMovementPos");

    SetBasePosition();

    Debug.Log("enemy Start");
    StartCoroutine(EnemyOneBehaviour(15));
  }

  IEnumerator EnemyOneBehaviour(int level)
  {
    Debug.Log("runnning behaviour");

    while (true)
    {
      yield return new WaitForSeconds(4f);

      int movement = Random.Range(1, 21);

      // if (level >= movement && currentState.GetType().Equals(typeof(EnemyIdleState)))
      if (level >= movement)
      {
        if (transform.position == basePos.transform.position)
        {
          Debug.Log("moving from base to idle");
          SwitchState(enemyIdleState);
        }
        // else if (transform.position == preWindowPos.transform.position || transform.position == hallwayPos.transform.position)
        else if (currentState.GetType().Equals(typeof(EnemyIdleState)))
        {
          if (Random.Range(0, 2) == 0)
          {
            Debug.Log("Moving from idle back to idle");
            SwitchState(enemyIdleState);
          }
          else
          {
            Debug.Log("moving from idle to prepare");
            SwitchState(enemyPrepareState);
          }

        }
        // else if (transform.position == hallwayPos.transform.position || transform.position == doorPos.transform.position)
        else if (currentState.GetType().Equals(typeof(EnemyPrepareState)))
        {
          //Nếu player đóng cửa hoặc núp vào tủ thì sẽ quay lại state idle.
          //Còn không thì attack luôn
          Debug.Log("Moving from prepare to attack");
          SwitchState(enemyAttackState);
        }
        else if (currentState.GetType().Equals(typeof(EnemyAttackState)))
        {
          SetBasePosition();
        }
        // Debug.Log("update to new gamestate");
        // currentState.updateState(this);
        // SwitchState(enemyPrepareState);
        // transform.position = windowEnemyPos.transform.position;
        // transform.rotation = windowEnemyPos.transform.rotation;
      }
      else if (level < movement)
      {
        Debug.Log("enemy failed to move");
      }
    }
  }

  public void SwitchState(EnemyBaseState state)
  {
    currentState = state;
    state.enterState(this);
  }

  public void SetBasePosition()
  {
    transform.SetPositionAndRotation(basePos.transform.position, basePos.transform.rotation);
  }
}
