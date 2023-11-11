using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

  public DoorBaseState currentState;
  DoorCloseState doorCloseState;
  public DoorHalfOpenState doorHalfOpenState;

  public AudioSource knockSound;
  public AudioSource openSound;

  // Start is called before the first frame update
  void Start()
  {
    doorCloseState = gameObject.AddComponent<DoorCloseState>();
    doorHalfOpenState = gameObject.AddComponent<DoorHalfOpenState>();

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
