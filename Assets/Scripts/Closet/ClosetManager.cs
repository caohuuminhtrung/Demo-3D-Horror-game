using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetManager : MonoBehaviour
{
  public ClosetBaseState currentState;
  ClosetBaseState closetCloseState;

  // Start is called before the first frame update
  void Start()
  {
    closetCloseState = gameObject.AddComponent<ClosetCloseState>();
    currentState = closetCloseState;
  }

  // Update is called once per frame
  void Update()
  {
      
  }

  public void switchState(ClosetBaseState state)
  {
    currentState = state;
    state.enterState(this);
  }
}
