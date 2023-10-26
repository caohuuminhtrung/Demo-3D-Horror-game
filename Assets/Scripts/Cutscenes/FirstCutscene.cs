using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FirstCutscene : MonoBehaviour
{
  readonly Dialogues dialogues = new Dialogues();
  IEnumerator Dialogue(DialogueController dialogueController) {
    yield return dialogueController.ShowDialogue(dialogues.dialogues[0]);
    yield return null;
  }

  IEnumerator WakeUp(GameObject player) {
    player.GetComponent<Animator>().Play("WakeUp");
    yield return new WaitForSeconds(1f);
  }

  IEnumerator EnablePlayer(GameObject player) {
    player.GetComponent<PlayerController>().SetEnable();
    player.GetComponent<Animator>().enabled = false;
    yield return null;
  }

  public IEnumerator Play(DialogueController dialogueController, GameObject player)
  {
    yield return Dialogue(dialogueController);
    yield return new WaitForSeconds(0.1f);
    yield return WakeUp(player);
    yield return new WaitForSeconds(0.1f);
    yield return EnablePlayer(player);
  }
}
