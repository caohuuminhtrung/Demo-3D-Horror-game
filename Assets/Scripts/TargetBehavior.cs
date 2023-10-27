using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
  GameObject arrow;
  GameObject playerCamera;

  void Start() {
    transform.parent = GameObject.Find("Room").gameObject.transform;
    playerCamera = Camera.main.gameObject;
    arrow = transform.Find("Arrow").gameObject;
  }
  // Update is called once per frame
  void Update()
  {
    arrow.transform.LookAt(playerCamera.transform);
  }
}
