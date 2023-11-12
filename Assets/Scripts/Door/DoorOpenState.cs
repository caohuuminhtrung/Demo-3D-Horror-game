using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenState : DoorBaseState
{
  public override void enterState(DoorManager door) {
    door.transform.parent.GetComponent<BoxCollider>().enabled = false;
    if(door.transform.parent.rotation.y == 90){
      return;
    }
    door.GetComponent<Animator>().Play("DoorOpen");
  }

  public override void updateState(DoorManager door){}
}
