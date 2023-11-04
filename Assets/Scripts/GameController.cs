using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public GameObject canvas;
  public PlayerController player;
  public GameObject enemy;
  public GameObject animationHolder;
  DialogueController dialogueController;
  readonly Dialogues dialogues = new();
  FirstCutscene firstCutscene;
  SecondCutscene secondCutscene;
  GameObject pickedUpObject;
  public GameObject target;

  ItemDictionary itemDictionary;
  int stage = 1;
  bool playStage = true;
  JumpscareScene jumpscareScene;
  // Start is called before the first frame update
  void Start()
  {
    itemDictionary = new ItemDictionary();
    firstCutscene = gameObject.AddComponent<FirstCutscene>();
    secondCutscene = gameObject.AddComponent<SecondCutscene>();
    jumpscareScene = gameObject.AddComponent<JumpscareScene>();

    dialogueController = gameObject.AddComponent<DialogueController>();
    dialogueController.canvas = canvas;
    dialogueController.player = player.GetComponent<PlayerController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.T)) {
      StartCoroutine(jumpscareScene.Play(player.gameObject, enemy, animationHolder));
    }
    if (stage == 1 && playStage) {
      StartCoroutine(firstCutscene.Play(dialogueController, player.gameObject));
      stage = 2;
      playStage = false;
    }
    if (stage == 2 && playStage) {
      HidePrompt();
      StartCoroutine(dialogueController.ShowDialogue(dialogues.dialogues[1]));

      GameObject.Find("Pill Bottle Pickup").gameObject.layer = LayerMask.NameToLayer("Interactive Objects");
      playStage = false;
      stage = 3;
    }
    if (stage == 3 && playStage) {
      HidePrompt();
      StartCoroutine(dialogueController.ShowDialogue(dialogues.dialogues[2]));
      GameObject.Find("Bed").layer = LayerMask.NameToLayer("Interactive Objects");
      
      playStage = false;
      stage = 4;
    }
    if (stage == 4 && playStage) {
      HidePrompt();
      StartCoroutine(secondCutscene.Play(dialogueController, player.gameObject));
      stage = 5;
      playStage = false;
    }
  }

  public void ShowPrompt(GameObject hitObject) {
    if ((hitObject.CompareTag("Player") && player.GetIsHolding()) ||
        (hitObject.CompareTag("Target") && !player.GetIsHolding())) {
      canvas.transform.Find("Prompt").GetComponent<TMP_Text>().text = "";
      return;
    }
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
    if (hitObject.CompareTag("Item") && !player.GetIsHolding()) {
      player.SetIsHolding(true);
      Instantiate(target, itemDictionary.itemDictionary[hitObject.name], Quaternion.identity);
      pickedUpObject = hitObject;
      pickedUpObject.AddComponent<PickupBehavior>();
      pickedUpObject.GetComponent<PickupBehavior>().parent = player.gameObject;
    }
    if (hitObject.CompareTag("Target") && player.GetIsHolding()) {
      player.SetIsHolding(false);
      Destroy(pickedUpObject.GetComponent<PickupBehavior>());
      pickedUpObject.transform.parent = GameObject.Find("Room").transform;
      pickedUpObject.transform.SetPositionAndRotation(hitObject.transform.position, hitObject.transform.rotation);
      pickedUpObject.layer = LayerMask.NameToLayer("Default");
      Destroy(hitObject.transform.parent.gameObject);
      playStage = true;
    }
    if (hitObject.CompareTag("Bed")) {
      playStage = true;
      hitObject.layer = LayerMask.NameToLayer("Default");
    }
  }

  public void OpenDoor(GameObject hitObject) {
    if (hitObject.CompareTag("Door")) {
      if (hitObject.GetComponent<DoorManager>().currentState.GetType() == typeof(DoorCloseState)) {
        hitObject.GetComponent<DoorManager>().switchState(new DoorOpenState());
      } 
  }}

  public void CloseDoor(GameObject hitObject) {
    if (hitObject != null) {
        if (hitObject.CompareTag("Door")) {
            if (hitObject.GetComponent<DoorManager>().currentState.GetType() == typeof(DoorOpenState)) {
                hitObject.GetComponent<DoorManager>().switchState(new DoorCloseState());
            } 
        }
    }
  }
}
