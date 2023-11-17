using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

  public DoorBaseState currentState;
  public DoorCloseState doorCloseState;
  public DoorOpenState doorOpenState;
  public DoorHalfOpenState doorHalfOpenState;
  public DoorHalfCloseState doorHalfCloseState;

  public AudioSource knockSound;
  public AudioSource openSound;

  // Start is called before the first frame update
  void Awake()
  {
    doorOpenState = gameObject.AddComponent<DoorOpenState>();
    doorCloseState = gameObject.AddComponent<DoorCloseState>();
    doorHalfOpenState = gameObject.AddComponent<DoorHalfOpenState>();
    doorHalfCloseState = gameObject.AddComponent<DoorHalfCloseState>();

    currentState = doorCloseState;
    currentState.enterState(this);
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void switchState(DoorBaseState state)
  {
    currentState = state;
    state.enterState(this);
  }
}
