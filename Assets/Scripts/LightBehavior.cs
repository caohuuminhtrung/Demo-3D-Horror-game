using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour
{
  bool isFlicker = true;
  bool preExplode = false;
  // bool stopFlicker = false;
  public GameObject[] lights;


  // Update is called once per frame
  void Update()
  {
    if (isFlicker)
    {
      StartCoroutine(LightFlicker());
    }

    if (preExplode)
    {
      StopCoroutine(LightFlicker());
      StartCoroutine(preExplodeFlicker());
    }
  }

  IEnumerator LightFlicker()
  {
    isFlicker = false;
    while (!isFlicker)
    {
      yield return new WaitForSeconds(Random.Range(0.8f, 1.89f));

      EnableLight(false);
      yield return new WaitForSeconds(Random.Range(0.15f, 0.59f));
      EnableLight(true);
      yield return new WaitForSeconds(Random.Range(0.3f, 1.34f));
      EnableLight(false);
      yield return new WaitForSeconds(Random.Range(0.19f, 0.68f));
      EnableLight(true);
    }
  }

  IEnumerator preExplodeFlicker()
  {
    preExplode = false;
    while (!preExplode)
    {
      yield return new WaitForSeconds(Random.Range(0.26f, 0.51f));

      EnableLight(false);
      yield return new WaitForSeconds(Random.Range(0.08f, 0.31f));
      EnableLight(true);
      yield return new WaitForSeconds(Random.Range(0.2f, 0.53f));
      EnableLight(false);
      yield return new WaitForSeconds(Random.Range(0.12f, 0.39f));
      EnableLight(true);
    }
  }

  public void EnableLight(bool isEnabled)
  {
    foreach (GameObject inviLight in lights)
    {
      inviLight.GetComponent<Light>().enabled = isEnabled;
    }
  }

  public void preExplodeLight()
  {
    preExplode = true;
  }

  public void DisableLight()
  {
    StopAllCoroutines();
    EnableLight(false);
  }
}
