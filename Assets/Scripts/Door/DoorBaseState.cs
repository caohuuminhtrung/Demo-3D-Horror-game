using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorBaseState : MonoBehaviour
{
  public abstract void enterState(DoorManager door);

  public abstract void updateState(DoorManager door);
}
