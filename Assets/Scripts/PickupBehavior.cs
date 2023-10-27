using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
  public GameObject parent;
  void Start() {
    transform.parent = parent.transform;
  }
  // Update is called once per frame
  void Update()
  {
    transform.localEulerAngles = new Vector3(0, 0, 0);
    transform.localPosition = new Vector3(0, 3, 2.5f);
  }
}
