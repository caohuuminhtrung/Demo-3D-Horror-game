using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHalfOpenState : ClosetBaseState
{
  public override void enterState(ClosetManager closet)
  {
    if (closet.transform.parent.rotation.z == 16)
    {
      Debug.Log("20 - 20");
      return;
    }
    closet.transform.parent.Find("door_L").GetComponent<Animator>().Play("DoorHalfOpen1");
    closet.transform.parent.Find("door_R").GetComponent<Animator>().Play("DoorHalfOpen2");
  }

  public override void updateState(ClosetManager door) { }
}
