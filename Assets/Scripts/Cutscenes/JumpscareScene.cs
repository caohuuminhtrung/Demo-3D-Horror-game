using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareScene : MonoBehaviour
{
  public IEnumerator Play(GameObject player, GameObject enemy, GameObject animationHolder)
  {
    player.GetComponent<PlayerController>().SetDisable();
    // player.transform.Find("Main Camera").transform.rotation = new Quaternion(0, 0, 0, 0);
    // player.GetComponent<CapsuleCollider>().enabled = false;
    Cursor.lockState = CursorLockMode.Locked;
    animationHolder.transform.position = player.transform.position;
    animationHolder.transform.rotation = player.transform.rotation;
    player.transform.parent = animationHolder.transform;
    player.GetComponent<Animator>().enabled = true;
    player.GetComponent<Animator>().Play("Camera Shake");
    enemy.transform.position = Camera.main.transform.position + Camera.main.transform.forward + new Vector3(0, -6.5f, 0);
    enemy.transform.LookAt(Camera.main.transform);
    enemy.transform.eulerAngles += new Vector3(45f, 0, 0);
    player.transform.Find("JumpscareSound").GetComponent<AudioSource>().Play();
    enemy.GetComponent<EnemyController>().PlayAttackAnimID();
    yield return new WaitForSeconds(6f);
    // player.transform.parent = null;
  }
}
