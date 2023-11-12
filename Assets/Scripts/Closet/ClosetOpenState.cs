using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetOpenState : ClosetBaseState
{
  public override void enterState(ClosetManager closet)
  {
    closet.transform.parent.Find("door_L").GetComponent<Animator>().Play("DoorOpen 1");
    closet.transform.parent.Find("door_R").GetComponent<Animator>().Play("DoorOpen 2");
  }

  public override void updateState(ClosetManager door) { }
}
