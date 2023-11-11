using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClosetBaseState: MonoBehaviour
{
  public abstract void enterState(ClosetManager door);

  public abstract void updateState(ClosetManager door);
}
