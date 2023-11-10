using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHalfOpenState : DoorBaseState
{
    // Start is called before the first frame update
    public override void enterState(DoorManager door)
    {
        door.transform.parent.GetComponent<BoxCollider>().enabled = true;
        if (door.transform.parent.rotation.y == 20)
        {
            return;
        }
        door.openSound.Play();
        door.GetComponent<Animator>().Play("DoorHalfOpen");
    }

    public override void updateState(DoorManager door) { }

}
