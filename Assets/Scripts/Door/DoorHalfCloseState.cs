using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHalfCloseState : DoorBaseState
{
    // Start is called before the first frame update
    public override void enterState(DoorManager door)
    {
        door.transform.parent.GetComponent<BoxCollider>().enabled = true;
        if (door.transform.parent.rotation.y == 0)
        {
            return;
        }
        door.GetComponent<Animator>().Play("DoorHalfClose");
    }

    public override void updateState(DoorManager door) { }

}
