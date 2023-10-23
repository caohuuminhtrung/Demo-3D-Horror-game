using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
  
  DoorBaseState currentState;
  DoorCloseState doorCloseState = new DoorCloseState();
  DoorOpenState doorOpenState = new DoorOpenState();

  // Start is called before the first frame update
  void Start()
  {
    currentState = doorCloseState;
    currentState.enterState(this);
  }

  // Update is called once per frame
  void Update()
  {
    // currentState.updateState(this);
  }

  public void switchState(DoorBaseState state) {
    currentState = state;
    state.enterState(this);
  }
}
