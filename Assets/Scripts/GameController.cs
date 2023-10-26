using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public GameObject canvas;
  public GameObject player;
  DialogueController dialogueController;
  FirstCutscene firstCutscene;

  // Start is called before the first frame update
  void Start()
  {
    player.GetComponent<PlayerController>().SetDisable();
    firstCutscene = gameObject.AddComponent<FirstCutscene>();
    dialogueController = gameObject.AddComponent<DialogueController>();
    dialogueController.canvas = canvas;
    StartCoroutine(firstCutscene.Play(dialogueController, player));
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void ShowPrompt(GameObject hitObject) {
    canvas.transform.Find("Prompt").GetComponent<TMP_Text>().text = "Press E to interact";
  }

  public void HidePrompt() {
    canvas.transform.Find("Prompt").GetComponent<TMP_Text>().text = "";
  }

  public void InteractObject(GameObject hitObject) {
    if (hitObject.CompareTag("Door")) {
      if (hitObject.GetComponent<DoorManager>().currentState.GetType() == typeof(DoorCloseState)) {
        hitObject.GetComponent<DoorManager>().switchState(new DoorOpenState());
      } else {
        hitObject.GetComponent<DoorManager>().switchState(new DoorCloseState());
      }
    }
  }

}
