using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseState : DoorBaseState
{
  public override void enterState(DoorManager door) {
    door.transform.parent.GetComponent<BoxCollider>().enabled = true;
    if(door.transform.parent.rotation.y == 0){
      return;
    }
    door.GetComponent<Animator>().Play("DoorClose");
    //door.transform.localEulerAngles = new Vector3(0, 0, 0);
  }

  public override void updateState(DoorManager door) {}
}
