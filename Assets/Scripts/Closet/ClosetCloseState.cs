using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetCloseState : ClosetBaseState
{

  public override void enterState(ClosetManager closet)
  {
    closet.transform.parent.Find("door_L").GetComponent<Animator>().Play("DoorClose 1");
    closet.transform.parent.Find("door_R").GetComponent<Animator>().Play("DoorClose 2");
  }

  public override void updateState(ClosetManager door) { }
}
