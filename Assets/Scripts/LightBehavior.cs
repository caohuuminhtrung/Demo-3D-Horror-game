using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour
{
  bool isFlicker = false;
  float delay;
  public GameObject[] lights;
  private bool stopFlicker = false;


  // Update is called once per frame
  void Update()
  {
    if (!isFlicker && !stopFlicker) {
      StartCoroutine(LightFlicker());
    }
  }

  IEnumerator LightFlicker() {
    isFlicker = true;
    foreach(GameObject inviLight in lights) {
      inviLight.GetComponent<Light>().enabled = false;
    }
    delay = Random.Range(0.1f, 1.5f);
    yield return new WaitForSeconds(delay);
    if (stopFlicker) {
      yield return null;
    }
    foreach (GameObject inviLight in lights)
    {
      inviLight.GetComponent<Light>().enabled = true;
    }
    if (stopFlicker)
    {
      DisableLight();
      yield return null;
    }
    delay = Random.Range(0.1f, 1.5f);
    yield return new WaitForSeconds(delay);
    isFlicker = false;
  }

  public void DisableLight() {
    stopFlicker = true;
    foreach (GameObject inviLight in lights)
    {
      inviLight.GetComponent<Light>().enabled = false;
    }
  }
}
