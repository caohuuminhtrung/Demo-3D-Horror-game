using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetManager : MonoBehaviour
{
  public ClosetBaseState currentState;
  public ClosetBaseState closetCloseState;
  public ClosetOpenState closetOpenState;
  public ClosetHalfCloseState closetHalfCloseState;
  public ClosetHalfOpenState closetHalfOpenState;

  public PlayerController player;

  // Start is called before the first frame update
  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    closetCloseState = gameObject.AddComponent<ClosetCloseState>();
    closetOpenState = gameObject.AddComponent<ClosetOpenState>();
    closetHalfCloseState = gameObject.AddComponent<ClosetHalfCloseState>();
    closetHalfOpenState = gameObject.AddComponent<ClosetHalfOpenState>();

    currentState = closetCloseState;
  }

  public void switchState(ClosetBaseState state)
  {
    if (player.doorHeld)
    {
      return;
    }
    currentState = state;
    state.enterState(this);
  }
}
