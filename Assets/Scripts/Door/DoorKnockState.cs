using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockState : DoorBaseState
{
  public override void enterState(DoorManager door)
  {
    door.knockSound.Play();
  }

  public override void updateState(DoorManager door)
  {

  }
}
