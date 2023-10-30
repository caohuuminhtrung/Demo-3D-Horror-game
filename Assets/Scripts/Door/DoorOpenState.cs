using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenState : DoorBaseState
{
  public override void enterState(DoorManager door) {
    door.transform.localEulerAngles = new Vector3(0, 90, 0);
  }

  public override void updateState(DoorManager door)
  {
    
  }
}
