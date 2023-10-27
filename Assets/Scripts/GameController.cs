using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public GameObject canvas;
  public PlayerController player;
  DialogueController dialogueController;
  FirstCutscene firstCutscene;
  GameObject pickedUpObject;
  public GameObject target;

  ItemDictionary itemDictionary;
  // Start is called before the first frame update
  void Start()
  {
    itemDictionary = new ItemDictionary();
    player.SetDisable();
    firstCutscene = gameObject.AddComponent<FirstCutscene>();
    dialogueController = gameObject.AddComponent<DialogueController>();
    dialogueController.canvas = canvas;
    StartCoroutine(firstCutscene.Play(dialogueController, player.gameObject));
  }

  // Update is called once per frame
  void Update()
  {
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
      Debug.Log(itemDictionary.itemDictionary[hitObject.name]);
      Instantiate(target, itemDictionary.itemDictionary[hitObject.name], Quaternion.identity);
      pickedUpObject = hitObject;
      pickedUpObject.AddComponent<PickupBehavior>();
      pickedUpObject.GetComponent<PickupBehavior>().parent = player.gameObject;
    }
    if (hitObject.CompareTag("Target") && player.GetIsHolding()) {
      player.SetIsHolding(false);
      Destroy(pickedUpObject.GetComponent<PickupBehavior>());
      pickedUpObject.transform.parent = GameObject.Find("Room").gameObject.transform;
      pickedUpObject.transform.SetPositionAndRotation(hitObject.transform.position, hitObject.transform.rotation);
      Destroy(hitObject.transform.parent.gameObject);
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
