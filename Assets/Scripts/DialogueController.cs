using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
  public GameObject canvas;

  public IEnumerator ShowDialogue(string[] messages) {
    canvas.GetComponent<Image>().color = Color.black;
    foreach (string message in messages) {
      canvas.transform.Find("Dialogue").gameObject.GetComponent<TMP_Text>().text = message;
      yield return new WaitForSeconds(2.5f);
    }
    canvas.transform.Find("Dialogue").gameObject.GetComponent<TMP_Text>().text = "";
    canvas.GetComponent<Image>().color = Color.clear;
    yield return new WaitForSeconds(0.1f);
  }
}
