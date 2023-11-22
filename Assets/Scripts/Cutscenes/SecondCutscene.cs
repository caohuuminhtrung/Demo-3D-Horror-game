using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class SecondCutscene : MonoBehaviour
{
  AudioSource enemyfootStep;
  EnemyController enemyController;
  FadeController fadeController = new FadeController();
  readonly Dialogues dialogues = new Dialogues();
  public bool lightExploded = false;
  IEnumerator FadeIn(GameObject player)
  {
    enemyfootStep = GameObject.FindGameObjectWithTag("HallwayMovementPos").transform.GetChild(0).GetComponent<AudioSource>();
    player.GetComponent<PlayerController>().SetDisable();
    fadeController.blackScreen = GameObject.Find("Canvas").GetComponent<Image>();
    fadeController.SetBlack();
    StartCoroutine(fadeController.FadeIn());
    player.transform.position = new Vector3(7.7f, 2.5f, -3f);
    player.transform.eulerAngles = new Vector3(-90, 0, 0);
    yield return null;
  }

  IEnumerator WakeUp(GameObject player)
  {
    player.GetComponent<Animator>().enabled = true;
    player.GetComponent<Animator>().Play("WakeUp");
    yield return new WaitForSeconds(1f);
  }

  IEnumerator EnablePlayer(GameObject player)
  {
    player.GetComponent<PlayerController>().SetIdle();
    player.GetComponent<Animator>().enabled = false;
    yield return null;
  }

  public IEnumerator LightExplode(GameObject light)
  {
    light.GetComponent<LightBehavior>().preExplodeLight();
    yield return new WaitForSeconds(4f);
    light.GetComponent<LightBehavior>().DisableLight();
    light.GetComponent<AudioSource>().Play();
    yield return new WaitForSeconds(0.5f);
    lightExploded = true;
  }

  public IEnumerator Play(DialogueController dialogueController, GameObject player)
  {
    GameObject light = GameObject.Find("Room Light");
    yield return FadeIn(player);
    yield return new WaitForSeconds(3f);
    yield return fadeController.FadeOut();
    yield return WakeUp(player);
    yield return new WaitForSeconds(1f);
    yield return EnablePlayer(player);
    yield return new WaitForSeconds(1f);
    yield return dialogueController.ShowDialogue(dialogues.dialogues[3]);
    yield return new WaitForSeconds(1.5f);
    light.GetComponent<LightBehavior>().enabled = true;
    yield return new WaitForSeconds(4.6f);
    enemyfootStep.Play();
    yield return dialogueController.ShowDialogue(dialogues.dialogues[4]);
    yield return new WaitForSeconds(5.5f);
    yield return LightExplode(light);

  }
}
