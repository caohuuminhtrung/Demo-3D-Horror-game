using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
  AudioSource audioSource;
  public float minMoveDistance = 0.1f;
  private Vector3 previousPosition;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    previousPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    float moveDistance = Vector3.Distance(transform.position, previousPosition);

    if (moveDistance >= minMoveDistance)
    {
    } else {
    }
  }
}
