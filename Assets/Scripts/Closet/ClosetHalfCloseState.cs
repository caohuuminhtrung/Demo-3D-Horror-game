using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHalfCloseState : ClosetBaseState
{

  public override void enterState(ClosetManager closet)
  {
    if (closet.transform.parent.rotation.z == 0)
    {
      Debug.Log("0 - 0");
      return;
    }
    closet.transform.parent.Find("door_L").GetComponent<Animator>().Play("DoorHalfClose1");
    closet.transform.parent.Find("door_R").GetComponent<Animator>().Play("DoorHalfClose2");
  }

  public override void updateState(ClosetManager door) { }
}
