using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
  
  public DoorBaseState currentState;
  DoorCloseState doorCloseState;
  DoorOpenState doorOpenState;

  // Start is called before the first frame update
  void Start()
  {
    doorCloseState = gameObject.AddComponent<DoorCloseState>();
    doorOpenState = gameObject.AddComponent<DoorOpenState>();
    
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
