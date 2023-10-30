using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour
{
  bool isFlicker = false;
  float delay;
  public GameObject[] lights;

  // Update is called once per frame
  void Update()
  {
    if (isFlicker == false) {
      StartCoroutine(LightFlicker());
    }
  }

  IEnumerator LightFlicker() {
    isFlicker = true;
    foreach(GameObject inviLight in lights) {
      inviLight.gameObject.GetComponent<Light>().enabled = false;
    }
    delay = Random.Range(0.1f, 1.5f);
    yield return new WaitForSeconds(delay);
    foreach (GameObject inviLight in lights)
    {
      inviLight.gameObject.GetComponent<Light>().enabled = true;
    }
    delay = Random.Range(0.1f, 1.5f);
    yield return new WaitForSeconds(delay);
    isFlicker = false;
  }
}
