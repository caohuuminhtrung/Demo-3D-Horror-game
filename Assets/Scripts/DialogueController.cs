using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
  public GameObject canvas;
  public PlayerController player;

  public IEnumerator ShowDialogue(string[] messages) {
    player.GetComponent<PlayerController>().SetDisable();
    canvas.transform.Find("Crosshair").gameObject.SetActive(false);
    canvas.GetComponent<Image>().color = Color.black;
    canvas.GetComponent<Image>().CrossFadeAlpha(1, 0, true);
    foreach (string message in messages) {
      canvas.transform.Find("Dialogue").gameObject.GetComponent<TMP_Text>().text = message;
      yield return new WaitForSeconds(2.5f);
    }
    canvas.transform.Find("Dialogue").gameObject.GetComponent<TMP_Text>().text = "";
    canvas.GetComponent<Image>().color = Color.clear;
    canvas.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
    canvas.transform.Find("Crosshair").gameObject.SetActive(true);
    player.GetComponent<PlayerController>().SetIdle();
  }
}
